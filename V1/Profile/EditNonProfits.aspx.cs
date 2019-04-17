using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_EditNonProfits : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var events = from c in dc.Organizations
							orderby c.Name
							select new {name = " - " + c.Name, c.OrganizationId };
			
			chkBoxOrganizations.DataSource = events;
			chkBoxOrganizations.DataBind();

			var userOrganizations = from uo in dc.UserOrganizations
								   where uo.UserId == userId
								   select uo;
			
			//Preselect the orgs for this user
			if(userOrganizations.Count() > 0 )
			{
				foreach(var userOrganization in userOrganizations)
				{
					for (int i = 0; i < chkBoxOrganizations.Items.Count; i++)
					{
						if(userOrganization.OrganizationId.ToString() == chkBoxOrganizations.Items[i].Value)
						{
							chkBoxOrganizations.Items[i].Selected = true;
						}
					}
				}
			}
			
			for (int i = 0; i < chkBoxOrganizations.Items.Count; i++)
			{
				//Add a disaster next to the org name if it's associated with one.
				var organizationEvents = from oe in dc.OrganizationEvents
											join ev in dc.Events on oe.EventId equals ev.EventId
											where oe.OrganizationId.ToString() == chkBoxOrganizations.Items[i].Value
											select new { oe, ev };

				if(organizationEvents.Count() > 0)
				{
					string eventsList = string.Empty;
					foreach(var organizationEvent in organizationEvents)
					{
						eventsList += organizationEvent.ev.Name + ", ";
					}
					chkBoxOrganizations.Items[i].Text = chkBoxOrganizations.Items[i].Text + " (" + eventsList.Substring(0, eventsList.Length-2) + ")";
				}
			}
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("Profile.aspx");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		divMessage.Visible = true;
		lblMessage.Text = "Your affilicated non-profits have been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		foreach (ListItem item in chkBoxOrganizations.Items)
		{
			if (item.Selected)
			{
				//If item is not already selected, then check it.
				var userCheck = from p in dc.UserOrganizations
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.OrganizationId == new Guid(item.Value)
								select p;

				if (userCheck.Count() == 0)
				{
					//insert the checked item.
					UserOrganization userOrganization = new UserOrganization();
					userOrganization.OrganizationId = new Guid(item.Value);
					userOrganization.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					userOrganization.UserOrganizationId = Guid.NewGuid();
					dc.UserOrganizations.InsertOnSubmit(userOrganization);
					dc.SubmitChanges();
				}
			}
			else
			{
				//If item is selected then unselect it.
				var userChecks = from p in dc.UserOrganizations
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.OrganizationId == new Guid(item.Value)
								select p;
				
				//Delete any checked records
				if (userChecks.Count() > 0)
				{
					//Item is selected.
					foreach(var userCheck in userChecks)
					{
						dc.UserOrganizations.DeleteOnSubmit(userCheck);
						dc.SubmitChanges();
					}
				}
			}
		}
	}
}