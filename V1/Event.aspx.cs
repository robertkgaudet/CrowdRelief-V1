using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

public partial class V1_Event : BaseOrganizationWebForm
{
    public string headerColor = string.Empty;
    public string color = string.Empty;
    public string icon = string.Empty;
    public string eventName = HttpContext.Current.Request.QueryString["eventName"];
    string _eventId = HttpContext.Current.Request.QueryString["eventId"];
    public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
    public string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
    public string registerNonProfit = "/V1/Profile/NonProfitNew.aspx";

    public string mapURL = string.Empty;
    public Guid eventId = Guid.Empty;
    public string btnSurvivorPage = string.Empty;
    public string btnBusinessPage = string.Empty;
    public string btnNonProfitPage = string.Empty;
    public string btnHelperPage = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
	{
		string disasterPageTitle = System.Configuration.ConfigurationManager.AppSettings["DisasterPageTitle"];
		if (!User.Identity.IsAuthenticated)
        {
            btnRebuildPost.Text = "Registration/Sign In is required to post.";
            btnRebuildPost.Enabled = false;
            txtPost.Attributes.Add("placeholder","Please Register/Sign In to post.");
            txtPost.Enabled = false;
        }

        if(!IsPostBack)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            if(!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventId"]))
            {
                eventId = new Guid(HttpContext.Current.Request.QueryString["eventId"]);
            }
            else if(!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventName"]))
            {
                //Get the eventId from the eventname
                var disaster = (from ev in dc.Events
                                where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
                                select new { ev.EventId }).SingleOrDefault();

                eventId = disaster.EventId;
            }

            LoadPosts();
            hidEventId.Value = eventId.ToString();

            if(!IsPostBack)
            {
				hypVolunteer.Font.Underline = true;
				hypAddASurvivor.Font.Underline = true;
				hypAddHome.Font.Underline = true;
				hypAddNonProfit.Font.Underline = true;
				hypUpdateProgress.Font.Underline = true;
				hypUpdateRebuildProgress.Font.Underline = true;

				//ucShelterLinks.eventId = eventId;
				ucDisasterSurvivorStoriesByDisaster.eventId = eventId;

                var disaster = (from ev in dc.Events
                                where ev.EventId == eventId
                                select ev).SingleOrDefault();


				lblQuickLinks.Text = disaster.Name + " Quick Links";

				hidEventName.Value = disaster.URLFriendlyName;
                btnSurvivorPage = "/" + disaster.URLFriendlyName + "/Survivor";
                btnBusinessPage = "/" + disaster.URLFriendlyName + "/Business";
                btnHelperPage = "/" + disaster.URLFriendlyName + "/Helper";
                btnNonProfitPage = "/" + disaster.URLFriendlyName + "/NonProfit";

				hypAddNonProfit.NavigateUrl = "/" + disaster.URLFriendlyName + "/NonProfit";
				hypAddASurvivor.NavigateUrl = "/S1/Profile/AddNewSurvivor.aspx?eventId=" + eventId;
				hypAddHome.NavigateUrl = "/V1/Profile/AddNewRebuild.aspx?eventId=" + eventId;
				hypVolunteer.NavigateUrl = "/" + disaster.URLFriendlyName + "/Helper";

				mapURL							= disaster.MAPUrl;
                
                string pageDescription = "Survivors, helpers, volunteers, non-profits and businesses start here to begin to restore and rebuild after " + disaster.Name + ".<br>" + disaster.Name + " " + disaster.Description;
                string pageTitle = disaster.Name + disasterPageTitle;

                
                this.Master.PageTitle			= "CrowdRelief - " +  pageTitle;
                this.Master.PageDescription		= "CrowdRelief - " + pageDescription;
                this.Master.FbDescription		= "CrowdRelief - " +  pageDescription;
                this.Master.FbImage				= "/V1/Images/" + disaster.ImageFileName;
                this.Master.FbImageType			= "image/jpg";
                this.Master.FbSite_name			= "CrowdRelief - " +  pageTitle;
                this.Master.FbURL				= Request.Url.AbsoluteUri;

				uc1EventHeader.PageTitle		= disasterPageTitle;
				uc1EventHeader.EventName		= disaster.Name;
				uc1EventHeader.PageDescription	= pageDescription;

                if(disaster.Icon != null)
                {
                    icon = disaster.Icon.Replace("COLOR", "btn-" + disaster.Color + " btn-outline");
                }

                if(disaster.Color != null)
                {
                    color = disaster.Color;
                    headerColor = CrowdRelief.Tools.GetColor(disaster.Color);
                }

                litEventNameForMap.Text = disaster.Name;
            
                divRequestAdmin.Visible = true;
                if(User.IsInRole("Administrator"))
                {
                    //divAdmin.Visible = true;
                    //LoadLinkCategories();
                    //divRequestAdmin.Visible = false;
                }

                hypNewCategory.NavigateUrl = "/V1/Profile/AddArticle.aspx?eventId=" + eventId;

                var homeCount = from c in dc.Rebuilds
                                where c.EventId == eventId
                                select c;

                litHomesAdded.Text = homeCount.Count().ToString();
                homeCount = homeCount.Where(c => c.CreatedBy == userId);

				var volunteerCount = from u in dc.aspnet_Users
									  join uir in dc.aspnet_UsersInRoles on u.UserId equals uir.UserId
									  join r in dc.aspnet_Roles on uir.RoleId equals r.RoleId
									  join uev in dc.UserEvents on u.UserId equals uev.UserId
									  where r.LoweredRoleName == "volunteer" || r.LoweredRoleName == "helper"
									  && uev.EventId == eventId
									  select u;

				litVolunteerCount.Text = volunteerCount.Count().ToString();


				var survivorCount = from u in dc.aspnet_Users
									 join uir in dc.aspnet_UsersInRoles on u.UserId equals uir.UserId
									 join r in dc.aspnet_Roles on uir.RoleId equals r.RoleId
									 join uev in dc.UserEvents on u.UserId equals uev.UserId
									 where r.LoweredRoleName == "survivor"
									 && uev.EventId == eventId
									 select u;

				litSurvivorCount.Text = survivorCount.Count().ToString();

				var nonProfitCount = from pe in dc.OrganizationEvents
									 join ev in dc.Events on pe.EventId equals ev.EventId
									 where ev.EventId == eventId
									 select pe;


				litNonprofitCount.Text = nonProfitCount.Count().ToString();

				litVolunteers.Text = CalculateVolunteersNeeded(Guid.Empty, 0, false, eventId);

                string rebuildTickLabel = string.Empty;
                string overallTickLabel = string.Empty;

                litRebuildProgress.Text = GetPercent(true, eventId, Guid.Empty, new Guid(rebuildProgressSliderId), out rebuildTickLabel).ToString() + "%";
                litOverallProgress.Text = GetPercent(true, eventId, Guid.Empty, new Guid(overallProgressSliderId), out overallTickLabel).ToString() + "%";
            
            }
        }
    }
    protected void btnRebuildPost_Click(object sender, EventArgs e)
    {
        eventId = new Guid(hidEventId.Value);

        string post = txtPost.Text;
        if(!string.IsNullOrEmpty(post))
        {
            UpdateEventPost(post, userId, eventId);
            LoadPosts();
        }
        Response.Redirect("~/" + hidEventName.Value);
    }
    
    public void LoadPosts()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var posts = from ep in dc.EventPosts
                    join p in dc.Profiles on ep.UserId equals p.UserId
                    where ep.EventId == eventId
                    orderby ep.CreatedOn descending
                    select new { p.ProfileId, ep.Post, p.UserId, ep.EventPostId, ep.CreatedOn, fullname = "<b>" + p.Firstname + " " + p.Lastname + "</b> posted an update.", ProfilePhoto = (p.Photo == null ? "Avatar.png" : p.Photo) };

        rptPosts.DataSource = posts;
        rptPosts.DataBind();
    }

