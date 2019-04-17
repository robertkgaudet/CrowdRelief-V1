<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Event.aspx.cs" Inherits="V1_Event" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>

<%@ Register Src="~/V1/UserControls/Links.ascx" TagPrefix="uc1" TagName="Links" %>
<%@ Register Src="~/S1/UserControls/DisasterSurvivorStoriesByDisaster.ascx" TagPrefix="uc1" TagName="DisasterSurvivorStoriesByDisaster" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/v1/Scripts/masonry.pkgd.min.js"></script>

	<script type="text/javascript">
		$(document).ready(function () {


			$('.btnRegisterNonProfit').click(function () {
				window.location.href = '<%=registerNonProfit%>';
				return false;
			})
			
			$('.volunteerPassed').click(function () {
				window.location.href = '/V1/Stream.aspx';
				return false;
			})

			$('.volunteerPending').click(function () {
				window.location.href = '/V1/Profile/Profile.aspx';
				return false;
			});

			$(".panelSurvivor").click(function () {
				location.href = "<%=btnSurvivorPage%>"; //"http://www.CajunRelief.org/Donate";
			});
			$(".panelHelper").click(function () {
				location.href = "<%=btnHelperPage%>"; //"http://www.CajunRelief.org/Donate";
			});
			$(".panelNonProfit").click(function () {
				location.href = "<%=btnNonProfitPage%>"; //"http://www.CajunRelief.org/Donate";
			});
			$(".panelBusiness").click(function () {
				location.href = "<%=btnBusinessPage%>"; //"http://www.CajunRelief.org/Donate";
			});
			


			$('.deleteLinkArticle').click(function () {
				swal({
					title: "Are you sure?",
					text: "Your will not be able to recover this imaginary file!",
					type: "warning",
					showCancelButton: true,
					confirmButtonColor: "#DD6B55",
					confirmButtonText: "Yes, delete it",
					cancelButtonText: "No, cancel please.",
					closeOnConfirm: false,
					closeOnCancel: false
				},
					function (isConfirm) {
						if (isConfirm) {
							swal("Deleted!", "Your entry has been deleted.", "success");
						} else {
							swal("Cancelled", "Your entry is safe :)", "error");
						}
					});
			});

			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});
		});
	</script>

	<style>
		.grid-item								{width:350px; }
		.disasterPanel, .activeDisasterPanel	{height:500px;}
		.disasterPanel:hover					{cursor:pointer; background-color:#FCF8E3; }
		.activeDisasterPanel:hover				{cursor:pointer; background-color:#62CB31; color:white; }
		.alert-success							{cursor:pointer; background-color:#62CB31; color:white; }

		.EventLink
		{
			color:#365899;
		}
		.EventLink:hover
		{
			color:#365899;
			text-decoration:underline;
		}
		.panelSurvivor:hover, .panelHelper:hover, .panelNonProfit:hover, .panelBusiness:hover
		{
			cursor:pointer;
			border:solid white 1px;
		}
	</style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<asp:HiddenField runat="server" id="hidEventId"></asp:HiddenField>
		<asp:HiddenField runat="server" id="hidEventName"></asp:HiddenField>
		<uc1:EventHeader runat="server" ID="uc1EventHeader" />



			<div class="row m-t-md">
				<div class="col-sm-3 m-b-sm">
					<div class="hpanel hbgyellow panelSurvivor">
						<div class="panel-body">
							<div class="text-center">
								<h1>Survivors</h1>
								<span>
									Request Help, Rebuild or Supplies
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-3 m-b-sm">
					<div class="hpanel hbggreen panelHelper">
						<div class="panel-body">
							<div class="text-center">
								<h1>Helpers</h1>
								<span>
									Volunteer, Send Supplies, Donate
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-3 m-b-sm">
					<div class="hpanel hbgviolet panelNonProfit">
						<div class="panel-body">
							<div class="text-center">
								<h1>Non-profits</h1>
								<span>
									Register your Non-profit Response
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-3 m-b-sm">
					<div class="hpanel hbgnavyblue panelBusiness">
						<div class="panel-body">
							<div class="text-center">
								<h1>Business</h1>
								<span>
									Do Business or Request Help
								</span>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-sm-6">
                    <div class="hpanel hgreen" runat="server">
			            <div class="panel-heading hbuilt">
							<asp:Label id="lblQuickLinks" runat="server"></asp:Label>
			            </div>
						<div class="panel-body">
							<div class="row m-b">
								<div class="col-xs-4">
									<b> Total Survivors</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litSurvivorCount" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
									<asp:HyperLink id="hypAddASurvivor" runat="server" text="Add Survivor"></asp:HyperLink>
								</div>
							</div>
							<div class="row m-b">
								<div class="col-xs-4">
									<b>Home Rebuilds</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litHomesAdded" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
									<asp:HyperLink id="hypAddHome" runat="server" text="Add Home"></asp:HyperLink>
								</div>
							</div>
							<div class="row m-b">
								<div class="col-xs-4">
									<b> Total Volunteers</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litVolunteerCount" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
								</div>
							</div>
							<div class="row m-b">
								<div class="col-xs-4">
									<b>Volunteers Needed</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litVolunteers" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
									<asp:HyperLink id="hypVolunteer" runat="server" text="Volunteer"></asp:HyperLink>
								</div>
							</div>
							<div class="row m-b">
								<div class="col-xs-4">
									<b> Total NonProfits</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litNonprofitCount" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
									<asp:HyperLink id="hypAddNonProfit" runat="server" text="Add Nonprofit"></asp:HyperLink>
								</div>
							</div>
							<div class="row m-b">
								<div class="col-xs-4">
									<b>Rebuild Progress</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litRebuildProgress" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
									<asp:HyperLink id="hypUpdateRebuildProgress" runat="server" text="Update Rebuild" NavigateURL="/V1/Profile/Default.aspx"></asp:HyperLink>
								</div>
							</div>
							<div class="row m-b">
								<div class="col-xs-4">
									<b>Overall Progress</b>
								</div>
								<div class="col-xs-3">
									<asp:Literal ID="litOverallProgress" runat="server"></asp:Literal>
								</div>
								<div class="col-xs-5">
									<asp:HyperLink id="hypUpdateProgress" runat="server" text="Update Overall Progress" NavigateURL="/V1/Profile/Default.aspx"></asp:HyperLink>
								</div>
							</div>
						</div>
		            </div>
				</div>
				<div class="col-sm-6">
					<h3>
						<asp:Literal ID="litEventNameForMap" runat="server"></asp:Literal> - Distribution Centers, Supply Pickup and Dropoff and Shelters
					</h3>
					<iframe src="<%=mapURL%>" style="width:100%; height:800px; border:0;"></iframe>
				</div>
			</div>
			<div class="row m-t-md">
				<div class="col-sm-6">
					<div class="tab-content">
						<div id="tab-1" class="tab-pane active">
							<div class="panel-body no-padding">
								<div class="chat-discussion" style="height: auto">
									<h2>Post your needs or how you can help.</h2>
									<asp:Image ID="imgNewPostLogo" runat="server" CssClass="post-logo" />
									<div class="message">
										<a class="message-author" href="#">  </a>
										<span class="message-content">
											<asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtPost" runat="server"></asp:TextBox>
										</span>
										<div style="overflow:auto;">
											<asp:LinkButton ID="btnRebuildPost" CausesValidation="false" OnClick="btnRebuildPost_Click" CssClass="btn w-xs btn-sm btn-primary pull-right m-t-sm" runat="server" Text="Post" />
										</div>
									</div>
									<asp:Repeater ID="rptPosts" runat="server" OnItemDataBound="rptPosts_OnItemDataBound">
										<ItemTemplate>
											<div class="hpanel">
												<div class="panel-body">
													<div class="message">
														<div class="blog-article-box">
															<asp:HyperLink id="hypFullname" runat="server"></asp:HyperLink>
														</div>
														<span class="message-date">
															<%# DataBinder.Eval(Container.DataItem, "createdon", "{0:M/d/yyyy HH:mm:ss}") %>
														</span>
														<span class="message-content">
															<%# DataBinder.Eval(Container.DataItem, "Post") %>
															<div class="pull-right" runat="server" id="divDelete" visible="false">
																<asp:LinkButton id="lbDelete" runat="server" OnClick="lbDelete_Click" text="Delete"></asp:LinkButton>
															</div>
														</span>
													</div>
												</div>
											</div>
										</ItemTemplate>
									</asp:Repeater>
								</div>
							</div>
						</div>
						<div id="tab-2" class="tab-pane">
							<div class="panel-body">
								<strong>Lorem ipsum dolor sit amet, consectetuer adipiscing</strong>

								<p>A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of
									existence in this spot, which was created for the bliss of souls like mine.</p>

								<div class="table-responsive">
										
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-6">
					<div>
					</div>
				</div>
			</div>

			<div class="row">
				<div class="col-lg-12">
					<div class="row">
						<div class="col-sm-12">
							<div class="alert alert-warning" runat="server" visible="false" id="divRequestAdmin">
								If you would like to help manage rescource information for this disaster please email <a href="mailto:RobGaudet@CrowdRelief.net" class="EventLink">RobGaudet@CrowdRelief.net</a>.
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-sm-12">
					<uc1:DisasterSurvivorStoriesByDisaster runat="server" ID="ucDisasterSurvivorStoriesByDisaster" />
				</div>
			</div>
			<div class="row">
				<div class="col-sm-12">
					<div class="hpanel hgreen" runat="server" visible="false" id="divAdmin">
						<div class="panel-tools m-sm">
							<a class="showhide"><i class="fa fa-chevron-up"></i></a>
							<a class="closebox"><i class="fa fa-times"></i></a>
						</div>
						<div class="alert alert-info">
							<i class="fa fa-lock"></i> Administrative Tools
						</div>
						<div class="panel-body">
							<div>
								<div class="row">
									<div class="col-sm-12 well">
										<div class="hpanel panel-body">
											<div class="panel-heading">
											<h2>Add A New Link</h2>
												<asp:HyperLink ID="hypNewCategory" CssClass="EventLink" runat="server" Text="Add A New Article"></asp:HyperLink>
											</div>
											<div class="alert alert-success m-b" runat="server" id="divAlertMessage" visible="false"><asp:Literal ID="litMessage" runat="server"></asp:Literal></div>
											<div class="form-group m-t-md">
												<div class="input-group">
													<div class="input-group-addon">
														Choose Link Category
													</div>
													<asp:DropDownList CssClass="form-control" ID="ddlCateogry" runat="server" DataTextField="category" DataValueField="CategoryId" ></asp:DropDownList>
												</div>
											</div>
											<div class="form-group ">
												<div class="input-group">
													<div class="input-group-addon">
														Link URL
													</div>
													<input class="form-control" id="txtUrl" runat="server" name="txtUrl" type="url" placeholder="URL" required/>
												</div>
											</div>
											<div class="form-group ">
												<div class="input-group">
													<div class="input-group-addon">
														Link Title
													</div>
													<input class="form-control" id="txtTitle" runat="server" name="txtTitle" type="text" placeholder="Link Title" required/>
												</div>
											</div>
											<div class="form-group ">
												<div class="input-group">
													<div class="input-group-addon">
														Link Description
													</div>
													<textarea class="form-control" cols="40" rows="10" id="txtDescription" runat="server" name="txtDescription" type="text" placeholder="Link Description" required></textarea>
												</div>
											</div>
											<div class="form-group ">
												<div class="input-group pull-right">
													<asp:Button ID="btnAddLink" runat="server" Text="Add Link" CssClass="btn btn-primary" OnClick="btnAddLink_Click" />
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div class="panel-footer hbuilt">
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>

