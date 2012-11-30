<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Channel.aspx.cs" Inherits="Code_BaseInfo_Channel" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AspNetPager" Namespace="AspNetPager" TagPrefix="NetPager" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../css/css.css?p=12" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css?l=0" rel="Stylesheet" type="text/css" />
    <script language="JavaScript" type="text/javascript" src="../../JScript/selectdialog.js"></script>
</head>
<body topmargin = 0 leftmargin="0">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel style="LEFT: 0px; POSITION: relative; TOP: 0px" id="pnlMain" runat="server" Width="100%" Height="100%"><asp:Panel id="pnlList" runat="server" Width="100%" Height="401px">
                        <TABLE style="HEIGHT: 30px" borderColor=#111111 cellSpacing=0 cellPadding=0 width="100%" border=0>
                            <TBODY>
                                <TR>
                                    <TD borderColor=#cccc99>
                                        <asp:DropDownList id="ddlField" runat="server">
                                            <asp:ListItem Value="LINECODE">分拣线代码</asp:ListItem>
                                            <asp:ListItem Value="CIGARETTECODE">卷烟代码</asp:ListItem>
                                            <asp:ListItem Value="CIGARETTENAME">卷烟名称</asp:ListItem>
                                         </asp:DropDownList> 
                                         <asp:TextBox id="txtValue" runat="server" Height="20px" CssClass="TextBox"></asp:TextBox> 
                                         <asp:Button id="btnQuery" onclick="btnQuery_Click" runat="server" CssClass="ButtonQuery" Text="查询"></asp:Button> 
                                         <asp:Button id="btnExit" onclick="btnExit_Click" runat="server" CssClass="ButtonExit" Text="退出"></asp:Button> 
                                    </TD>
                                </TR>
                            </TBODY>
                        </TABLE>
                        <asp:Panel id="pnlGrid" runat="server" Width="100%" Height="460px">
                            <asp:GridView id="gvMain" runat="server" Width="100%" OnRowEditing="gvMain_RowEditing" AutoGenerateColumns="False" OnRowDataBound="gvMain_RowDataBound">
                                <Columns>
                                    <asp:CommandField HeaderText="操作" ShowEditButton="True">
                                    <HeaderStyle Width="40px" HorizontalAlign="Center"></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:CommandField>
                                    <asp:BoundField DataField="CHANNELCODE" HeaderText="烟道代码">
                                        <HeaderStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CHANNELNAME" HeaderText="烟道名称">
                                        <HeaderStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LINECODE" HeaderText="分拣线">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CHANNELTYPE" HeaderText="烟道类型">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CIGARETTECODE" HeaderText="卷烟代码">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CIGARETTENAME" HeaderText="卷烟名称" />
                                    <asp:BoundField DataField="LEDGROUP" HeaderText="LED组号">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LEDNO" HeaderText="LED编号">
                                        <HeaderStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="STATUS" HeaderText="状态">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CHANNELID" HeaderText="烟道编号" >
                                        <HeaderStyle Width="70px" />
                                    </asp:BoundField>
                                </Columns>

                                <RowStyle BackColor="White" Height="28px"></RowStyle>

                                <HeaderStyle CssClass="gridheader"></HeaderStyle>

                                <AlternatingRowStyle BackColor="#E8F4FF"></AlternatingRowStyle>
                            </asp:GridView> 
                        </asp:Panel> 
                        <NetPager:AspNetPager id="pager" runat="server" Width="555px" Height="24px" ShowPageIndex="False" AlwaysShow="True" OnPageChanging="pager_PageChanging" ShowInputBox="Never"></NetPager:AspNetPager> 
                    </asp:Panel> <asp:Panel style="LEFT: 0px; POSITION: relative; TOP: 0px" id="pnlEdit" runat="server" Width="100%" Height="131px" Visible="False"><TABLE class="OperationBar"><TBODY><TR><TD style="WIDTH: 47px"><asp:Button id="btnUpdate" onclick="btnUpdate_Click" runat="server" CssClass="ButtonSave" Text="保存"></asp:Button></TD><TD style="WIDTH: 33px"><asp:Button id="btnCanel" onclick="btnCanel_Click" runat="server" CssClass="ButtonBack" Text="返回"></asp:Button></TD><TD style="HEIGHT: 20px"></TD></TR></TBODY></TABLE><TABLE width="100%"><TBODY><TR><TD class="tdTitle">烟道代码:</TD><TD><asp:TextBox id="txtChannelCode" runat="server" CssClass="Textbox" Text="" MaxLength="12"></asp:TextBox></TD><TD style="WIDTH: 72px; HEIGHT: 0px"></TD></TR><TR class="tdTitle"><TD class="tdTitle">卷烟代码:</TD><TD><asp:TextBox id="txtCigaretteCode" runat="server" CssClass="TextBox" Text="" MaxLength="10"></asp:TextBox></TD><TD style="WIDTH: 72px; HEIGHT: 20px"><asp:Button id="Button2" runat="server" Text="..." OnClientClick="return SelectDialog2('txtCIGARETTECODE,txtCIGARETTENAME','AS_BI_CIGARETTE','CIGARETTECODE,CIGARETTENAME')" 　CssClass="ButtonBrowse"></asp:Button> <asp:Button id="Button1" runat="server" CssClass="ButtonClear" Text="清空" OnClick="Button1_Click"></asp:Button> </TD><TD></TD></TR><TR><TD class="tdTitle">卷烟名称:</TD><TD><asp:TextBox id="txtCigaretteName" runat="server" CssClass="TextBox" Text="" MaxLength="50"></asp:TextBox></TD><TD style="WIDTH: 72px" height=20></TD></TR><TR><TD class="tdTitle"><FONT color=red>*</FONT>LED组号:</TD><TD><asp:TextBox id="txtLedGroup" runat="server" CssClass="TextBox" Text="" MaxLength="4"></asp:TextBox></TD><TD style="WIDTH: 72px" height=20></TD></TR><TR><TD class="tdTitle"><FONT color=red>*</FONT>LED编号:</TD><TD><asp:TextBox id="txtLedNo" runat="server" CssClass="TextBox" Text="" MaxLength="4"></asp:TextBox></TD><TD style="WIDTH: 72px" height=20></TD></TR><TR><TD class="tdTitle">是否启用:</TD><TD style="WIDTH: 132px" align=left height=20><asp:DropDownList id="ddlStatus" runat="server">
                                        <asp:ListItem Value="1">启用</asp:ListItem>
                                        <asp:ListItem Value="0">未启用</asp:ListItem>
                                        </asp:DropDownList></TD><TD style="WIDTH: 72px" height=20></TD></TR></TBODY></TABLE></asp:Panel> </asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel>
        
    </form>
</body>
</html>
