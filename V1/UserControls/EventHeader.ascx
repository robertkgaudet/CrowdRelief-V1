<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EventHeader.ascx.cs" Inherits="V1_UserControls_EventHeader" %>

		<style>
		.eventBackground
			{
				background-image:url('../V1/Images/<%=eventBackgroundImage%>');
				background-repeat:no-repeat;
				-webkit-background-size:cover;
				-moz-background-size:cover;
				-o-background-size:cover;
				background-size:cover;
				background-position:center;
			}
		.rowBody
		{
			display: table;
		}

		.rowBody [class*="col-"]
		{
			float: none;
			display: table-cell;
			vertical-align: top;
		}
		</style>
		<div class="row rowBody">
			<div class="col-xs-6">
				<div class="hpanel h<%=headerColor%>">
					<div class="panel-body">
							<%=icon%> <asp:Literal ID="litEventName" runat="server"></asp:Literal>
						<h2 class="m-b-xs">
							<asp:Literal ID="litPageName" runat="server"></asp:Literal>
						</h2>
						<p class="font-bold text-info">
							<asp:Literal ID="litDate" runat="server"></asp:Literal>
						</p>
						<p>
							<small><asp:Literal ID="litEventDescription" runat="server"></asp:Literal></small>
						</p>
					
						<div id="hbreadcrumb" class="pull-left">
							<asp:Literal ID="litBreadcrumb" runat="server"></asp:Literal>
						</div>
					</div>
					<div class="alert alert-info" runat="server" id="divRegister" visible="false">
						To use all of the features of CrowdRelief please sign in or register. <i class="fa fa-user"></i> 
						<p class="m-t-sm">
							<a href="/Register" class="btn btn-success">Register Here</a>
							<a href="/SignIn" class="btn btn-success">Sign In</a>
						</p>	
					</div>
					<div class="alert alert-info" runat="server" id="divChooseDefaultDisaster" visible="false">
						<asp:Button ID="btnSetDefaultDisaster" CssClass="btn btn-info" CausesValidation="false" runat="server" Text="Set as Default Disaster" OnClick="btnSetDefaultDisaster_Click" />
						<asp:Label ID="lblDefaultEventMessage" runat="server"></asp:Label>
					</div>
				</div>
			</div>
			<div class="col-xs-6 eventBackground">
			</div>
		</div>