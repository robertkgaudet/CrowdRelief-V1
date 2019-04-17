using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_DisasterList : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string title = System.Configuration.ConfigurationManager.AppSettings["Title"].ToString();
		string description = System.Configuration.ConfigurationManager.AppSettings["Description"].ToString();

		this.Master.PageTitle			= title;
		this.Master.FbSite_name			= title;
		this.Master.PageDescription		= description;
		this.Master.FbDescription		= description;
		this.Master.FbImage				= "Images/MainDisasterImage.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbURL				= Request.Url.AbsoluteUri;

		string userType = Request.QueryString["userType"];

		if(String.IsNullOrEmpty(userType))
		{
			userType = "Survivor";
		}

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var disasters = from d in dc.Events
						where d.IsDisaster == true
						orderby d.BeginDate descending
						select new { URLFriendlyName = "/" + d.URLFriendlyName + "/" + userType, d.Name };

		lvDisasters.DataSource = disasters;
		lvDisasters.DataBind();
	}
}