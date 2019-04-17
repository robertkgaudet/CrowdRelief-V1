using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Profile_ChooseEvent : BaseOrganizationWebForm
{
	public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{
		///Get a list of the disasters and display them.
		this.Master.PageTitle = "CrowdRelief - Recent Natural Disasters";
		this.Master.PageDescription = "Select the disaster you want to engage with.";
		this.Master.FbDescription = "Select the disaster you want to engage with.";
		this.Master.FbImage = "V1/Images/HMichael.png";
		this.Master.FbImageType = "image/jpg";
		this.Master.FbSite_name = "CrowdRelief - Recent Natural Disasters";
		this.Master.FbURL = Request.Url.AbsoluteUri;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var pastDisasters = from d in dc.Events
							where d.IsDisaster == true
						orderby d.BeginDate descending
						select d;

		litEvents.Text = CreateDisasterPanels(pastDisasters);
		
		litEventDescription.Text = "Choose a Disaster";
		string action = Request.QueryString["action"];
		if(!string.IsNullOrEmpty(action))
		{
			if(action == "volunteer")
			{
				litEventDescription.Text = "Choose a Disaster To Volunteer";
			}
			else if(action =="rebuild")
			{
				litEventDescription.Text = "Choose a Disaster to Track Your Home Rebuilding";
			}
		}
	}
	
	protected string CreateDisasterPanels(IOrderedQueryable<Event> disasters)
	{
		string disasterPanel = string.Empty;

		foreach(Event disaster in disasters)
		{	
			//string icon = string.Empty;
			string headerColor = string.Empty;
			string eventDate = String.Format("{0:Y}", disaster.BeginDate);
			string color = disaster.Color;
			
			string rebuildTickLabel = string.Empty;
			//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);
			string rebuildProgress = GetPercent(true, disaster.EventId, Guid.Empty, new Guid(rebuildProgressSliderId), out rebuildTickLabel).ToString() + "%";

			//if(disaster.Icon != null)
			//{
			//	icon = disaster.Icon.Replace("COLOR", "btn-" + color + " btn-outline");
			//}

			if(disaster.Color != null)
			{
				headerColor = "h" + CrowdRelief.Tools.GetColor(disaster.Color);
			}

			string activeCSS = string.Empty;
			string activeHeader = string.Empty;
			
			string tickLabel = string.Empty;
			string overallPercent = string.Empty;

			if(disaster.IsActive)
			{
				activeCSS = "activeDisasterPanel";
				
				activeHeader =	 "<div class=\"alert alert-success\">" + Environment.NewLine +
									"<h6 style=\"margin:0px;\"><i class=\"fa fa-bolt\"></i> ACTIVE</h6>" + Environment.NewLine +
								"</div>" + Environment.NewLine;
			}
			else
			{
				activeCSS = "disasterPanel";
				//string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
				//overallPercent = GetPercent(true, disaster.EventId, Guid.Empty, new Guid(overallProgressSliderId), out tickLabel).ToString();
				activeHeader = "<div class=\"alert alert-warning\">" + Environment.NewLine +
									"<h6 style=\"margin:0px;\"><i class=\"fa fa-tachometer\"></i> RECOVERY " + rebuildProgress + "</h6>" + Environment.NewLine +
								"</div>" + Environment.NewLine;
			}
			
			//CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			//int homeCount = (from c in dc.Rebuilds
			//				where c.EventId == disaster.EventId
			//				select c).Count();

			//disasterPanel += "<div class=\"grid-item\" onclick=\"window.location.href='/V1/Event.aspx?eventId=" + disaster.EventId + "';\">" +  Environment.NewLine + Environment.NewLine +

			disasterPanel += "<div class=\"grid-item\" onclick=\"window.location.href='/" + disaster.URLFriendlyName + "';\">" +  Environment.NewLine + Environment.NewLine +
								"<div class=\"hpanel " + headerColor + "\">" + Environment.NewLine +
									"<div class=\"panel-body " + activeCSS + "\">" + Environment.NewLine +
										"<div class=\"text-center\">" + Environment.NewLine +
											"<p class=\"font-bold text-" + color + "\">" + eventDate + "</p>" + Environment.NewLine +
											"<div class=\"titleHeight\"><h3 class=\"m-b-xs font-bold\">" + disaster.Name + "</h3></div>" + Environment.NewLine +
											//"<div class=\"m\"><h4>" + Environment.NewLine +
											//	icon + Environment.NewLine +
											//"</h4></div>" + Environment.NewLine +
											//"<br><div class=\"badge m-t-md badge-info\"><h4>" + homeCount + " Homes</h4></div>" + Environment.NewLine +
											//"<br><div class=\"badge m-t-md badge-info\">" + volunteersNeeded + " Volunteers Needed</div>" + Environment.NewLine + 
											"<p class=\"small p-sm\">" + Environment.NewLine +
												disaster.Description + Environment.NewLine +
											"</p>" + Environment.NewLine +
										"</div>" + Environment.NewLine +
									"</div>" + activeHeader + Environment.NewLine +
								"</div>" + Environment.NewLine +
							"</div>" + Environment.NewLine + Environment.NewLine;
			}

		return disasterPanel;
	}
}