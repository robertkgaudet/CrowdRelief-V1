using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CrowdRelief
{

	/// <summary>
	/// Summary description for Tools
	/// </summary>
	public class Tools
	{
		public Tools()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string GetColor(string color)
		{
			switch(color)
			{
				case "danger":
					{
						color = "red";
					}
					break;
				case "warning":
					{
						color = "yellow";
					}
					break;
				case "info":
					{
						color = "blue";
					}
					break;
			}
			return color;
		}

		public static void SendEmailTemplate(string firstName, string lastName, string recipientEmail, string message, string subject, string htmlFilePath, System.Web.UI.Control obj)
		{
			//htmlFilePath "~\\CreateAccount.html"
			string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
			string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

			ListDictionary ldEmailBodyReplacements = new ListDictionary();
			if (!String.IsNullOrEmpty(firstName))
			{
				ldEmailBodyReplacements.Add("<%Firstname%>", firstName);
			}
			if (!String.IsNullOrEmpty(lastName))
			{
				ldEmailBodyReplacements.Add("<%Lastname%>", lastName);
			}
			if (!String.IsNullOrEmpty(message))
			{
				ldEmailBodyReplacements.Add("<%Message%>", message);
			}
			if (!String.IsNullOrEmpty(subject))
			{
				ldEmailBodyReplacements.Add("<%Subject%>", subject);
			}

			MailDefinition mailDefinition = new MailDefinition();

			mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath(htmlFilePath);
			mailDefinition.Subject = subject;
			mailDefinition.IsBodyHtml = true;
			//mailDefinition.From = emailFrom;
			//mailDefinition.CC = "support@crowdrelief.net";

			MailMessage msg = mailDefinition.CreateMailMessage(recipientEmail, ldEmailBodyReplacements, obj);
			//msg.To.Add(new MailAddress(recipientEmail, fullname));
			
			msg.From = new MailAddress(emailFrom, emailFromDisplayName);
			msg.Bcc.Add(new MailAddress("support@crowdrelief.net", "CrowdRelief Support"));
			//msg.Bcc.Add(new MailAddress("robgaudet@crowdrelief.net", "Rob Gaudet"));
			//msg.Bcc.Add(new MailAddress("melissa.adair@gaudet.media", "Melissa Adair"));
			//msg.Bcc.Add(new MailAddress("rob.gaudet@gaudet.media", "Rob Gaudet"));

			SmtpClient smtp = new SmtpClient();
			smtp.EnableSsl = true;
			smtp.Send(msg);
		}

		public static string CalculateVolunteersNeeded(Guid userId, int points, bool returnAllNeededVolunteers, Guid eventId)
		{
			string volunteerCount = string.Empty;
			int totalVolunteersNeeded = 0;
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			if (returnAllNeededVolunteers)
			{
				var pointsVolunteerCounts = (from r in dc.Rebuilds
											 where r.CreatedBy == userId
											 group r by r.Difficulty into g
											 select new { diffculty = g.Key, totalDifficulty = g.Sum(x => x.Difficulty), totalCount = g.Count() });

				if (pointsVolunteerCounts != null)
				{
					int volunteerCountForMath = 0;
					foreach (var item in pointsVolunteerCounts)
					{
						switch (item.diffculty)
						{
							case 0:
								{
									volunteerCountForMath = 1;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath; //Just count all times 0 was selected, since there is one volunteer per count.

									break;
								}
							case 1:
								{
									volunteerCountForMath = 5;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;

									break;
								}
							case 2:
								{
									volunteerCountForMath = 10;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


									break;
								}
							case 3:
								{
									volunteerCountForMath = 20;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


									break;
								}
						}
					}
				}
				volunteerCount = totalVolunteersNeeded.ToString();
			}
			else if(userId == Guid.Empty && eventId != null)
			{
				var pointsVolunteerCounts = (from r in dc.Rebuilds
											 where r.EventId == eventId
											 group r by r.Difficulty into g
											 select new { diffculty = g.Key, totalDifficulty = g.Sum(x => x.Difficulty), totalCount = g.Count() });
				
				if (pointsVolunteerCounts != null)
				{
					int volunteerCountForMath = 0;
					foreach (var item in pointsVolunteerCounts)
					{
						switch (item.diffculty)
						{
							case 0:
								{
									volunteerCountForMath = 1;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath; //Just count all times 0 was selected, since there is one volunteer per count.

									break;
								}
							case 1:
								{
									volunteerCountForMath = 5;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;

									break;
								}
							case 2:
								{
									volunteerCountForMath = 10;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


									break;
								}
							case 3:
								{
									volunteerCountForMath = 20;
									totalVolunteersNeeded += (int)item.totalCount * volunteerCountForMath;


									break;
								}
						}
					}
				}
				volunteerCount = totalVolunteersNeeded.ToString();
			}
			else
			{
				// 0 points		- 1 person, Quick jobs can be completed in less than 1 day with 1 person.
				// 1 point		- 5 persons, Easy jobs can be completed in a couple of days with minimal volunteers.
				// 2 points		- 10 persons, Moderately difficult jobs that take many weeks and require a team with few professional skillsets.
				// 3 points		- 20 persons, Extremely difficult jobs that require months of work, planning and require multiple professional skillsets.

				switch (points)
				{
					case 0:
						{
							volunteerCount = "1";
							break;
						}
					case 1:
						{
							volunteerCount = "5";
							break;
						}
					case 2:
						{
							volunteerCount = "10";
							break;
						}
					case 3:
						{
							volunteerCount = "20";
							break;
						}
				}
			}

			return volunteerCount;
		}

		public static void SendEmail(string firstName, string lastName, string htmlFilePath, string subject, string recipientEmail, System.Web.UI.Control obj)
		{
			try{
				//htmlFilePath "~\\CreateAccount.html"
				string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
				string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

				ListDictionary ldEmailBodyReplacements = new ListDictionary();
				if (!String.IsNullOrEmpty(firstName))
				{
					ldEmailBodyReplacements.Add("<%Firstname%>", firstName);
				}
				if(!String.IsNullOrEmpty(lastName))
				{ 
					ldEmailBodyReplacements.Add("<%Lastname%>", lastName);
				}

				MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

				MailDefinition mailDefinition = new MailDefinition();

				mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath(htmlFilePath);
				mailDefinition.Subject = subject;
				mailDefinition.IsBodyHtml = true;

				MailMessage newUserMailMessage = mailDefinition.CreateMailMessage(recipientEmail, ldEmailBodyReplacements, obj);

				SmtpClient smtp = new SmtpClient();
				//smtp.EnableSsl = true;
				smtp.Send(newUserMailMessage);
			}
			catch(Exception ex)
			{}
			}

		public static string GetElapsedTime(DateTime time)
		{
			string elapsedTime = "0";
			DateTime rightNow = DateTime.Now;

			TimeSpan interval = rightNow - time;

			if (interval.TotalSeconds < 60)
			{
				//Less than 60 minutes then show the interval in minutes.
				elapsedTime = interval.Seconds.ToString() + " " + (interval.Seconds == 1 || interval.Seconds == 0 ? "second ago" : "seconds ago");
			}
			else if (interval.TotalMinutes < 60)
			{
				//Less than 60 minutes then show the interval in minutes.
				elapsedTime = interval.Minutes.ToString() + " " + (interval.Minutes > 1 ? "minutes ago" : "minute ago");
			}
			else if (interval.TotalHours < 24)
			{
				//Less than 24 hours show hours passed.
				elapsedTime = interval.Hours.ToString() + " " + (interval.Hours > 1 ? "hours ago" : "hour ago");
			}
			else if (interval.TotalDays < 7)
			{
				//More than 24 hours show hours passed.
				elapsedTime = time.Date.DayOfWeek + " at " + time.ToShortTimeString();
			}
			else
			{
				elapsedTime = time.ToLongDateString();
			}

			return elapsedTime;
		}
	}
}