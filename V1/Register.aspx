<%@ Page Title="" Language="C#" MasterPageFile="~/S1/MasterPages/HomerNoNavigation.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="V1_Register" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/HomerNoNavigation.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<title>Sign Up for CrowdRelief</title>
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>

	<script type="text/javascript">
		$(document).ready(function ()
		{
			<%=preselectedDisasterJQuery%>

			$("#disasterEvent.dropdown-menu li").click(function ()
			{
				$("#btn-dropdown.disasterEvent").html($(this).text());
				$("#<%=hidEventId.ClientID%>").val($(this).attr('id'));
			});

		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	
    <div class="middle-box text-center loginscreen animated fadeInDown m-t-lg">
        <div>
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
				<img class="img-responsive" src="/V1/Images/Logo-Horizontal.png" />
				<div class="p-sm text-center">
					CrowdRelief is the Official Disaster Recovery Platform of the 
					<b><a href="http://www.CajunRelief.org" target="_blank"><img class="right" src="/S1/Images/CajunNavySmallLogo.png" /> Cajun Navy</a></b>
				</div>
            </div>
            <div class="col-sm-4"></div>
		</div>
	</div>
	<div class="register-container animated fadeInDown">
    <div class="row">
        <div class="col-sm-2"></div>
        <div class="col-md-8">
			<h3>Register Here to Join CrowdRelief!</h3>
			<p><asp:Label ID="lblMessage" runat="server" text="Survivors and helpers are invited to create a profile and use the various CrowdRelief applications to rebuild their lives."></asp:Label></p>
			<div runat="server" id="divError" visible="false">
				<div class="alert alert-danger">
					<a class="alert-link" href="#">REGISTRATION ERROR!!</a>
					<p>
						<asp:Literal ID="litError" runat="server"></asp:Literal>
					</p>
				</div>
			</div>
            <div class="hpanel">
                <div class="panel-body">
                        <form action="#" id="loginForm">
							<div class="row">
								<div class="form-group col-lg-12">
									<label>Choose a Disaster</label>
									<div id="div1" class="dropdown m-b-md" runat="server">
										<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Disaster <i class="fa fa-sort-down"></i></button>
										<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
											<%=disasterDropDown%>
										</ul>
									</div>
									<input type="hidden" id="hidEventId" runat="server" />
								</div>
								<div class="form-group col-lg-12">
									<label>Choose One</label>
									<br />Are you a survivor or a helper/volunteer?
									<div class="radio m-l-sm radio-primary">
										<asp:RadioButton type="radio" id="rdMemberTypeSurvivor" runat="server" value="survivor" Font-Size="Larger" Text="Survivor" GroupName="memberType" Checked="true" />
									</div>
									<div class="radio m-l-sm radio-primary">
										<asp:RadioButton type="radio" id="rdMemberTypeHelper" runat="server" value="helper" Font-Size="Larger" Text="Helper" GroupName="memberType" />
									</div>
									<div class="radio m-l-sm radio-primary">
										<asp:RadioButton type="radio" id="rdMemberTypeNonProfit" runat="server" value="nonprofit" Font-Size="Larger" Text="NonProfit" GroupName="memberType" />
									</div>
									<div class="radio m-l-sm radio-primary">
										<asp:RadioButton type="radio" id="rdMemberTypeBusiness" runat="server" value="business" Font-Size="Larger" Text="Business" GroupName="memberType" />
									</div>
								</div>
								<div class="form-group col-lg-12">
									<label>Firstname</label>
									<asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" required="" placeholder="First Name"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>Lastname</label>
									<asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" required="" placeholder="Last Name"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>Email Address</label>
									<asp:TextBox type="email" ID="txtEmail" runat="server" CssClass="form-control" required="" placeholder="Email"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>Phone Number</label>
									<asp:TextBox ID="txtPhoneNumber" onkeypress="return isNumberKey(event)" MaxLength="10" TextMode="Phone" runat="server" CssClass="form-control" required="" placeholder="Phone Number"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>Username</label>
									<asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="" placeholder="Username"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>Password</label>
									<asp:TextBox type="password" ID="txtPassword" runat="server" CssClass="form-control" required="" placeholder="Password"></asp:TextBox>
								</div>
                            </div>
                            <div class="text-center">
                                <asp:Button CssClass="btn btn-success w-lg" runat="server" id="btnSubmit" OnClick="btnSubmit_Click" Text="Register"/>
								<div class="text-muted text-center"><small>Already have an account?</small></div>
								<a class="btn btn-sm btn-default w-lg" href="/SignIn">Login</a>
                            </div>
                        </form>
						<div class="row m-b-lg m-t-lg">
							<div class="col-md-12 text-center">

								CrowdRelief was created in 2016 by the Cajun Navy Foundation to help those affected by disasters recovery more quickly.
								<br />
								<br/> 2019 Copyright CrowdRelief
							</div>
						</div>
                </div>
            </div>
        </div>
        <div class="col-sm-2"></div>
    </div>
</div>
</asp:Content>