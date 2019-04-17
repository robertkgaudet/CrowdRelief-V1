using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class S1_Post : BaseOrganizationWebForm
{
	public string panelImage = string.Empty;
	public string panelImageSrc = string.Empty;
	public string pageFriendlyURL = string.Empty;
	public string articleAboutUserRole = "Survivor";
	public string survivorLinkList = string.Empty;
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();
	public string disasterHelperStoryCategoryId	= ConfigurationManager.AppSettings["disasterHelperStoryCategoryId"].ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
		Page.Form.DefaultButton = btnComment.UniqueID;
		string delete = Request.QueryString["d"];
	//	Master.BoxedBody = true;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if(!String.IsNullOrEmpty(delete))
		{
			Guid commentId = new Guid(Request.QueryString["commentId"]);
			//delete the comment.
			var comment = (from c in dc.Comments
						  where c.CommentId == commentId
						  select c).SingleOrDefault();

			comment.IsDeleted = true;
			dc.SubmitChanges();

			Response.Redirect("\\S1\\Default.aspx");
		}

		if(!IsPostBack)
		{ 
			string storyPhotoFolder		= System.Configuration.ConfigurationManager.AppSettings["storyPhotoFolder"].ToString();
			string articleNumber = Request.QueryString["number"];
			this.Master.HideCategoryList = true;

			if(!String.IsNullOrEmpty(articleNumber))
			{
				var article = (from a in dc.Articles
								join u in dc.Profiles on a.CreatedBy equals u.UserId
								where a.ArticleNumber == Int32.Parse(articleNumber)
								&& a.IsDeleted == false
								select new {a, u }).SingleOrDefault();
			
				if(article == null)
				{
					Response.Redirect("/stories");
				}


				var storyPhoto = (from sp in dc.ArticlePhotos
								 join p in dc.Photos on sp.PhotoId equals p.PhotoId
								 where sp.ArticleId == article.a.ArticleId
								 orderby sp.CreatedOn descending
								 select new {p.FilenameCropped }).Take(1).SingleOrDefault();

				
				
				Guid loggedInUserId = Guid.Empty;
				if(User.Identity.IsAuthenticated)
				{
					loggedInUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					
					if(User.IsInRole("Administrator") || article.u.UserId == loggedInUserId)
					{
						divArticleAdmin.Visible = true;
						hypAddPhoto.NavigateUrl = "/S1/Profile/BlogUploadPhoto.aspx?userId=" + article.u.UserId + "&articleId=" + article.a.ArticleId;
						hypEdit.NavigateUrl		= "/S1/Profile/EditBlogPost.aspx?userId=" + article.u.UserId + "&articleId=" + article.a.ArticleId;
						hypDelete.NavigateUrl	= "/S1/Default.aspx?d=t&userId=" + article.u.UserId + "&articleId=" + article.a.ArticleId;
					}
				}

				string storyTitle = article.a.Title;
				string articlePhoto = string.Empty;
				hidArticleId.Value = article.a.ArticleId.ToString();
				hidTitle.Value = article.a.Title.Replace(" ","-");

				if(storyPhoto != null)
				{
					panelImage = "panel-image";
					panelImageSrc = "<img class=\"img-responsive\" src=\"" + storyPhotoFolder + storyPhoto.FilenameCropped + "\" \\>";
					//storyTitle = string.Empty;
				}

				//BEGIN FACEBOOK META TAGS
				string facebookArticleImage = "/S1/Images/CrowdReliefFBPost.jpg";

				pageFriendlyURL = "http://www.crowdrelief.net/a/" + article.a.ArticleNumber + "/" + article.a.Title.Replace(" ","-");

				Master.FbURL			= pageFriendlyURL;//Request.Url.AbsoluteUri;
				Master.FbDescription	= StripHTML(article.a.Text.Length > 501 ? article.a.Text.Substring(0, 500) + "..." : article.a.Text);
				Master.PageDescription	= StripHTML(article.a.Text.Length > 501 ? article.a.Text.Substring(0, 500) + "..." : article.a.Text);

				if(storyPhoto != null)
				{
					facebookArticleImage = storyPhotoFolder + storyPhoto.FilenameCropped;
				}

				Master.FbImage			= facebookArticleImage;
				Master.FbImageType		= "image/jpg";
				Master.FbSite_name		= "CrowdRelief Disaster Aid Platform";
				//END FACEBOOK META TAGS

				string articleDate = article.a.CreatedOn.ToString("MM/dd/yyyy h:mm tt");

				int? countInit = 0;
				if(article.a.ViewCount != null)
				{
					countInit = article.a.ViewCount;
				}
				article.a.ViewCount = countInit + 1;
				dc.SubmitChanges();

				int? viewCount = 0;
				if(article.a.ViewCount != null)
				{
					viewCount = (article.a.ViewCount + 248);
				}

				var commentCount = (from c in dc.ArticleComments
								   where c.ArticleId == article.a.ArticleId
								   select c).Count();
				
				litTitle.Text			= storyTitle;
				litViews.Text			= viewCount.ToString();
				litCommentCount.Text	= commentCount.ToString();
			
				var storyUser = (from s in dc.Profiles
							   where s.UserId == article.a.UserId
							   select s).SingleOrDefault();

				string headerSubTitle = string.Empty;
				string survivorInfo = string.Empty;
				if(storyUser != null)
				{
					if(article.a.CategoryId != null && new Guid(disasterSurvivorStoryCategoryId) == article.a.CategoryId)
					{
						//SURVIVOR
						articleAboutUserRole = "Survivor";
						dtRecoveryStage.Visible = true;
						litRecoveryStage.Visible = true;
						dtHousingTitle.Visible = true;
						litHousingSituation.Visible = true;
						dtQualifiersTitle.Visible = true;
						litQualifiers.Visible = true;

						
						var survivorHousing = (from sh in dc.UserHousings
											  join h in dc.Housings on sh.HousingId equals h.HousingId
											  where sh.UserId == article.a.UserId
											  orderby sh.CreatedOn descending
											  select new {h }).Take(1).SingleOrDefault();

						if(survivorHousing != null)
						{
							dtHousingTitle.Visible		= true;
							litHousingSituation.Visible = true;
							litHousingSituation.Text	= survivorHousing.h.Sentence + " " + survivorHousing.h.Housing1;
						}

						if(!String.IsNullOrEmpty(storyUser.AmazonWishListURL))
						{
							litHelpSurvivorMessage.Text = "<p>Help " + storyUser.Firstname + " " + storyUser.Lastname +  " by purchasing much needed items from their Amazon Wish List.</p>";

							hypAmazon.Text = "Donate From My Amazon Wish List";
							hypAmazon.NavigateUrl = storyUser.AmazonWishListURL;
						}
						else
						{
							hypAmazon.Visible = false;
						}
						
						
						var recoveryStage = (from ust in dc.UserSliderTicks
											join s in dc.SliderTicks on ust.SliderTickId equals s.SliderTickId
											where ust.SurvivorId == article.a.UserId
											orderby ust.CreatedOn descending
											select new {s.Lable }).Take(1).SingleOrDefault();

						if(recoveryStage != null)
						{
							litRecoveryStage.Visible = true;
							dtRecoveryStage.Visible = true;
							litRecoveryStage.Text = recoveryStage.Lable;
						}

						string qualifierList = string.Empty;

						var qualifiers = from rs in dc.RebuildStatus
										join uq in dc.UserQualifiers on rs.RebuildStatusId equals uq.QualifierId
										where rs.StatusType == 2 && uq.UserId == article.a.UserId
										orderby rs.Status
										select new { rs };

						if(qualifiers.Count() > 0)
						{
							foreach(var qualifier in qualifiers)
							{
								qualifierList += "<span class=\"btn btn-default m-xs btn-m\">" + qualifier.rs.Status + "</span>";
							}
							litQualifiers.Visible	= true;
							litQualifiers.Text		= qualifierList;
							dtQualifiersTitle.Visible	= true;
						}
						
						Master.PageTitle		= storyTitle + " - CrowdRelief Disaster Survivors Story";
						headerSubTitle = "This CrowdRelief Disaster Survivors Story was created for <a href=\"\\sp\\" +  storyUser.ProfileNumber + "\\" + storyUser.Firstname + "-" + storyUser.Lastname + "\"\">" + storyUser.Firstname + " " + storyUser.Lastname + "</a> by <a href=\"\\hp\\" +  article.u.ProfileNumber + "\\" + article.u.Firstname + "-" + article.u.Lastname + "\"\">" +  article.u.Firstname + " " + article.u.Lastname + "</a> who is helping them rebuild their lives. " + articleDate;
					}
					else if(article.a.CategoryId != null && new Guid(disasterHelperStoryCategoryId) == article.a.CategoryId)
					{
						//HELPER
						hypAmazon.Visible = false;
						LoadSurvivors(article.a.UserId);
						articleAboutUserRole = "Helper";
						dtRecoveryStage.Visible = false;
						litRecoveryStage.Visible = false;
						dtHousingTitle.Visible = false;
						litHousingSituation.Visible = false;
						dtQualifiersTitle.Visible = false;
						litQualifiers.Visible = false;
						
						Master.PageTitle		= storyTitle + " - CrowdRelief Disaster Volunteer Story";
						headerSubTitle = "This CrowdRelief Disaster Helpers Story was created for helper <a href=\"\\hp\\" +  storyUser.ProfileNumber + "\\" + storyUser.Firstname + "-" + storyUser.Lastname + "\"\">" + storyUser.Firstname + " " + storyUser.Lastname + "</a>. " + articleDate;
					}
					litHeaderSubTitle.Text = headerSubTitle;
					
					var survivorEvent = (from d in dc.Events
										 join ue in dc.UserEvents on d.EventId equals ue.EventId
										 where ue.UserId == article.a.UserId
										 select new {d.Name, d.URLFriendlyName}).Take(1).SingleOrDefault();

					if(survivorEvent != null)
					{
						dtDisasterTitle.Visible		= true;
						hypDisaster.Visible			= true;
						hypDisaster.Text			= survivorEvent.Name;
						hypDisaster.NavigateUrl		= "/" + survivorEvent.URLFriendlyName;
						hypDisaster.Font.Underline	= true;
						litDisasterName.Visible		= true;
						litDisasterName.Text		= survivorEvent.Name + " Disaster " + articleAboutUserRole;
					}

					
					divSurvivorInfo.Visible		= true;
					litText.Text				= article.a.Text;
					litText.Visible				= true;
					litSurvivorName.Text		= storyUser.Firstname + " " + storyUser.Lastname;
					litSurvivorName.Visible		= true;
					hypAllArticles.NavigateUrl	= "\\s\\" + storyUser.ProfileNumber + "\\" +  storyUser.Firstname + "-" + storyUser.Lastname;
					hypAllArticles.Visible		= true;
					hypAllArticles.Text			= "More Articles...";	
				}
				else
				{
					divSurvivorInfo.Visible		= false;
					litTextNoSurvivor.Visible	= true;
					litTextNoSurvivor.Text		= article.a.Text;
				}

				LoadCategories(article.a.ArticleId);
				LoadComments(article.a.ArticleId);
			}
		}
	}
	protected string StripHTML(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			input = Regex.Replace(input, "<.*?>", String.Empty);
		}
		return input;
	}
	
	
	protected void LoadSurvivors(Guid? profileUserId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var survivorProfiles = (from uu in dc.UserUsers
								join p in dc.Profiles on uu.AcceptingUserId equals p.UserId
								where uu.RequestingUserId == profileUserId
								orderby p.Lastname
								select new {p}).Distinct();

		if(survivorProfiles != null && survivorProfiles.Count() > 0)
		{
			dtSurvivorLinkList.Visible = true;
		}

		foreach(var survivorProfile in survivorProfiles)
		{
			survivorLinkList = survivorLinkList + "<li><a href=\"\\sp\\" + survivorProfile.p.ProfileNumber + "\\" + survivorProfile.p.Firstname + "-" + survivorProfile.p.Lastname  + "\">" + survivorProfile.p.Firstname + " " + survivorProfile.p.Lastname +  "</a></li>" + Environment.NewLine;
		}
	}



	protected void LoadComments(Guid articleId)
	{
		string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var articleComments = from c in dc.Comments
							  join ac in dc.ArticleComments on c.CommentId equals ac.CommentId
							  join p in dc.Profiles on ac.UserId equals p.UserId
							  where ac.ArticleId == articleId && c.IsDeleted == false
							  orderby c.CreatedOn descending
							  select new {c, ac, p};

		string comments = string.Empty;

		foreach(var articleComment in articleComments)
		{
			string profilePhoto = string.Empty;

			var profileImage = (from ph in dc.ProfilePhotos
							   join p in dc.Photos on ph.PhotoId equals p.PhotoId
							   where ph.UserId == articleComment.p.UserId && ph.IsCurrrent == true
							   orderby p.CreatedOn descending
							   select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if(profileImage != null)
			{
				//Get the users profile image
				profilePhoto = profilePhotoFolder + profileImage.FilenameCropped;
			}

			string createdOn = GetElapsedTime(articleComment.ac.CreatedOn); //articleComment.ac.CreatedOn.ToString("MM/dd/yyyy h:mm tt");
			string comment = articleComment.c.Comment1;
			string fullname = articleComment.p.Firstname + " " + articleComment.p.Lastname;

			string delete = string.Empty;
			if(User.IsInRole("Administrator"))
			{
				delete = "<div><a href=\"Post.aspx?articleId=" + articleId + "&d=true&commentId=" + articleComment.c.CommentId + "\">Delete</a></div>";
			}

			comments += "<div class=\"social-talk\">" +  Environment.NewLine +
							"<div class=\"media social-profile clearfix\">" +  Environment.NewLine +
								"<a class=\"pull-left\">" +  Environment.NewLine +
									"<img src=\"" + profilePhoto + "\" alt=\"post-picture\">" +  Environment.NewLine +
								"</a>" +  Environment.NewLine +
								"<div class=\"media-body\">" +  Environment.NewLine +
									"<span class=\"font-bold\">" + fullname + "</span>" +  Environment.NewLine +
									"<small class=\"text-muted\">" + createdOn + "</small>" +  Environment.NewLine +
									"<div class=\"social-content\">" +  Environment.NewLine +
										comment +  Environment.NewLine +
									"</div>" +  Environment.NewLine +
								"</div>" +  Environment.NewLine +
							"</div>" +  Environment.NewLine +
							delete + 
						"</div>";
		}
		litComments.Text = comments;
	}

	protected void LoadCategories(Guid articleId)
	{
		string categoryList = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var categories = from c in dc.Categories
						join ac in dc.ArticleCategories on c.CategoryId equals ac.CategoryId
						where ac.ArticleId == articleId
						orderby c.Category1
						select new { c.Category1 };

		if(categories.Count() > 0)
		{
			foreach(var category in categories)
			{
				categoryList += "<a href=\"\\c\\" + category.Category1.Replace(" ","-").Replace("/","_") + "\" class=\"btn btn-default m-xs btn-m\">" + category.Category1 + "</a>";
			}
			litCategory.Text = categoryList;
		}
	}

	protected void imgAmazonButton_Click(object sender, ImageClickEventArgs e)
	{
		ImageButton btn = (ImageButton)(sender);
		Response.Redirect(btn.CommandArgument);
	}

	protected void btnComment_Click(object sender, EventArgs e)
	{
		string commentText = txtComment.Text;
		Guid commentId = Guid.NewGuid();
		Guid articleId = new Guid(hidArticleId.Value);
		string title =  hidTitle.Value;

		commentText = Linkify(commentText);


		Comment comment = new Comment();
		comment.Comment1 = commentText;
		comment.CommentId = commentId;
		comment.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		comment.CreatedOn = DateTime.Now;
		comment.IsDeleted = false;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		dc.Comments.InsertOnSubmit(comment);
		dc.SubmitChanges();

		ArticleComment articleComment = new ArticleComment();
		articleComment.ArticleCommentId = Guid.NewGuid();
		articleComment.ArticleId = articleId;
		articleComment.CreatedOn = DateTime.Now;
		articleComment.CommentId = commentId;
		articleComment.IsDeleted = false;
		articleComment.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

		dc.ArticleComments.InsertOnSubmit(articleComment);
		dc.SubmitChanges();
		
		string articleNumber = Request.QueryString["number"];

		Response.Redirect("\\a\\" + articleNumber + "\\" + title);
	}
	protected string Linkify( string SearchText ) {
	// this will find links like:
	// http://www.mysite.com
	// as well as any links with other characters directly in front of it like:
	// href="http://www.mysite.com"
	// you can then use your own logic to determine which links to linkify
	Regex regx = new Regex( @"\b(((\S+)?)(@|mailto\:|(news|(ht|f)tp(s?))\://)\S+)\b", RegexOptions.IgnoreCase );
	SearchText = SearchText.Replace( "&nbsp;", " " );
	MatchCollection matches = regx.Matches( SearchText );

	foreach ( Match match in matches ) {
		if ( match.Value.StartsWith( "http" ) ) { // if it starts with anything else then dont linkify -- may already be linked!
			SearchText = SearchText.Replace( match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>" );
		}
	}

	return SearchText;
}
}