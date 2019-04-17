using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_EventHeader : System.Web.UI.UserControl
{
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];
	public string headerColor = string.Empty;
	public string color = string.Empty;
	public string icon = string.Empty;
	public string _pageTitle = string.Empty; 
	public string _eventName = string.Empty; 
	public string _pageDescription = string.Empty;
	public string eventBackgroundImage = "MichaelImage.jpg";
	public Guid? eventId = Guid.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		//Get the eventId from the eventname
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disaster = (from ev in dc.Events
						where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
						select ev).SingleOrDefault();
		
		if(disaster != null)
		{
			eventId = disaster.EventId;
			
			if(HttpContext.Current.User.Identity.IsAuthenticated)
			{
				var profile = (from p in dc.Profiles
							  where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
							  select p).SingleOrDefault();
		
				if(profile.DefaultEventId == eventId)
				{
					//Show message that this is the users default event id.
					lblDefaultEventMessage.Text = disaster.Name + " is your default disaster.";
					btnSetDefaultDisaster.Visible = false;
				}
				else
				{
					lblDefaultEventMessage.Text = "Set this as your default disaster to load this page when you log in.";
				}
			}

			if(disaster.Icon != null)
			{
				icon = disaster.Icon.Replace("COLOR", "btn-" + disaster.Color + " btn-outline");
			}
			litDate.Text = String.Format("{0:Y}", disaster.BeginDate);
			
			if(disaster.Color != null)
			{
				color = disaster.Color;
				headerColor = CrowdRelief.Tools.GetColor(disaster.Color);
			}

			litEventName.Text = _eventName;
			litPageName.Text = _pageTitle;
			litEventDescription.Text = _pageDescription;

			string breadCrumb = "<ol class=\"hbreadcrumb breadcrumb\">" +
									"<li><a class=\"btn btn-xs btn-info m-t-sm\" href=\"/ChooseDisaster\">Disasters</a></li>" +
									"<li><a class=\"btn btn-xs btn-info m-t-sm\" href=\"/" +   disaster.URLFriendlyName + "\"> " + disaster.Name +  " </a></li>" +	
									"<li>" +
										"<span><a class=\"btn btn-xs btn-info m-t-sm\" href=\"/" +   disaster.URLFriendlyName + "/Survivor\"> Survivors </a></span>" +
									"</li>" +
									"<li>" +
										"<span><a class=\"btn btn-xs btn-info m-t-sm\" href=\"/" +   disaster.URLFriendlyName + "/Helper\"> Helpers </a></span>" +
									"</li>" +
									"<li>" +
										"<span><a class=\"btn btn-xs btn-info m-t-sm\" href=\"/" +   disaster.URLFriendlyName + "/Nonprofit\"> Non-profits </a></span>" +
									"</li>" +
									"<li>" +
										"<span><a class=\"btn btn-xs btn-info m-t-sm\" href=\"/" +   disaster.URLFriendlyName + "/Business\"> Business </a></span>" +
									"</li>" +
								"</ol>";
			litBreadcrumb.Text = breadCrumb;

			if(!String.IsNullOrEmpty(disaster.ImageFileName))
			{
				eventBackgroundImage = disaster.ImageFileName;
			}
		}

		if(!HttpContext.Current.User.Identity.IsAuthenticated)
		{
			divRegister.Visible = true;
			divChooseDefaultDisaster.Visible = false;
		}
		else
		{
			divRegister.Visible = false;
			divChooseDefaultDisaster.Visible = true;
		}

	}
	

	public string PageDescription
	{
		get { return _pageDescription; }
		set { _pageDescription = value; }
	}
	public string PageTitle
	{
		get { return _pageTitle; }
		set { _pageTitle = value; }
	}
	public string EventName
	{
		get { return _eventName; }
		set { _eventName = value; }
	}

	protected void btnSetDefaultDisaster_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());


        var profile = (from p in dc.Profiles
					  where p.UserId == userId
                       select p).SingleOrDefault();

		profile.DefaultEventId = eventId;
		dc.SubmitChanges();
		
		var disaster = (from ev in dc.Events
						where ev.EventId == eventId
                        select ev).SingleOrDefault();

        //Makes sure the user is assocated with the response if not already.
        var userCheck = from p in dc.UserEvents
                        where p.UserId == userId
                        && p.EventId == disaster.EventId
                        select p;

        if (userCheck.Count() == 0)
        {
            UserEvent userEvent = new UserEvent();
            userEvent.EventId = disaster.EventId;
            userEvent.UserId = userId;
            userEvent.UserEventId = Guid.NewGuid();
            dc.UserEvents.InsertOnSubmit(userEvent);
            dc.SubmitChanges();
        }

        Response.Redirect("/" + disaster.URLFriendlyName);
	}
}