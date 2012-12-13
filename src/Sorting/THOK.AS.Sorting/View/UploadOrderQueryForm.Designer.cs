namespace THOK.AS.Sorting.View
{
    partial class UploadOrderQueryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.gvMain = new System.Windows.Forms.DataGridView();
            this.ORDER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SORT_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SORTING_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SORT_BEGIN_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SORT_END_TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpUpload = new System.Windows.Forms.DateTimePicker();
            this.lblProcess = new System.Windows.Forms.Label();
            this.btnSet = new System.Windows.Forms.Button();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnSet);
            this.pnlTool.Controls.Add(this.lblProcess);
            this.pnlTool.Controls.Add(this.dtpUpload);
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnUpload);
            this.pnlTool.Size = new System.Drawing.Size(592, 53);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.gvMain);
            this.pnlContent.Size = new System.Drawing.Size(592, 229);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(592, 282);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::THOK.AS.Sorting.Properties.Resources.Exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(106, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 47);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Image = global::THOK.AS.Sorting.Properties.Resources.Chart;
            this.btnUpload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpload.Location = new System.Drawing.Point(52, 2);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(48, 47);
            this.btnUpload.TabIndex = 17;
            this.btnUpload.Text = "上传";
            this.btnUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // gvMain
            // 
            this.gvMain.AllowUserToAddRows = false;
            this.gvMain.AllowUserToDeleteRows = false;
            this.gvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Info;
            this.gvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.gvMain.BackgroundColor = System.Drawing.SystemColors.Info;
            this.gvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ORDER_ID,
            this.SORT_TYPE,
            this.SORTING_CODE,
            this.SORT_BEGIN_TIME,
            this.SORT_END_TIME});
            this.gvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMain.Location = new System.Drawing.Point(0, 0);
            this.gvMain.Name = "gvMain";
            this.gvMain.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvMain.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gvMain.RowHeadersWidth = 20;
            this.gvMain.RowTemplate.Height = 23;
            this.gvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMain.Size = new System.Drawing.Size(592, 229);
            this.gvMain.TabIndex = 4;
            // 
            // ORDER_ID
            // 
            this.ORDER_ID.DataPropertyName = "ORDER_ID";
            this.ORDER_ID.HeaderText = "订单ID";
            this.ORDER_ID.Name = "ORDER_ID";
            this.ORDER_ID.ReadOnly = true;
            // 
            // SORT_TYPE
            // 
            this.SORT_TYPE.DataPropertyName = "SORT_TYPE";
            this.SORT_TYPE.HeaderText = "分拣类型";
            this.SORT_TYPE.Name = "SORT_TYPE";
            this.SORT_TYPE.ReadOnly = true;
            // 
            // SORTING_CODE
            // 
            this.SORTING_CODE.DataPropertyName = "SORTING_CODE";
            this.SORTING_CODE.HeaderText = "分拣代码";
            this.SORTING_CODE.Name = "SORTING_CODE";
            this.SORTING_CODE.ReadOnly = true;
            // 
            // SORT_BEGIN_TIME
            // 
            this.SORT_BEGIN_TIME.DataPropertyName = "SORT_BEGIN_TIME";
            this.SORT_BEGIN_TIME.HeaderText = "开始时间";
            this.SORT_BEGIN_TIME.Name = "SORT_BEGIN_TIME";
            this.SORT_BEGIN_TIME.ReadOnly = true;
            // 
            // SORT_END_TIME
            // 
            this.SORT_END_TIME.DataPropertyName = "SORT_END_TIME";
            this.SORT_END_TIME.HeaderText = "结束时间";
            this.SORT_END_TIME.Name = "SORT_END_TIME";
            this.SORT_END_TIME.ReadOnly = true;
            // 
            // dtpUpload
            // 
            this.dtpUpload.Location = new System.Drawing.Point(161, 25);
            this.dtpUpload.Name = "dtpUpload";
            this.dtpUpload.Size = new System.Drawing.Size(111, 21);
            this.dtpUpload.TabIndex = 19;
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new System.Drawing.Point(277, 33);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(23, 12);
            this.lblProcess.TabIndex = 20;
            this.lblProcess.Text = "0/0";
            this.lblProcess.Visible = false;
            // 
            // btnSet
            // 
            this.btnSet.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSet.Image = global::THOK.AS.Sorting.Properties.Resources.Modify;
            this.btnSet.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSet.Location = new System.Drawing.Point(0, 0);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(48, 51);
            this.btnSet.TabIndex = 21;
            this.btnSet.Text = "设置";
            this.btnSet.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // UploadOrderQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 282);
            this.Name = "UploadOrderQueryForm";
            this.Text = "分拣状态上传";
            this.pnlTool.ResumeLayout(false);
            this.pnlTool.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.DataGridView gvMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORDER_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SORT_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SORTING_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SORT_BEGIN_TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn SORT_END_TIME;
        private System.Windows.Forms.DateTimePicker dtpUpload;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.Button btnSet;
    }
}