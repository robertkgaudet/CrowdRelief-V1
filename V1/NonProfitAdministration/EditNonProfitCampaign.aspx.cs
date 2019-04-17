using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_EditNonProfitCampaign : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(Request.QueryString["OrganizationEventId"] == null)
		{
			litEventName.Text = "No OrganizationEventId provided.";
		}
		else
		{
			Guid organizationEventId = new Guid(Request.QueryString["OrganizationEventId"]);
			loadForm(organizationEventId);
		}
	}

	protected void loadForm(Guid organizationEventId)
	{
		//Load the states list
		ListItemCollection statesList = new ListItemCollection();
		foreach (string stateItem in States.Names())
		{
			ListItem li = new ListItem(stateItem, stateItem);
			statesList.Add(li);
		}
		ddlState.DataSource = statesList;
		ddlState.DataBind();
		
		//If org and eventid already exist, then don't allow them here.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organizationEvent = (from oe in dc.OrganizationEvents
								 join ev in dc.Events on oe.EventId equals ev.EventId
								 where oe.OrganizationEventId == organizationEventId
								 select new { ev, oe }).SingleOrDefault();
		
		txtPurposeMission.Value = organizationEvent.oe.MissionPurpose;
		txtCampaignName.Value = organizationEvent.oe.CampaignName;
		txtURLFriendlyCampaignName.Value = organizationEvent.oe.URLFriendlyCampaignName;
		chkVoad.Checked = (bool)organizationEvent.oe.IsVoadMember;
		chkAcceptsVolunteers.Checked = (bool)organizationEvent.oe.AcceptsVolunteers;
		txtVolunteerInstructions.Value = organizationEvent.oe.VolunteerInstructions;
		txtAddress.Value = organizationEvent.oe.StagingAddress;
		txtCity.Value = organizationEvent.oe.StagingCity;
		//ddlState.SelectedValue = organizationEvent.oe.StagingState;
		txtZipCode.Value = organizationEvent.oe.StagingZipCode;
		txtPOCFullname.Value = organizationEvent.oe.PointOfContactName;
		txtPhonenumber.Value = organizationEvent.oe.PhoneNumber;
		txtZelloChannel.Value = organizationEvent.oe.ZelloChannel;
		txtEmailAddress.Value = organizationEvent.oe.Email;
		txtDonationLink.Value = organizationEvent.oe.DonationURL;
		txtWebsite.Value = organizationEvent.oe.Website;
		txtblogURL.Value = organizationEvent.oe.BlogURL;
		txtFacebook.Value = organizationEvent.oe.FacebookPage;
		txtFacebookGroup.Value = organizationEvent.oe.FacebookGroup;



		var disaster = (from ev in dc.Events
						where ev.EventId == organizationEvent.oe.EventId
						select new { ev.Name }).SingleOrDefault();
		litEventName.Text = disaster.Name;
		//Hide form and show message and link to the campaign.
		divMessage.Visible = true;
		divForm.Visible = true;
		hypLinkToCampaign.NavigateUrl = "/NonProfitResponse/" + organizationEvent.oe.URLFriendlyCampaignName;
		hypLinkToCampaign.Text = " Visit your campaign page " + organizationEvent.oe.CampaignName;
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
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
		
		Guid organizationEventId = new Guid(Request.QueryString["OrganizationEventId"]);

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organizationEvent = (from oe in dc.OrganizationEvents
								 where oe.OrganizationEventId == organizationEventId
								 select oe).SingleOrDefault();
								 
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
		organizationEvent.IsVoadMember = isVoad;
		dc.SubmitChanges();

		Response.Redirect("/NonProfitResponse/" + URLFriendlyCampaignName);
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/NonProfitList.aspx");
	}
}