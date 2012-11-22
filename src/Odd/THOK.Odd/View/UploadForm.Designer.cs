namespace THOK.Odd.View
{
    partial class UploadForm
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
            this.lblDeleteFile = new System.Windows.Forms.Label();
            this.lblSendFile = new System.Windows.Forms.Label();
            this.lblZipFile = new System.Windows.Forms.Label();
            this.lblGenFile = new System.Windows.Forms.Label();
            this.pbGenFile = new System.Windows.Forms.ProgressBar();
            this.pbDeleteFile = new System.Windows.Forms.ProgressBar();
            this.pbSendFile = new System.Windows.Forms.ProgressBar();
            this.pbZipFile = new System.Windows.Forms.ProgressBar();
            this.cbBatch = new System.Windows.Forms.ComboBox();
            this.lblBatch = new System.Windows.Forms.Label();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.dtpOrderDate = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnUpload);
            this.pnlTool.Size = new System.Drawing.Size(661, 47);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.lblDeleteFile);
            this.pnlContent.Controls.Add(this.lblSendFile);
            this.pnlContent.Controls.Add(this.lblZipFile);
            this.pnlContent.Controls.Add(this.lblGenFile);
            this.pnlContent.Controls.Add(this.pbGenFile);
            this.pnlContent.Controls.Add(this.pbDeleteFile);
            this.pnlContent.Controls.Add(this.pbSendFile);
            this.pnlContent.Controls.Add(this.pbZipFile);
            this.pnlContent.Controls.Add(this.cbBatch);
            this.pnlContent.Controls.Add(this.lblBatch);
            this.pnlContent.Controls.Add(this.lblOrderDate);
            this.pnlContent.Controls.Add(this.dtpOrderDate);
            this.pnlContent.Location = new System.Drawing.Point(0, 47);
            this.pnlContent.Size = new System.Drawing.Size(661, 380);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(661, 427);
            this.pnlMain.ParentChanged += new System.EventHandler(this.pnlMain_ParentChanged);
            // 
            // lblDeleteFile
            // 
            this.lblDeleteFile.AutoSize = true;
            this.lblDeleteFile.Location = new System.Drawing.Point(32, 214);
            this.lblDeleteFile.Name = "lblDeleteFile";
            this.lblDeleteFile.Size = new System.Drawing.Size(89, 12);
            this.lblDeleteFile.TabIndex = 23;
            this.lblDeleteFile.Text = "删除过程文件：";
            // 
            // lblSendFile
            // 
            this.lblSendFile.AutoSize = true;
            this.lblSendFile.Location = new System.Drawing.Point(32, 172);
            this.lblSendFile.Name = "lblSendFile";
            this.lblSendFile.Size = new System.Drawing.Size(89, 12);
            this.lblSendFile.TabIndex = 22;
            this.lblSendFile.Text = "发送压缩文件：";
            // 
            // lblZipFile
            // 
            this.lblZipFile.AutoSize = true;
            this.lblZipFile.Location = new System.Drawing.Point(32, 131);
            this.lblZipFile.Name = "lblZipFile";
            this.lblZipFile.Size = new System.Drawing.Size(89, 12);
            this.lblZipFile.TabIndex = 21;
            this.lblZipFile.Text = "生成压缩文件：";
            // 
            // lblGenFile
            // 
            this.lblGenFile.AutoSize = true;
            this.lblGenFile.Location = new System.Drawing.Point(32, 89);
            this.lblGenFile.Name = "lblGenFile";
            this.lblGenFile.Size = new System.Drawing.Size(89, 12);
            this.lblGenFile.TabIndex = 20;
            this.lblGenFile.Text = "生成数据文件：";
            // 
            // pbGenFile
            // 
            this.pbGenFile.Location = new System.Drawing.Point(121, 82);
            this.pbGenFile.Name = "pbGenFile";
            this.pbGenFile.Size = new System.Drawing.Size(232, 23);
            this.pbGenFile.TabIndex = 19;
            // 
            // pbDeleteFile
            // 
            this.pbDeleteFile.Location = new System.Drawing.Point(121, 207);
            this.pbDeleteFile.Name = "pbDeleteFile";
            this.pbDeleteFile.Size = new System.Drawing.Size(232, 23);
            this.pbDeleteFile.TabIndex = 18;
            // 
            // pbSendFile
            // 
            this.pbSendFile.Location = new System.Drawing.Point(121, 166);
            this.pbSendFile.Name = "pbSendFile";
            this.pbSendFile.Size = new System.Drawing.Size(232, 23);
            this.pbSendFile.TabIndex = 17;
            // 
            // pbZipFile
            // 
            this.pbZipFile.Location = new System.Drawing.Point(121, 124);
            this.pbZipFile.Name = "pbZipFile";
            this.pbZipFile.Size = new System.Drawing.Size(232, 23);
            this.pbZipFile.TabIndex = 16;
            // 
            // cbBatch
            // 
            this.cbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBatch.FormattingEnabled = true;
            this.cbBatch.Location = new System.Drawing.Point(276, 40);
            this.cbBatch.Name = "cbBatch";
            this.cbBatch.Size = new System.Drawing.Size(53, 20);
            this.cbBatch.TabIndex = 15;
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Location = new System.Drawing.Point(214, 43);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(65, 12);
            this.lblBatch.TabIndex = 14;
            this.lblBatch.Text = "订单批次：";
            // 
            // lblOrderDate
            // 
            this.lblOrderDate.AutoSize = true;
            this.lblOrderDate.Location = new System.Drawing.Point(32, 44);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(65, 12);
            this.lblOrderDate.TabIndex = 13;
            this.lblOrderDate.Text = "订单日期：";
            // 
            // dtpOrderDate
            // 
            this.dtpOrderDate.Location = new System.Drawing.Point(97, 39);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(111, 21);
            this.dtpOrderDate.TabIndex = 12;
            this.dtpOrderDate.ValueChanged += new System.EventHandler(this.dtpOrderDate_ValueChanged);
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnExit.Image = global::THOK.Odd.Properties.Resources.Exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(48, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 45);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUpload.Image = global::THOK.Odd.Properties.Resources.Right;
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpload.Location = new System.Drawing.Point(0, 0);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(48, 45);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Text = "上传";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // UploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(661, 427);
            this.Name = "UploadForm";
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblDeleteFile;
        private System.Windows.Forms.Label lblSendFile;
        private System.Windows.Forms.Label lblZipFile;
        private System.Windows.Forms.Label lblGenFile;
        private System.Windows.Forms.ProgressBar pbGenFile;
        private System.Windows.Forms.ProgressBar pbDeleteFile;
        private System.Windows.Forms.ProgressBar pbSendFile;
        private System.Windows.Forms.ProgressBar pbZipFile;
        private System.Windows.Forms.ComboBox cbBatch;
        private System.Windows.Forms.Label lblBatch;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.DateTimePicker dtpOrderDate;
        private System.Windows.Forms.Button btnExit;
    }
}
