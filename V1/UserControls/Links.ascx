<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Links.ascx.cs" Inherits="V1_UserControls_Links" %>
					
			
					<div class="grid">
						<asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
							<ItemTemplate>
								<div class="hpanel hblue grid-item">
									<div class="panel-heading hbuilt">
										<asp:Literal ID="litParentCategory" runat="server"></asp:Literal>
									</div>
									<div class="panel-body">
										<asp:Repeater ID="rptArticles" runat="server" OnItemDataBound="repArticles_ItemDataBound">
											<ItemTemplate>
												<h4><asp:Label ID="lblText" runat="server"></asp:Label></h4>
												<asp:Label ID="lblDescription" runat="server"></asp:Label>
												<div class="alert alert-info" runat="server" id="divDeleteArticle" visible="false">
													<i class="fa fa-lock"></i> <asp:LinkButton CssClass="deleteLinkArticle" OnClick="lbDeleteArticle_Click" ID="lbDeleteArticle" runat="server" Text="DELETE ARTICLE"></asp:LinkButton>
												</div>
											</ItemTemplate>
										</asp:Repeater>
										<dl>
											<asp:Repeater ID="rptLinks" runat="server" OnItemDataBound="rptLinks_ItemDataBound">
												<ItemTemplate>
													<dt class="m-b-xs">
														<asp:HyperLink CssClass="EventLink" ID="hypLinkText" runat="server" Target="_blank"></asp:HyperLink>
													</dt>
													<dd class="m-b-sm">
														<asp:Label ID="lblDescription" runat="server"></asp:Label>
													</dd>
													<div class="alert alert-info m-b-lg" runat="server" id="divDeleteLinks" visible="false">
														<i class="fa fa-lock"></i> <asp:LinkButton CssClass="deleteLinkArticle" OnClick="lbDeleteLink_Click" ID="lbDeleteLink" runat="server" Text="DELETE LINK"></asp:LinkButton>
													</div>
												</ItemTemplate>
											</asp:Repeater>
										</dl>
									</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>