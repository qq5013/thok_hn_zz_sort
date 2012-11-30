namespace THOK.Odd.View
{
    partial class DownloadForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.dtpOrderDate = new System.Windows.Forms.DateTimePicker();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.lblBatch = new System.Windows.Forms.Label();
            this.cbBatch = new System.Windows.Forms.ComboBox();
            this.pbClear = new System.Windows.Forms.ProgressBar();
            this.pbOrder = new System.Windows.Forms.ProgressBar();
            this.pbRoute = new System.Windows.Forms.ProgressBar();
            this.pbCigarette = new System.Windows.Forms.ProgressBar();
            this.lblCigarette = new System.Windows.Forms.Label();
            this.lblClear = new System.Windows.Forms.Label();
            this.lblOrder = new System.Windows.Forms.Label();
            this.lblRoute = new System.Windows.Forms.Label();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnClear);
            this.pnlTool.Controls.Add(this.btnDownload);
            this.pnlTool.Size = new System.Drawing.Size(633, 47);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.lblRoute);
            this.pnlContent.Controls.Add(this.lblOrder);
            this.pnlContent.Controls.Add(this.lblClear);
            this.pnlContent.Controls.Add(this.lblCigarette);
            this.pnlContent.Controls.Add(this.pbCigarette);
            this.pnlContent.Controls.Add(this.pbRoute);
            this.pnlContent.Controls.Add(this.pbOrder);
            this.pnlContent.Controls.Add(this.pbClear);
            this.pnlContent.Controls.Add(this.cbBatch);
            this.pnlContent.Controls.Add(this.lblBatch);
            this.pnlContent.Controls.Add(this.lblOrderDate);
            this.pnlContent.Controls.Add(this.dtpOrderDate);
            this.pnlContent.Location = new System.Drawing.Point(0, 47);
            this.pnlContent.Size = new System.Drawing.Size(633, 390);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(633, 437);
            this.pnlMain.ParentChanged += new System.EventHandler(this.pnlMain_ParentChanged);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExit.Image = global::THOK.Odd.Properties.Resources.Exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(96, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 45);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClear.Image = global::THOK.Odd.Properties.Resources.Delete;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClear.Location = new System.Drawing.Point(48, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(48, 45);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清除";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDownload.Image = global::THOK.Odd.Properties.Resources.Right;
            this.btnDownload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDownload.Location = new System.Drawing.Point(0, 0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(48, 45);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.Text = "下载";
            this.btnDownload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // dtpOrderDate
            // 
            this.dtpOrderDate.Location = new System.Drawing.Point(97, 39);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(111, 21);
            this.dtpOrderDate.TabIndex = 0;
            this.dtpOrderDate.ValueChanged += new System.EventHandler(this.dtpOrderDate_ValueChanged);
            // 
            // lblOrderDate
            // 
            this.lblOrderDate.AutoSize = true;
            this.lblOrderDate.Location = new System.Drawing.Point(32, 44);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(65, 12);
            this.lblOrderDate.TabIndex = 1;
            this.lblOrderDate.Text = "订单日期：";
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Location = new System.Drawing.Point(214, 43);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(65, 12);
            this.lblBatch.TabIndex = 2;
            this.lblBatch.Text = "订单批次：";
            // 
            // cbBatch
            // 
            this.cbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBatch.FormattingEnabled = true;
            this.cbBatch.Location = new System.Drawing.Point(276, 40);
            this.cbBatch.Name = "cbBatch";
            this.cbBatch.Size = new System.Drawing.Size(53, 20);
            this.cbBatch.TabIndex = 3;
            // 
            // pbClear
            // 
            this.pbClear.Location = new System.Drawing.Point(97, 124);
            this.pbClear.Name = "pbClear";
            this.pbClear.Size = new System.Drawing.Size(232, 23);
            this.pbClear.TabIndex = 4;
            // 
            // pbOrder
            // 
            this.pbOrder.Location = new System.Drawing.Point(97, 166);
            this.pbOrder.Name = "pbOrder";
            this.pbOrder.Size = new System.Drawing.Size(232, 23);
            this.pbOrder.TabIndex = 5;
            // 
            // pbRoute
            // 
            this.pbRoute.Location = new System.Drawing.Point(97, 207);
            this.pbRoute.Name = "pbRoute";
            this.pbRoute.Size = new System.Drawing.Size(232, 23);
            this.pbRoute.TabIndex = 6;
            // 
            // pbCigarette
            // 
            this.pbCigarette.Location = new System.Drawing.Point(97, 82);
            this.pbCigarette.Name = "pbCigarette";
            this.pbCigarette.Size = new System.Drawing.Size(232, 23);
            this.pbCigarette.TabIndex = 7;
            // 
            // lblCigarette
            // 
            this.lblCigarette.AutoSize = true;
            this.lblCigarette.Location = new System.Drawing.Point(32, 89);
            this.lblCigarette.Name = "lblCigarette";
            this.lblCigarette.Size = new System.Drawing.Size(65, 12);
            this.lblCigarette.TabIndex = 8;
            this.lblCigarette.Text = "下载卷烟：";
            // 
            // lblClear
            // 
            this.lblClear.AutoSize = true;
            this.lblClear.Location = new System.Drawing.Point(32, 131);
            this.lblClear.Name = "lblClear";
            this.lblClear.Size = new System.Drawing.Size(65, 12);
            this.lblClear.TabIndex = 9;
            this.lblClear.Text = "清空数据：";
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new System.Drawing.Point(32, 172);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(65, 12);
            this.lblOrder.TabIndex = 10;
            this.lblOrder.Text = "下载订单：";
            // 
            // lblRoute
            // 
            this.lblRoute.AutoSize = true;
            this.lblRoute.Location = new System.Drawing.Point(32, 214);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new System.Drawing.Size(65, 12);
            this.lblRoute.TabIndex = 11;
            this.lblRoute.Text = "下载线路：";
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(633, 437);
            this.Name = "DownloadForm";
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ProgressBar pbClear;
        private System.Windows.Forms.ComboBox cbBatch;
        private System.Windows.Forms.Label lblBatch;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.DateTimePicker dtpOrderDate;
        private System.Windows.Forms.ProgressBar pbOrder;
        private System.Windows.Forms.ProgressBar pbRoute;
        private System.Windows.Forms.ProgressBar pbCigarette;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.Label lblClear;
        private System.Windows.Forms.Label lblCigarette;
    }
}
