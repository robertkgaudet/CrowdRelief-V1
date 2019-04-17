using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_NonProfitCampaign : BaseOrganizationWebForm
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string donateLink = string.Empty;
	public string volunteerLink = string.Empty;
	public string icon = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        if (String.IsNullOrEmpty(Request.QueryString["organizationEventFriendlyURLName"]))
		{
			Response.Write("No organization Id provided.");
			Response.End();
        }

        string organizationEventFriendlyURLName = Request.QueryString["organizationEventFriendlyURLName"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organizationEvent = (from oe in dc.OrganizationEvents
								join ev in dc.Events on oe.EventId equals ev.EventId
								join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
								where oe.URLFriendlyCampaignName == organizationEventFriendlyURLName
								select new { oe, ev, o }).SingleOrDefault();

		if (organizationEvent.ev.Icon != null)
		{
			icon = organizationEvent.ev.Icon.Replace("COLOR", "btn-" + organizationEvent.ev.Color + " btn-outline");
		}

		litCampaignName.Text = organizationEvent.oe.CampaignName;
		litEventName.Text = organizationEvent.ev.Name;

		var peopleList =	from uo in dc.UserOrganizations
							join p in dc.Profiles on uo.UserId equals p.UserId
							where uo.OrganizationId == organizationEvent.oe.OrganizationId
							orderby p.Title descending
							select p;

		rpNonProfitPeople.DataSource = peopleList;
		rpNonProfitPeople.DataBind();
		
		Master.PageTitle			= organizationEvent.o.Name + " - CrowdRelief";
		Master.PageDescription		= organizationEvent.o.Description;
		Master.FbDescription		= organizationEvent.o.Description;
		Master.FbImage				= organizationEvent.o.CoverImage;
		Master.FbImageType			= "image/jpg";
		Master.FbSite_name			= organizationEvent.o.Name + " - CrowdRelief";
		Master.FbURL				= Request.Url.AbsoluteUri;

		litCampaignMission.Text				= organizationEvent.oe.MissionPurpose;
		hypOrganizationName.Text			= organizationEvent.o.Name;
		hypOrganizationName.NavigateUrl		= organizationEvent.o.Website;
		hypOrganizationName.Font.Underline	= true;
		lblParentOrgName.Text				= organizationEvent.o.Name;
		hypParentAddress.Text				= organizationEvent.o.Address + "<br/>" + organizationEvent.o.City + ", " + organizationEvent.o.State + " " + organizationEvent.o.Zip;
		hypParentAddress.NavigateUrl		= "http://maps.google.com/maps?q=" + organizationEvent.o.Address.Replace(" ","+") + "," + organizationEvent.o.City.Replace(" ","+") + "," + organizationEvent.o.State.Replace(" ","+") + "," + organizationEvent.o.Zip;
		lblVoadMember.Text					= organizationEvent.o.IsVoadMember.ToString();
		lbl501c3.Text						= organizationEvent.o._501c3Status.ToString();
			
			
		lblPointOfContactPerson.Text = organizationEvent.o.PointOfContactName;
		if(!String.IsNullOrEmpty(organizationEvent.o.PointOfContactPhoneNumber))
		{
			hypPointOfContactPhone.Text				= Regex.Replace(organizationEvent.o.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypPointOfContactPhone.NavigateUrl			= "tel:" + organizationEvent.o.PointOfContactPhoneNumber;
			hypPointOfContactPhone.Font.Underline		= true;
		}
			
		if(!String.IsNullOrEmpty(organizationEvent.o.PointOfContactEmail))
		{
			hypPointOfContactEmail.Text				= organizationEvent.o.PointOfContactEmail;
			hypPointOfContactEmail.NavigateUrl		= "mailto:" + organizationEvent.o.PointOfContactEmail;
			hypPointOfContactEmail.Font.Underline	= true;
		}
			
			
		if(!String.IsNullOrEmpty(organizationEvent.o.FacebookURL))
		{
			hypFacebookPage.Text			= organizationEvent.o.Name + " Facebook Page";
			hypFacebookPage.NavigateUrl		= organizationEvent.o.FacebookURL;
			hypFacebookPage.Font.Underline	= true;
		}

		if(!String.IsNullOrEmpty(organizationEvent.o.FacebookGroupURL))
		{
			hypFacebookGroup.Text			= organizationEvent.o.Name + " Facebook Group";
			hypFacebookGroup.NavigateUrl	= organizationEvent.o.FacebookGroupURL;
			hypFacebookGroup.Font.Underline	= true;
		}
			
		if(!String.IsNullOrEmpty(organizationEvent.o.TwitterURL))
		{
			hypTwitter.Text					= "Visit " + organizationEvent.o.TwitterURL;
			hypTwitter.NavigateUrl			= "https://www.Twitter.com/" + organizationEvent.o.TwitterURL;
			hypTwitter.Font.Underline		= true;
		}
			
		if(!String.IsNullOrEmpty(organizationEvent.o.YouTubeURL))
		{
			hypYouTube.Text					= organizationEvent.o.Name + " YouTube Channel";
			hypYouTube.NavigateUrl			= organizationEvent.o.YouTubeURL;
			hypYouTube.Font.Underline		= true;
		}
			
		if(!String.IsNullOrEmpty(organizationEvent.o.PrimaryPhone))
		{
			dtPrimaryPhone.Visible				= true;
			ddPrimaryPhone.Visible				= true;
			hypPrimaryPhone.Text				= Regex.Replace(organizationEvent.o.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypPrimaryPhone.NavigateUrl			= "tel:" + organizationEvent.o.PrimaryPhone;
			hypPrimaryPhone.Font.Underline		= true;
		}


		if(!String.IsNullOrEmpty(organizationEvent.o.PublicPhoneNumber))
		{
			ddParentPhone.Visible				= true;
			hypParentPhone.Text			= Regex.Replace(organizationEvent.o.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypParentPhone.NavigateUrl	= "tel:" + organizationEvent.o.PublicPhoneNumber;
			hypParentPhone.Font.Underline = true;
		}

		if(!String.IsNullOrEmpty(organizationEvent.o.PublicEmail))
		{ 
			
			ddParentEmail.Visible			= true;
			hypParentEmail.Text				= organizationEvent.o.PublicEmail;
			hypParentEmail.NavigateUrl		= "mailto:" + organizationEvent.o.PublicEmail;
			hypParentEmail.Font.Underline	= true;
		}

		if(!String.IsNullOrEmpty(organizationEvent.o.Website))
		{
			ddWebsite.Visible			= true;
			hypWebsite.Text				= organizationEvent.o.Website;
			hypWebsite.NavigateUrl		= organizationEvent.o.Website;
			hypWebsite.Font.Underline	= true;
		}
			
		if(!String.IsNullOrEmpty(organizationEvent.o.DonationURL))
		{
			lbDonate.Visible = true;
			donateLink = organizationEvent.o.DonationURL;
		}
	}
    
    protected void rpNonProfitPeople_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
        //List disasters
        //Show their photo
        //Show their skills
        
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname	= (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname		= (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName		= (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title	= (String)DataBinder.Eval(dataItem.DataItem, "Title");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var profilePhoto = (from p in dc.ProfilePhotos
							   join ph in dc.Photos on p.PhotoId equals ph.PhotoId
							   where p.UserId == userId && p.IsCurrrent == true
							   select new {ph.FilenameCropped }).Take(1).SingleOrDefault();
            
            Literal lblInfo = (Literal)e.Item.FindControl("lblInfo");
			Image imgProfilePhoto = (Image)e.Item.FindControl("imgProfilePhoto");

            imgProfilePhoto.ImageUrl = "/V1/Images/icons8-customer-64.png";
			if(profilePhoto != null)
			{
				imgProfilePhoto.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;
			}

			zelloName = String.IsNullOrEmpty(zelloName) ? "none" : zelloName;
			title = String.IsNullOrEmpty(title) ? "none" : title;
				
			lblInfo.Text = "<p><dl class=\"dl-horizontal\" class=\"m-l-sm\"><dd><b><a href=\"/V1/Profile/Profile.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " "+  lastname + "</a></b><br>Zello: " + zelloName + "<br>Title: " + title + "</dd></dl></p>";
		}
	}
}