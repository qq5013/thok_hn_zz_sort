﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectDialog2.aspx.cs" Inherits="Common_SelectDialog2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择</title>
    <base target="_self" />
    <link href="../css/css.css" type="text/css" rel="Stylesheet" />
<script type="text/javascript">
function ReturnValue(fieldValue){
   if (window.document.all)//IE判断window.showModalDialog!=null
   {
     window.returnValue=fieldValue;
     window.opener=null;
     window.close();
   }
   else
   {
//    returnValue=fieldValue;
//    window.close();
       var aryValue=new Array();
       aryValue=fieldValue.split('|');
       var aryTarget=new Array();
       aryTarget=document.getElementById('hideTargetControls').value.split(',');
       for(var i=0;i<aryTarget.length;i++)
       {
          var e=parent.opener.document.getElementById(aryTarget[i]);
          if(e!=null)
          {
            e.value=aryValue[i];
          }
       }  
       window.close();      
   }
}
function window_onload()
{
    if(window.document.all)//IE
    {
    //对于IE直接读数据
    //        var txtInput=window.document.getElementById("txtInput");
    //        txtInput.value=window.dialogArguments;
    }
    else//FireFox
    {
    //获取参数
    //window.dialogArguments=window.opener.myArguments;
    //var txtInput=window.document.getElementById("txtInput");
    //txtInput.value=window.dialogArguments;
    }
}

function window_onunload() {
//对于Firefox需要进行返回值的额外逻辑处理
    if(!window.document.all)//FireFox
    {
       window.opener.myAction.returnAction(window.returnValue)
    }
}

</script>       
</head>
<body style="margin:0 0 0 0;" onload="return window_onload()" onunload="return window_onunload()">
    <form id="form1" runat="server">
      <table style="background-color:WhiteSmoke;"><!--search-->
         <tr><td style="vertical-align:top;"></td>
           <td style="vertical-align:top;"><asp:DropDownList ID="ddl_Field" runat="server">
              </asp:DropDownList></td>
           <td>
               <asp:TextBox ID="txtKeywords" runat="server"></asp:TextBox></td>
           <td><asp:Button ID="btnQuery" runat="server" Text="查找" OnClick="btnQuery_Click" /></td>
         </tr>
      </table>
    
    <div style=" padding-left:5px; padding-right:5px; height:340px;overflow:auto;"><!--data-->
     <table style="width:100%;" cellpadding="0" cellspacing="0">
       <tr>
         <td>
         <asp:DataGrid ID="dgResult" runat="server" OnItemDataBound="dgResult_ItemDataBound" Width="99%">
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
        </asp:DataGrid>          
         </td>
       </tr>
     </table>
    </div>
    
    <div style=" width:500px; position:absolute; left:10px; bottom:10px;"><!--分页-->
         <table cellpadding="0" cellspacing="0" style=" width:100%;">
          <tr>
            <td style="vertical-align:top; height:35px; width:550px;">
                <table cellpadding="0" cellspacing="0" style="width:100%">
                 <tr>
                  <td style="padding-left:1em;">第<asp:Label ID="lblCurrentPage" runat="server" Text="Label"></asp:Label>页</td>
                  <td>共<asp:Label ID="lblPageCount" runat="server" Text="Label"></asp:Label>页</td>   
                  <td>共<asp:Label ID="lblTotalCount" runat="server" Text="Label"></asp:Label>条记录</td>
                  <td><asp:LinkButton ID="btnFirst" runat="server" OnClick="btnFirst_Click">首　页</asp:LinkButton></td>
                  <td><asp:LinkButton ID="btnPrevious" runat="server" OnClick="btnPrevious_Click">上一页</asp:LinkButton></td>
                  <td><asp:LinkButton ID="btnNext" runat="server" OnClick="btnNext_Click">下一页</asp:LinkButton></td>
                  <td><asp:LinkButton ID="btnLast" runat="server" OnClick="btnLast_Click">尾　页</asp:LinkButton></td>
                  <td><asp:DropDownList ID="ddl_PageIndex" runat="server"></asp:DropDownList></td>
                  <td><asp:LinkButton ID="btnGo" runat="server" OnClick="btnGo_Click">跳转</asp:LinkButton></td>
                  <td></td>
                  </tr>
               </table>
            </td>
          </tr>
         </table>
      </div>  
      <div>
          <asp:HiddenField ID="hideTargetControls" runat="server" />
      </div> 
    </form> 
</body>
</html>
