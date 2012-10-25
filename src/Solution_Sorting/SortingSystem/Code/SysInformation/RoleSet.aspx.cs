﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using THOK.AS.Dal;

public partial class Code_SysInfomation_RoleManage_RoleSet : BasePage
{
    SysGroupDal groupDal = new SysGroupDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitSmartTree();
            if (Request.QueryString["GroupID"] != null)
            {
                ViewState["GroupID"] = Request.QueryString["GroupID"].ToString();
                string GroupName = Request.QueryString["GroupName"].ToString();
                this.lbTitle.Text = "用户组<font color='Gray'>" + GroupName + "</font>权限设置";
                GroupOperationBind();
            }
        }
        if (ViewState["GroupID"] != null)
        {
            //JScript.Instance.RegisterScript(this, "document.getElementById('divOp').style.display='block';");
            this.lnkBtnSave.Visible = true;
        }
    }

    #region SmartTreeView

    public void InitSmartTree()
    {
        this.sTreeModule.Nodes.Clear();
        DataTable dtModules = groupDal.GetSystemModules().Tables[0];
        DataTable dtSubModules = groupDal.GetSystemSubModules().Tables[0];
        DataTable dtOperations = groupDal.GetSystemOperations().Tables[0];
        foreach (DataRow dr in dtModules.Rows)
        {
            TreeNode tnRoot = new TreeNode(dr["MenuTitle"].ToString(), dr["ModuleCode"].ToString().Trim());
            tnRoot.SelectAction = TreeNodeSelectAction.Expand;
            tnRoot.ShowCheckBox = true;
            this.sTreeModule.Nodes.Add(tnRoot);
        }
        //为第一级菜单增加子级菜单


        if (dtModules.Rows.Count > 0)
        {
            foreach (DataRow drSub in dtSubModules.Rows)
            {
                for (int i = 0; i < sTreeModule.Nodes.Count; i++)
                {
                    if (sTreeModule.Nodes[i].Value == drSub["ModuleCode"].ToString().Trim())
                    {
                        TreeNode tnChild = new TreeNode(drSub["MenuTitle"].ToString(), drSub["SubModuleCode"].ToString().Trim());
                        tnChild.ShowCheckBox = true;
                        tnChild.SelectAction = TreeNodeSelectAction.Expand;
                        this.sTreeModule.Nodes[i].ChildNodes.Add(tnChild);
                        break;
                    }

                }
            }
        }

        foreach (DataRow drOp in dtOperations.Rows)
        {
            for (int i = 0; i < sTreeModule.Nodes.Count; i++)
            {
                for (int j = 0; j < sTreeModule.Nodes[i].ChildNodes.Count; j++)
                {
                    if (sTreeModule.Nodes[i].ChildNodes[j].Value == drOp["SubModuleCode"].ToString().Trim())
                    {
                        TreeNode tnOp = new TreeNode(drOp["OperatorDescription"].ToString(), drOp["ModuleID"].ToString());
                        tnOp.ShowCheckBox = true;
                        tnOp.SelectAction = TreeNodeSelectAction.None;
                        sTreeModule.Nodes[i].ChildNodes[j].ChildNodes.Add(tnOp);
                    }
                }
            }
        }

    }
    #endregion


    protected void lnkBtnSave_Click(object sender, EventArgs e)
    {
        int GroupID = Convert.ToInt32(ViewState["GroupID"]);
        int iGroupID = int.Parse(ViewState["GroupID"].ToString());
        try
        {
            groupDal.DeleteGroupOperation(GroupID);
            foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
            {
                foreach (TreeNode tnSub in tnRoot.ChildNodes)
                {
                    foreach (TreeNode tnOp in tnSub.ChildNodes)
                    {
                        if (tnOp.Checked)
                        {
                            groupDal.InsertGroupOperation(GroupID, tnOp.Value);
                        }
                    }
                }
            }
            JScript.Instance.ShowMessage(this, "保存成功！");
            GroupOperationBind();
        }
        catch
        {
            JScript.Instance.ShowMessage(this, "保存失败，请与管理员联系。");
        }
        
    }

    public void GroupOperationBind()
    {
        DataTable dtOP = groupDal.GetGroupOperation(Convert.ToInt32(ViewState["GroupID"])).Tables[0];
        if (dtOP.Rows.Count > 0)
        {
            foreach (DataRow dr in dtOP.Rows)
            {
                foreach (TreeNode tnRoot in this.sTreeModule.Nodes)
                {
                    bool IsAllSelected = true;
                    foreach (TreeNode tnSub in tnRoot.ChildNodes)
                    {
                        bool IsSubAllSelected = true;
                        foreach (TreeNode tnOp in tnSub.ChildNodes)
                        {
                            if (tnOp.Value == dr["ModuleID"].ToString())
                            {
                                tnOp.Checked = true;
                                break;
                            }
                            if (!tnOp.Checked)
                            {
                                IsSubAllSelected = false;
                            }
                        }
                        if (IsSubAllSelected)
                        {
                            tnSub.Checked = true;
                        }
                        else
                        {
                            IsAllSelected = false;
                        }
                    }
                    if (IsAllSelected)
                    {
                        tnRoot.Checked = true;
                    }
                }
            }

        }
    }

    protected void lnkBtnCollapse_Click(object sender, EventArgs e)
    {
        //this.tvModuleList.CollapseAll();
        this.sTreeModule.CollapseAll();
    }
    protected void lnkBtnExpand_Click(object sender, EventArgs e)
    {
        //this.tvModuleList.ExpandAll();
        this.sTreeModule.ExpandAll();
    }

    protected void tvModuleList_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        //Response.Write(e.Node.Value);
    }
    protected void tvModuleList_TreeNodeCheckChanged1(object sender, TreeNodeEventArgs e)
    {
        //JScript.Instance.ShowMessage(this, e.Node.Value);
    }
    protected void tvModuleList_SelectedNodeChanged(object sender, EventArgs e)
    {
        //JScript.Instance.ShowMessage(this, "hi");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //JScript.Instance.ShowMessage(this, tvModuleList.Nodes[0].ChildNodes[0].ChildNodes[0].ValuePath);
        
    }
}
