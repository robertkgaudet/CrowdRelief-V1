<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Basic.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="V1_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="middle-box text-center loginscreen animated fadeInDown">
        <div>
            <div>
                <h1 class="logo-name">
					<img class="img-responsive" src="/V1/Images/Logo-Horizontal.png" />
                </h1>
				<div class="p-sm text-center">
					Welcome to CrowdRelief<br /> The Official Disaster Recovery Platform of the 
					<b><a href="http://www.CajunRelief.org" target="_blank"><img class="right" src="/S1/Images/CajunNavySmallLogo.png" /> Cajun Navy</a></b>
				</div>
            </div>
            <p>Survivors and helpers conveniently keep friends and family updated with your recovery.
            </p>
            <p>Sign in now to get started.</p>
            <div class="m-t" role="form">
                <div class="form-group">
					<asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Username" required=""></asp:TextBox>
                </div>
                <div class="form-group">
					<asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Password" required=""></asp:TextBox>
                </div>
				<asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Login" CssClass="btn btn-success block full-width m-b" />

                <a href="/V1/PasswordReset.aspx"><small>Forgot password?</small></a>
                <p class="text-muted text-center"><small>Do not have an account?</small></p>
                <a class="btn btn-sm btn-white btn-block" href="/Register">Create an Account</a>
            </div>
            <p class="m-t">
				<small>CrowdRelief was created in 2016 by the Cajun Navy Foundation to help those affected by disasters recovery more quickly.</small>
            </p>
        </div>
    </div>
</asp:Content>