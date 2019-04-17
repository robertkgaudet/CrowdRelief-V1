using System;
using System.Linq;
using System.Web.Security;
using CrowdRelief;
using System.Web.UI.WebControls;

public partial class V1_Register : System.Web.UI.Page
{
	public string disasterDropDown	= string.Empty;
	public string preselectedDisasterJQuery = string.Empty;
	public string eventId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		eventId = Request.QueryString["eventId"];
		Master.PageTitle = "Sign Up for CrowdRelief - Disaster Recovery Platform";
		Master.PageDescription = "The worst thing that can happen after a natural disaster, is to be forgotten. CrowdRelief is a story telling platform that enables you track your recovery and connects you with helpers to tell your story. Easily share your story which can include your Amazon Wish Lists and other critical information about your current situation.";
		Master.FbImage = "S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType = ".jpg";

		if(User.Identity.IsAuthenticated)
		{
			Redirect();
		}

		if(!IsPostBack)
		{
			LoadDisasters();
			string memberType = Request.QueryString["type"];

			if(!String.IsNullOrEmpty(memberType))
			{
				if(memberType == "survivor")
				{
					//Check for survivor, put into the survivor role.
				}
				else if(memberType == "helper")
				{
					//Check for member, put into the helper role.
				}
				else if(memberType == "nonprofitadministrator")
				{
					//Check for member, put into the helper role.
				}
			}
		}
	}
	protected void Redirect()
	{
		string urlRedirect = "/Survivor";
		if(Roles.IsUserInRole("survivor"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=survivor";
		}
		if(Roles.IsUserInRole("nonprofitadministrator"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=nonprofit";
		}
		if(Roles.IsUserInRole("business") || Roles.IsUserInRole("contractor"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=business";
		}
		if(Roles.IsUserInRole("helper") || Roles.IsUserInRole("volunteer"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=helper";
		}

		Response.Redirect(urlRedirect);
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		MembershipCreateStatus status;
		string passwordQuestion = "What time is lunch?";
		string firstName = txtFirstName.Text;
		string lastName = txtLastName.Text;
		string password = txtPassword.Text;
		string username = txtUsername.Text;
		string phoneNumber = txtPhoneNumber.Text;
		string passwordAnswer = "12:00";
		string email = txtEmail.Text;
		string urlRedirect = string.Empty;
		string eventName = string.Empty;

		MembershipUser newUser = Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, true, out status);

		if (newUser == null)
		{
			litError.Text = GetErrorMessage(status);
			lblMessage.Visible = false;
			divError.Visible = true;
		}
		else
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			if(!String.IsNullOrEmpty(hidEventId.Value))
			{
				UserEvent userEvent = new UserEvent();
				userEvent.EventId = new Guid(hidEventId.Value);
				userEvent.UserEventId = Guid.NewGuid();
				userEvent.UserId = new Guid(newUser.ProviderUserKey.ToString());
				dc.UserEvents.InsertOnSubmit(userEvent);
				dc.SubmitChanges();


				var eventNameValue = (from u in dc.Events
								where u.EventId == new Guid(hidEventId.Value)
								select new { u.URLFriendlyName }).SingleOrDefault();

				eventName = eventNameValue.URLFriendlyName;
			}

			if(rdMemberTypeSurvivor.Checked)
			{
				Roles.AddUserToRole(username, "Survivor");
				//Send survivor to the survivor disaster page
				urlRedirect = eventName = String.IsNullOrEmpty(eventName) ? "/V1/DisasterList.aspx?userType=survivor" : "/" + eventName + "/Survivor";
			}

			if(rdMemberTypeHelper.Checked)
			{
				//Send helpers to the 
				Roles.AddUserToRole(username, "Helper");
				Roles.AddUserToRole(username, "Volunteer");
				urlRedirect = eventName = String.IsNullOrEmpty(eventName) ? "/V1/DisasterList.aspx?userType=helper" : "/" + eventName + "/Helper";
			}
			
			if(rdMemberTypeNonProfit.Checked)
			{
				Roles.AddUserToRole(username, "Helper");
				Roles.AddUserToRole(username, "Volunteer");
				Roles.AddUserToRole(username, "NonProfitAdministrator");
				urlRedirect = eventName = String.IsNullOrEmpty(eventName) ? "/V1/DisasterList.aspx?userType=nonprofit" : "/" + eventName + "/NonProfit";
			}

			if(rdMemberTypeBusiness.Checked)
			{
				Roles.AddUserToRole(username, "Business");
				urlRedirect = eventName = String.IsNullOrEmpty(eventName) ? "/V1/DisasterList.aspx?userType=business" : "/" + eventName + "/Business";
			}

			Roles.AddUserToRole(username, "Member");

			//Create a profile for this user.
			Profile userProfile			= new Profile();
			userProfile.UserId			= new Guid(newUser.ProviderUserKey.ToString());
			userProfile.ProfileId		= Guid.NewGuid();
			userProfile.Firstname		= firstName;
			userProfile.Lastname		= lastName;
			userProfile.PhoneNumber		= phoneNumber;
			dc.Profiles.InsertOnSubmit(userProfile);
			dc.SubmitChanges();

			Tools.SendEmail(firstName, lastName, "~\\EmailTemplates\\CreateAccount.html", "Welcome to Crowd Relief", email, this);

			// Log the user into the site
			FormsAuthentication.SetAuthCookie(username, true);
			Response.Redirect(urlRedirect);
		}
	}
	
	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						orderby d.BeginDate descending
						select new {d };

		int idNumber = 0;
		foreach(var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate +  "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
		if(!String.IsNullOrEmpty(eventId))
		{
			//Hide the Dropdown and show the selected disaster
			var disaster = (from d in dc.Events
							where d.EventId == new Guid(eventId)
							orderby d.BeginDate descending
							select new {d}).SingleOrDefault();
			
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
			hidEventId.Value = eventId;
			
			var eventDetails = (from ev in dc.Events
						   where ev.EventId == new Guid(eventId)
						   select ev).SingleOrDefault();
		}
	}
	public string GetErrorMessage(MembershipCreateStatus status)
	{
		switch (status)
		{
			case MembershipCreateStatus.DuplicateUserName:
				return "Username already exists. Please enter a different user name.";

			case MembershipCreateStatus.DuplicateEmail:
				return "That e-mail address already exists, do you need to sign in?<br><a class=\"alert-link\" href=\"Login.aspx\">Sign In</a>";

			case MembershipCreateStatus.InvalidPassword:
				return "The password provided is invalid. Please enter a password with 8 characters that includes a character and a number.";

			case MembershipCreateStatus.InvalidEmail:
				return "The e-mail address provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.InvalidAnswer:
				return "The password retrieval answer provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.InvalidQuestion:
				return "The password retrieval question provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.InvalidUserName:
				return "The user name provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.ProviderError:
				return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

			case MembershipCreateStatus.UserRejected:
				return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

			default:
				return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
		}
	}
}