using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_NonProfit : System.Web.UI.Page
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string donateLink = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        if (String.IsNullOrEmpty(Request.QueryString["organizationId"]))
		{
			Response.Write("No organization Id provided.");
			Response.End();
        }

        string organizationId = Request.QueryString["organizationId"];


		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var peopleList =	from uo in dc.UserOrganizations
							join p in dc.Profiles on uo.UserId equals p.UserId
							where uo.OrganizationId == new Guid(organizationId)
							orderby p.Title descending
							select new { p.Firstname, p.Lastname, p.UserId, p.Title, p.ZelloName };

		rpNonProfitPeople.DataSource = peopleList;
		rpNonProfitPeople.DataBind();


		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(organizationId)
						   select o).SingleOrDefault();

		if(organization != null)
        {
            if (User.IsInRole("Administrator"))
            {
                divUnpublishedInformation.Visible = true;
                divUploadLogoCover.Visible = true;
                dtEIN.Visible = true;
                ddEIN.Visible = true;

                if (!String.IsNullOrEmpty(Request.QueryString["ownerId"]))
                {
                    //Set the new ownerId
                    organization.OwnerId = new Guid(Request.QueryString["ownerId"]);
                    dc.SubmitChanges();
                    litAlertMessage.Text = "A new non-profit page owner has been set.";
                    divAlertMessage.Visible = true;
                }
            }

            Master.PageTitle			= organization.Name + " - CrowdRelief";
			Master.PageDescription		= organization.Description;
			Master.FbDescription		= organization.Description;
			Master.FbImage				= organization.CoverImage;
			Master.FbImageType			= "image/jpg";
			Master.FbSite_name			= organization.Name + " - CrowdRelief";
			Master.FbURL				= Request.Url.AbsoluteUri;

			litOrganizationName.Text	= organization.Name;
			lblOrgName.Text				= organization.Name;
			litMission.Text				= organization.PurposeMission;
			litDescription.Text			= organization.Description;
			litYearFounded.Text			= organization.YearFounded;
			hypAddress.Text				= organization.Address + "<br/>" + organization.City + ", " + organization.State + " " + organization.Zip;
			hypAddress.NavigateUrl		= "http://maps.google.com/maps?q=" + organization.Address.Replace(" ","+") + "," + organization.City.Replace(" ","+") + "," + organization.State.Replace(" ","+") + "," + organization.Zip;
			lblVoadMember.Text			= organization.IsVoadMember.ToString();
			lbl501c3.Text				= organization._501c3Status.ToString();
			
			
			lblPointOfContactPerson.Text = organization.PointOfContactName;
			if(!String.IsNullOrEmpty(organization.PointOfContactPhoneNumber))
			{
				hypPointOfContactPhone.Text				= Regex.Replace(organization.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPointOfContactPhone.NavigateUrl			= "tel:" + organization.PointOfContactPhoneNumber;
				hypPointOfContactPhone.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(organization.PointOfContactEmail))
			{
				hypPointOfContactEmail.Text				= organization.PointOfContactEmail;
				hypPointOfContactEmail.NavigateUrl		= "mailto:" + organization.PointOfContactEmail;
				hypPointOfContactEmail.Font.Underline	= true;
			}
			
			
			if(!String.IsNullOrEmpty(organization.FacebookURL))
			{
				hypFacebookPage.Text			= organization.Name + " Facebook Page";
				hypFacebookPage.NavigateUrl		= organization.FacebookURL;
				hypFacebookPage.Font.Underline	= true;
			}

			if(!String.IsNullOrEmpty(organization.FacebookGroupURL))
			{
				hypFacebookGroup.Text			= organization.Name + " Facebook Group";
				hypFacebookGroup.NavigateUrl	= organization.FacebookGroupURL;
				hypFacebookGroup.Font.Underline	= true;
			}
			
			if(!String.IsNullOrEmpty(organization.TwitterURL))
			{
				hypTwitter.Text					= "Visit " + organization.TwitterURL;
				hypTwitter.NavigateUrl			= "https://www.Twitter.com/" + organization.TwitterURL;
				hypTwitter.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(organization.InstagramURL))
			{
				hypInstagram.Text				= "Instagram";
				hypInstagram.NavigateUrl		= "https://www.Instagram.com/" + organization.InstagramURL;
				hypInstagram.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(organization.YouTubeURL))
			{
				hypYouTube.Text					= organization.Name + " YouTube Channel";
				hypYouTube.NavigateUrl			= organization.YouTubeURL;
				hypYouTube.Font.Underline		= true;
			}
			

			
			






			if(!String.IsNullOrEmpty(organization.EIN))
			{
				lblEIN.Text		= organization.EIN;
				dtEIN.Visible	= true;
				ddEIN.Visible	= true;
			}
			
			if(!String.IsNullOrEmpty(organization.PrimaryPhone))
			{
				dtPrimaryPhone.Visible				= true;
				ddPrimaryPhone.Visible				= true;
				hypPrimaryPhone.Text				= Regex.Replace(organization.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPrimaryPhone.NavigateUrl			= "tel:" + organization.PrimaryPhone;
				hypPrimaryPhone.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(organization.SecondaryPhone))
			{
				dtSecondaryPhone.Visible				= true;
				ddSecondaryPhone.Visible			= true;
				hypSecondaryPhone.Text				= Regex.Replace(organization.SecondaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypSecondaryPhone.NavigateUrl		= "tel:" + organization.SecondaryPhone;
				hypSecondaryPhone.Font.Underline	= true;
			}


			if(!String.IsNullOrEmpty(organization.PublicPhoneNumber))
			{
				ddPublicPhone.Visible				= true;
				hyoPublicPhoneNumber.Text			= Regex.Replace(organization.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hyoPublicPhoneNumber.NavigateUrl	= "tel:" + organization.PublicPhoneNumber;
				hyoPublicPhoneNumber.Font.Underline = true;
			}
			if(!String.IsNullOrEmpty(organization.PublicEmail))
			{
				ddPublicEmail.Visible					= true;
				hypPublicEmailAddress.Text				= organization.PublicEmail;
				hypPublicEmailAddress.NavigateUrl		= "mailto:" + organization.PublicEmail;
				hypPublicEmailAddress.Font.Underline	= true;
			}
			if(!String.IsNullOrEmpty(organization.Website))
			{
				ddWebsite.Visible			= true;
				hypWebsite.Text				= organization.Website;
				hypWebsite.NavigateUrl		= organization.Website;
				hypWebsite.Font.Underline	= true;
			}
			
			if(!String.IsNullOrEmpty(organization.BlogURL))
			{
				ddBlog.Visible			= true;
				hypBlog.Text			= organization.BlogURL;
				hypBlog.NavigateUrl		= organization.BlogURL;
				hypBlog.Font.Underline	= true;
			}
			
			if(!String.IsNullOrEmpty(organization.DonationURL))
			{
				lbDonate.Visible = true;
				donateLink = organization.DonationURL;
			}
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
            HyperLink hypMakeOwner = (HyperLink)e.Item.FindControl("hypMakeOwner");

            hypMakeOwner.NavigateUrl = "NonProfit.aspx?organizationId=" + Request.QueryString["organizationId"] +"&ownerId=" + userId.ToString();
            hypMakeOwner.Text = "Set '" + firstname + "' As Owner";
            if (User.IsInRole("Administrator"))
            {
                hypMakeOwner.Visible = true;
            }

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