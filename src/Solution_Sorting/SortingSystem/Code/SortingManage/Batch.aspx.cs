using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using THOK.AS.Dal;

public partial class Code_SortingManage_Batch : BasePage
{
    private int pageIndex = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        pager.PageSize = PagingSize;

        BatchDal batchDal = new BatchDal();
        string filter = null;
        if (ViewState["Filter"] != null)
            filter = ViewState["Filter"].ToString();
        
        pager.RecordCount = batchDal.GetCount(filter);
        DataTable batchTable = batchDal.GetAll(pageIndex, PagingSize, filter);
        BindTable2GridView(gvMain, batchTable);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        pageIndex = 1;
        ViewState["Filter"] = string.Format("{0} like '%{1}%'", ddlField.SelectedValue, txtValue.Text);
        BindData();
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Exit();
    }

    protected void pager_PageChanging(object src, PageChangingEventArgs e)
    {
        pageIndex = e.NewPageIndex;
        BindData();
    }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[3].Text != "0")
                e.Row.Cells[3].Text = "已上传";
            else
                e.Row.Cells[3].Text = "未上传";

            if (e.Row.Cells[4].Text != "0")
                e.Row.Cells[4].Text = "已优化";
            else
                e.Row.Cells[4].Text = "未优化";
        }
    }


    protected void gvMain_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = gvMain.Rows[e.RowIndex];
        THOK.AS.Schedule.SemiAutoSchedule schedule = new THOK.AS.Schedule.SemiAutoSchedule();
        if (row.Cells[4].Text == "已优化")
        {
            if (row.Cells[3].Text == "未上传")
            {
                schedule.ClearSchedule(row.Cells[1].Text.ToString(), Convert.ToInt32(row.Cells[2].Text));
                JScript.Instance.ShowMessage(UpdatePanel1, "清除数据成功。");
            }
            else
                JScript.Instance.ShowMessage(UpdatePanel1, "不能清除当前批次，原因是数据已上传一号工程。");
        }
        else
            JScript.Instance.ShowMessage(UpdatePanel1, "不能清除当前批次，原因是当前批次未优化。");
        BindData();
    }
}
