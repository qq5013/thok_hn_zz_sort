﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="Code_SysInformation_UserManage_UserList" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AspNetPager" Namespace="AspNetPager" TagPrefix="NetPager" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>用户信息</title>
    <script type="text/javascript" src="../../JScript/setday9.js"></script>
    <script type="text/javascript" src="../../JScript/Check.js?t=00"></script>
    <link href="../../css/css.css?t=98" rel="Stylesheet" type="text/css" />
    <link href="../../css/op.css" rel="Stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true">
          <ContentTemplate>

          <!--数据显示-->
          <asp:Panel ID="pnlList" runat="server" Height="480px" Width="100%">
             <!--工具栏-->
             <asp:Panel ID="pnlListToolbar" runat="server"  Height="30px" Style="position: relative" Width="100%">
                  <table style="width:100%; height:20px;">
                     <tr>
                       <td style="height: 22px">
                           <asp:DropDownList ID="ddl_Field" runat="server">
                               <asp:ListItem Selected="True" Value="UserName">用户帐号</asp:ListItem>
                               <asp:ListItem Value="EmployeeName">用户姓名</asp:ListItem>
                               <asp:ListItem Value="Memo">备注</asp:ListItem>
                           </asp:DropDownList>
                           <asp:TextBox ID="txtKeyWords" runat="server" CssClass="TextBox"></asp:TextBox><asp:RadioButton GroupName="order" ID="rbASC" runat="server" Text="升" Checked="True" />
                           <asp:RadioButton GroupName="order" ID="rbDESC" runat="server" Text="降" />
                           <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" CssClass="ButtonQuery"/>
                           <asp:Button ID="btnCreate" runat="server" Text="新增"　CssClass="ButtonCreate" OnClick="btnCreate_Click" Enabled="False"/>
                           <asp:Button ID="btnDelete" runat="server" Text="删除"　CssClass="ButtonDel" OnClick="btnDelete_Click" OnClientClick="return DelConfirm('btnDelete')" Enabled="False"/>
                           <asp:Button ID="btnExit" runat="server" Text="退出"  OnClick="btnExit_Click" CssClass="ButtonExit" />
                        </td>
                     </tr>
                  </table>
             </asp:Panel>
             <!--数据-->
              <asp:Panel ID="pnlMain" runat="server" Height="480px" Style="position: relative; overflow-x:hidden; overflow-y:auto;" Width="100%">
                  <asp:GridView ID="gvMain" runat="server" Style="position: relative;table-layout:fixed;width:100%;"
                     OnRowEditing="gvMain_RowEditing" OnRowDataBound="gvMain_RowDataBound" CssClass="GridStyle" CellPadding="3" CellSpacing="1" BorderWidth="0px" AutoGenerateColumns="False">
                      <RowStyle BackColor="WhiteSmoke" Height="28px" />
                      <HeaderStyle CssClass="GridHeader" />
                      <AlternatingRowStyle BackColor="White" />
                      <Columns>
                          <asp:TemplateField HeaderText="操作">
                              <HeaderStyle Width="60px" />
                          </asp:TemplateField>
                          <asp:BoundField DataField="UserName" HeaderText="用户帐号" >
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          <asp:BoundField DataField="EmployeeName" HeaderText="用户姓名" >
                              <HeaderStyle Width="100px" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Memo" HeaderText="备注" >
                          </asp:BoundField>
                      </Columns>
                  </asp:GridView>
              </asp:Panel>
              <!--分页导航-->
              <asp:Panel ID="pnlNavigator" runat="server" Height="31px" Style="left: 0px; position: relative; top: 0px" Width="100%">
                 <table id="paging" cellpadding="0" cellspacing="0" style="width:500px;">
                   <tr>
                     <td>
                       <NetPager:AspNetPager ID="pager" runat="server" OnPageChanging="pager_PageChanging" ShowPageIndex="false" ShowInputBox="Never" AlwaysShow="true"></NetPager:AspNetPager>
                     </td>
                   </tr>
                  </table>  
               </asp:Panel>
          </asp:Panel>  
           
          <!--编辑-->
          <asp:Panel ID="pnlEdit" runat="server" Height="480px" Width="100%" Visible="false">
               <table class="OperationBar">
                  <tr>
                    <td>
                    <asp:Button ID="btnSave" Text=" 保 存" runat="server" OnClick="btnSave_Click" CssClass="ButtonSave" OnClientClick="return CheckBeforeSubmit()" />
                    <asp:Button ID="btnCancel" Text="取 消" runat="server" CssClass="ButtonCancel" OnClick="btnCancel_Click" />            
                    </td>
                  </tr>
               </table>
               <table>
                  <tr>
                     <td class="tdTitle"><font color="red">*</font>用户帐号</td>
                     <td><asp:TextBox ID="txtUserName" runat="server" CssClass="TextBox"></asp:TextBox>
                         <asp:TextBox ID="txtUserID" runat="server" Width="0" Height="0" BorderWidth="0"></asp:TextBox>
                     </td>
                  </tr>
                  <tr>
                     <td class="tdTitle">用户姓名</td> 
                     <td><asp:TextBox ID="txtEmployeeName" runat="server"  onfocus="CannotEdit(this)" CssClass="DisabledTextBox" Enabled="False"></asp:TextBox>
                         <asp:TextBox ID="txtEmployeeCode" runat="server" Width="0" BorderWidth="0" Height="0"></asp:TextBox>
                        <input type=button value="..." class="ButtonBrowse" onclick="SelectDialog2('txtEmployeeCode,txtEmployeeName','AS_BI_EMPLOYEE','EMPLOYEECODE,EMPLOYEENAME');"/>
                        <input type="button" value="清空" class="ButtonClear" onclick="ClearName()"/>
                     </td>
                  </tr>
                  <tr>
                     <td class="tdTitle">备注</td>
                     <td><asp:TextBox ID="txtMemo" runat="server"  CssClass="MultiLineTextBox" TextMode="MultiLine"></asp:TextBox></td>
                  </tr>
               </table>
          </asp:Panel>  
           </ContentTemplate>
     </asp:UpdatePanel>
        <!--隐藏数据-->  
        <div>
           <asp:HiddenField ID="hdnXGQX" Value="0" runat="server" />
           <asp:HiddenField ID="hdnOpFlag" Value="0" runat="server" />
        </div>
    </form>
