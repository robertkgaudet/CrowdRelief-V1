using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_RespondToEvent : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Guid eventId = new Guid(Request.QueryString["eventId"]);
		Guid organizationId = new Guid(Request.QueryString["organizationId"]);
		//If org and eventid already exist, then don't allow them here.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organizationEvent = (from oe in dc.OrganizationEvents
									join ev in dc.Events on oe.EventId equals ev.EventId
								where oe.EventId == eventId && oe.OrganizationId == organizationId
								select new { ev, oe }).SingleOrDefault();

		if(organizationEvent != null)
		{
			//Hide form and show message and link to the campaign.
			divMessage.Visible = true;
			lblMessage.Text = " A campaign for this event already exists.";
			divForm.Visible = false;
			hypLinkToCampaign.NavigateUrl = "/NonProfitResponse/" + organizationEvent.oe.URLFriendlyCampaignName;
			hypLinkToCampaign.Text = "Visit your campaign page " + organizationEvent.oe.CampaignName;
		}

		if (!IsPostBack)
		{
			ListItemCollection statesList = new ListItemCollection();
			foreach (string state in States.Names())
			{
				ListItem li = new ListItem(state, state);
				statesList.Add(li);
			}

			ddlState.DataSource = statesList;
			ddlState.DataBind();




			var disaster = (from ev in dc.Events
							where ev.EventId == eventId
							select new { ev.Name }).SingleOrDefault();

			litEventName.Text = disaster.Name;
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		Guid eventId = new Guid(Request.QueryString["eventId"]);	
		Guid organizationId = new Guid(Request.QueryString["organizationId"]);

		string pointOfContactName = txtPOCFullname.Value;
		string purposeMission = txtPurposeMission.Value;
		string campaignName = txtCampaignName.Value;
		string URLFriendlyCampaignName = txtURLFriendlyCampaignName.Value.Replace(" ", "").Replace("'", "").Replace("\"", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(",", "").Replace("!", "").Replace("-", "").Replace(":", "").Replace("+", "").Replace("&", "").Replace("*", "");
		bool? isVoad = chkVoad.Checked;
		bool acceptsVolunteers = chkAcceptsVolunteers.Checked;
		string volunteerInstructions = txtVolunteerInstructions.Value;
		string address = txtAddress.Value;
		string city = txtCity.Value;
		string state = ddlState.SelectedValue;
		string zip = txtZipCode.Value;
		string POCName = txtPOCFullname.Value;
		string phoneNumber = txtPhonenumber.Value;
		string zelloChannel = txtZelloChannel.Value;
		string emailAddress = txtEmailAddress.Value;
		string donationLink = txtDonationLink.Value;
		string website = txtWebsite.Value;
		string blogURL = txtblogURL.Value;
		string facebook = txtFacebook.Value;
		string facebookGroup = txtFacebookGroup.Value;



		OrganizationEvent organizationEvent = new OrganizationEvent();
		organizationEvent.OrganizationEventId = Guid.NewGuid();
		organizationEvent.EventId = eventId;
		organizationEvent.OrganizationId = organizationId;
		organizationEvent.PointOfContactName = pointOfContactName;
		organizationEvent.AcceptsVolunteers = acceptsVolunteers;
		organizationEvent.MissionPurpose = purposeMission;
		organizationEvent.CampaignName = campaignName;
		organizationEvent.URLFriendlyCampaignName = URLFriendlyCampaignName;
		organizationEvent.VolunteerInstructions = volunteerInstructions;
		organizationEvent.AcceptsVolunteers = acceptsVolunteers;
		organizationEvent.PointOfContactName = POCName;
		organizationEvent.PhoneNumber = phoneNumber;
		organizationEvent.ZelloChannel = zelloChannel;
		organizationEvent.Email = emailAddress;
		organizationEvent.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
		organizationEvent.Website = string.IsNullOrEmpty(website) ? null : website;
		organizationEvent.BlogURL = string.IsNullOrEmpty(blogURL) ? null : blogURL;
		organizationEvent.FacebookPage = string.IsNullOrEmpty(facebook) ? null : facebook;
		organizationEvent.FacebookGroup = string.IsNullOrEmpty(facebookGroup) ? null : facebookGroup;
		organizationEvent.StagingAddress = address;
		organizationEvent.StagingCity = city;
		organizationEvent.StagingState = state;
		organizationEvent.StagingZipCode = zip;
		organizationEvent.Createdby = userId;
		organizationEvent.CreatedOn = DateTime.Now;
		organizationEvent.IsVoadMember = isVoad;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		dc.OrganizationEvents.InsertOnSubmit(organizationEvent);
		dc.SubmitChanges();

		Response.Redirect("/NonProfitResponse/" + URLFriendlyCampaignName);

	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/NonProfitList.aspx");
	}
}