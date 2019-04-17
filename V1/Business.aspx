<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Business.aspx.cs" Inherits="V1_Business" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:EventHeader runat="server" ID="uc1EventHeader" />
	<div class="row m-t-lg">
					<div class="col-sm-6">
						<div class="hpanel">
							<div class="panel-heading hbuilt ">
								<h1>COMING SOON:</h1> Business Services Directory
							</div>
						</div>
					</div>
				</div>	
</asp:Content>