<script>

//删除确认
function DelConfirm(btnID)
{
     var table=document.getElementById('gvMain');
     var hasChecked=false;
     for(var i=1;i<table.rows.length;i++)
     {
        var cell=table.rows[i].cells[0];
        var chk=cell.getElementsByTagName("INPUT");
        if(chk[0].checked)
        {
            hasChecked=true;
            break;
        }
     }
     
     if(!hasChecked)
     {
        alert('请选择要删除的数据！');
        return false;
     }
      if(confirm('确定要删除选择的数据？','删除提示'))
      {
         var btn=document.getElementById(btnID);
         btn.click();
         //window.location.reload();
      }
      else
      {
         return false;
      }
}


function CheckBeforeSubmit()
{
    var username=document.getElementById('txtUserName').value.trim();
    if(username=="")
    {
       alert('用户名不能为空！');
       return false;
    }
}

function SelectDialog2(strTarget,strTableName,strReturnField)
{
   var date=new Date();
   var time=date.getMilliseconds();
   var aryTarget=strTarget.split(',');
   var aryField=strReturnField.split(',');
   if(aryTarget.length!=aryField.length)
   {
      alert('参数有错！');
      return false;
   }
   
   if (window.document.all)//IE判断window.showModalDialog!=null
   {
      var returnvalue;
      returnvalue=window.showModalDialog("../../Common/SelectDialog2.aspx?TableName="+strTableName+"&ReturnField="+strReturnField+"&time="+time,"",
                                         "top=0;left=0;toolbar=no;menubar=no;scrollbars=no;resizable=no;location=no;status=no;dialogWidth=680px;dialogHeight=450px");
   
       if(returnvalue==null)
       {
          return false;
       }
       else if(returnvalue!='')
       {
           var aryValue=new Array();
           aryValue=returnvalue.split('|');
           for(var i=0;i<aryTarget.length;i++)
           {
              var e=document.getElementById(aryTarget[i]);
              if(e!=null)
              {
                e.value=aryValue[i];
              }
           }  
           return false; 
       } 
   }
   else
   {
        //参数
        var strPara = "height=450px;width=500px;help=off;resizable=off;scroll=no;status=off;modal=yes;dialog=yes";
        //打开窗口
        var url="../../../Common/SelectDialog2.aspx?TableName="+strTableName+"&ReturnField="+strReturnField+"&time="+time+"&targetControls="+strTarget;
        var DialogWin = window.open(url,"myOpen",strPara,true);
   } 
                                   
}

function ClearName()
{
  document.getElementById('txtEmployeeCode').value="";
  document.getElementById('txtEmployeeName').value="";
}

</script>
</body>
</html>
