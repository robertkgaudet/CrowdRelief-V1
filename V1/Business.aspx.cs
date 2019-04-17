using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Business : BaseOrganizationWebForm
{
	public string eventName = HttpContext.Current.Request.QueryString["eventName"];

	protected void Page_Load(object sender, EventArgs e)
	{
		string businessPageTitle = System.Configuration.ConfigurationManager.AppSettings["BusinessPageTitle"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disaster = (from ev in dc.Events
						where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
						select ev).SingleOrDefault();

		if(disaster != null)
		{
			string pageDescription = disaster.Name+ " businesses will go through the CrowdRelief vetting process to become visible to suvivors looking for your services.";
			string pageTitle = disaster.Name + businessPageTitle;

			this.Master.PageTitle = "CrowdRelief - " + pageTitle;
			this.Master.PageDescription = "CrowdRelief - " + pageDescription;
			this.Master.FbDescription = "CrowdRelief - " + pageDescription;
			this.Master.FbImage = "/V1/Images/" + disaster.ImageFileName;
			this.Master.FbImageType = "image/jpg";
			this.Master.FbSite_name = "CrowdRelief - " + pageTitle;
			this.Master.FbURL = Request.Url.AbsoluteUri;

			uc1EventHeader.PageTitle		= businessPageTitle;
			uc1EventHeader.EventName		= disaster.Name;
			uc1EventHeader.PageDescription	= pageDescription;
		}
	}
}