<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfitCampaign.aspx.cs" Inherits="V1_NonProfit_NonProfitCampaign" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.img-rounded{max-width:100px;}
	</style>
	<script type="text/javascript">

		$(document).ready(function () {
			$('.donateButton').click(function () {
				window.location.href = '<%=donateLink%>';
				return false;
			});
			$(document).ready(function () {
				$('.volunteerButton').click(function () {
					window.location.href = '<%=volunteerLink%>';
				return false;
			});
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="normalheader ">
	<div class="hpanel">
		<div class="panel-body">
			<div class="row">
				<div class="col-xs-12">
					<%=icon%> <asp:Literal ID="litEventName" runat="server"></asp:Literal>
					<h2 class="font-light m-b-xs">
						<asp:Literal id="litCampaignName" runat="server"></asp:Literal>
					</h2>
				</div>
			</div>
			<div class="row">
				<div class="col-xs-6">
					<dl class="dl-horizontal">
						<dt>
							Point of Contact
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblPointOfContactPerson" runat="server"></asp:Label>
						</dd>
						
						<dt id="dtZelloChannel" runat="server">
							Zello Channel
						</dt>
						<dd id="ddZelloChannel" runat="server" class="m-b-sm">
							<asp:Label id="lblZelloChannel" runat="server"></asp:Label>
						</dd>

						<dt id="dtPhone" runat="server">
							Phone Number
						</dt>
						<dd id="ddPhone" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPointOfContactPhone" runat="server"></asp:HyperLink>
						</dd>
						
						<dt id="dtEmail" runat="server">
							Email (Unpublished)
						</dt>
						<dd id="ddEmail" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPointOfContactEmail" runat="server"></asp:HyperLink>
						</dd>

						<dt id="dtPrimaryPhone" runat="server">
							Phone (Unpublished)
						</dt>
						<dd id="ddPrimaryPhone" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPrimaryPhone" runat="server" target="new"></asp:HyperLink>
						</dd>

						<dt runat="server" id="dtWebsite" visible="false">
							Website
						</dt>
						<dd runat="server" id="ddWebsite" visible="false" class="m-b-sm">
							<asp:HyperLink id="hypWebsite" runat="server" target="new"></asp:HyperLink>
						</dd>
					</dl>
					<div class="form-group" runat="server" visible="false" id="divEditCampaign">
						<div class="pull-right">
							<asp:Button id="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit Campaign" />
						</div>
					</div>
				</div>
				<div class="col-xs-6">
					<b>Campaign Mission</b>
					<p>
						<asp:Literal id="litCampaignMission" runat="server"></asp:Literal>
					</p>
					<p>
						<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success donateButton" Text="Donate" visible="false"></asp:LinkButton>
						<asp:LinkButton id="lbVolunteer" runat="server" CssClass="btn btn-success volunteerButton" Text="Volunteer" visible="false"></asp:LinkButton>
					</p>
					<p>
						<asp:label id="lblVolunteerInstructions" runat="server"></asp:label>
					</p>
					<i class="font-light m-b-xs">
						Campaign created by: <asp:HyperLink id="hypOrganizationName" runat="server" target="_blank"></asp:HyperLink>
					</i>
				</div>
			</div>
		</div>
        <div class="alert alert-success" runat="server" id="divAlertMessage" visible="false">
            <i class="fa fa-bolt"></i> <asp:Literal id="litAlertMessage" runat="server"></asp:Literal>
        </div>
	</div>
</div>

<div class="content">
	<div class="row">
		<div class="col-lg-6">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Social Media Links
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
						<dt>
							Facebook Page
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypFacebookPage" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Facebook Group
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypFacebookGroup" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Blog
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypBlog" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							YouTube Channel
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypYouTube" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Twitter
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypTwitter" runat="server" target="new"></asp:HyperLink>
						</dd>
					</dl>
				</div>
			</div>
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					This Campaign Created By:
				</div>
				<div class="panel-body">
					<dl class="dl-verticle">
						<dd>
							<asp:Label id="lblParentOrgName" runat="server"></asp:Label>
						</dd>
						<dd id="ddParentAddress" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypParentAddress" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddParentPhone" runat="server" visible="false">
							<asp:HyperLink id="hypParentPhone" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddParentEmail" runat="server" visible="false">
							<asp:HyperLink id="hypParentEmail" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddParentWebsite" runat="server" visible="false">
							<asp:HyperLink id="hypParentWebsite" runat="server" target="new"></asp:HyperLink>
						</dd>	
					</dl>
					<dl class="dl-horizontal">
						<dt>
							VOAD Member
						</dt>
						<dd>
							<asp:Label id="lblVoadMember" runat="server"></asp:Label>
						</dd>
						<dt>
							501c3
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lbl501c3" runat="server"></asp:Label>
						</dd>
					</dl>
				</div>
			</div>
		</div>
		<div class="col-lg-6">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Volunteers
				</div>
				<div class="panel-body">
					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<HeaderTemplate>
							<table id="nonProfitsTable" class="footable table table-bordered table-hover" data-page-size="20" data-filter="#filter">
								<thead>
									<tr>
										<th data-toggle="true">Full Name</th>
										<th data-toggle="true" data-hide="phone,tablet">Skills</th>
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
								<tr>
									<td>
										<div class="profile-picture">
											<asp:Image CssClass="pull-left m-r-md img-rounded" ID="imgProfilePhoto" runat="server"></asp:Image>
										</div>
										<asp:Literal id="lblInfo" runat="server"></asp:Literal>
									</td>
									<td><asp:Label ID="lblSkills" runat="server"></asp:Label></td>
								</tr>
						</ItemTemplate>
						<FooterTemplate>
								</tbody>
								<tfoot>
									<tr>
										<td colspan="2">
											<ul class="pagination pull-right"></ul>
										</td>
									</tr>
								</tfoot>
							</table>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</div>
		</div>
	</div>
</div>
</asp:Content>