    protected void LoadLinkCategories()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var categories = from c in dc.Categories
                            orderby c.ParentCategory, c.Category1
                            select new {c.Order, c.ParentCategory, category = c.ParentCategory + " - " + c.Category1, c.CategoryId };

        ddlCateogry.DataSource = categories;
        ddlCateogry.DataBind();
    }

    protected void rptPosts_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        { 
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            HyperLink hypFullname = (HyperLink)e.Item.FindControl("hypFullname");
            Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "userId");
            string fullname = (string)DataBinder.Eval(dataItem.DataItem, "fullname");
            hypFullname.Text = fullname;
            hypFullname.NavigateUrl = "~/V1/Profile/Profile.aspx?userId=" + userId;

            if(User.IsInRole("Administrator"))
            {
                Guid eventPostId = (Guid)DataBinder.Eval(dataItem.DataItem, "eventPostId");
                HtmlGenericControl divDelete = (HtmlGenericControl)e.Item.FindControl("divDelete");
                LinkButton lbDelete = (LinkButton)e.Item.FindControl("lbDelete");

                divDelete.Visible = true;
                lbDelete.CommandArgument = eventPostId.ToString();
            }
        }
    }
    
    protected void lbDelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string eventPostId = btn.CommandArgument;
        
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var eventPost = (from ep in dc.EventPosts
                          where ep.EventPostId == new Guid(eventPostId)
                          select ep).SingleOrDefault();

        dc.EventPosts.DeleteOnSubmit(eventPost);
        dc.SubmitChanges();
        Response.Redirect("~/" + HttpContext.Current.Request.QueryString["eventName"]);
    }

    protected void btnAddLink_Click(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        Link link = new Link();

        link.CategoryId = new Guid(ddlCateogry.SelectedValue);
        link.Title = txtTitle.Value;
        link.Description = txtDescription.Value;
        link.LinkId = Guid.NewGuid();
        link.URL = txtUrl.Value;
        link.EventId = eventId;
        link.CreatedBy = userId;
        link.CreatedOn = DateTime.Now;

        dc.Links.InsertOnSubmit(link);
        dc.SubmitChanges();
        divAlertMessage.Visible = true;
        litMessage.Text = "New link added.";

        Response.Redirect("~/" + HttpContext.Current.Request.QueryString["eventName"]);
    }
}