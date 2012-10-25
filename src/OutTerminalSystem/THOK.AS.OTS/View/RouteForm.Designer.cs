namespace THOK.AS.OTS.View
{
    partial class RouteForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlToolBar = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMaster = new System.Windows.Forms.DataGridView();
            this.bsMaster = new System.Windows.Forms.BindingSource(this.components);
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.Column1 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column2 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column3 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.ROUTECODE = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column5 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMaster)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlToolBar
            // 
            this.pnlToolBar.Controls.Add(this.btnPrint);
            this.pnlToolBar.Controls.Add(this.btnExit);
            this.pnlToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolBar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolBar.Name = "pnlToolBar";
            this.pnlToolBar.Size = new System.Drawing.Size(959, 51);
            this.pnlToolBar.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::THOK.AS.OTS.Properties.Resources.Print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.Location = new System.Drawing.Point(6, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(48, 44);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "打印";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::THOK.AS.OTS.Properties.Resources.Exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(58, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 44);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "退出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AllowUserToResizeColumns = false;
            this.dgvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvDetail.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11});
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.Location = new System.Drawing.Point(0, 0);
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowHeadersWidth = 35;
            this.dgvDetail.RowTemplate.Height = 23;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Size = new System.Drawing.Size(959, 259);
            this.dgvDetail.TabIndex = 3;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "ORDERID";
            this.Column6.HeaderText = "订单号";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "ROUTECODE";
            this.Column7.HeaderText = "线路代码";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 120;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "ROUTENAME";
            this.Column8.HeaderText = "线路名称";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 200;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "CUSTOMERCODE";
            this.Column9.HeaderText = "客户代码";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 120;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "CUSTOMERNAME";
            this.Column10.HeaderText = "客户名称";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 250;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "QUANTITY";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column11.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column11.HeaderText = "订单数量";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // dgvMaster
            // 
            this.dgvMaster.AllowUserToAddRows = false;
            this.dgvMaster.AllowUserToDeleteRows = false;
            this.dgvMaster.AllowUserToResizeColumns = false;
            this.dgvMaster.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvMaster.AutoGenerateColumns = false;
            this.dgvMaster.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.ROUTECODE,
            this.Column5,
            this.Column4,
            this.Column12});
            this.dgvMaster.DataSource = this.bsMaster;
            this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMaster.Location = new System.Drawing.Point(0, 0);
            this.dgvMaster.Name = "dgvMaster";
            this.dgvMaster.ReadOnly = true;
            this.dgvMaster.RowHeadersWidth = 35;
            this.dgvMaster.RowTemplate.Height = 23;
            this.dgvMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMaster.Size = new System.Drawing.Size(959, 258);
            this.dgvMaster.TabIndex = 2;
            this.dgvMaster.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMaster_RowEnter);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 51);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.dgvMaster);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.dgvDetail);
            this.scMain.Size = new System.Drawing.Size(959, 521);
            this.scMain.SplitterDistance = 258;
            this.scMain.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "ORDERDATE";
            this.Column1.FilteringEnabled = false;
            this.Column1.HeaderText = "订单日期";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "AREACODE";
            this.Column2.FilteringEnabled = false;
            this.Column2.HeaderText = "区域代码";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.Width = 120;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "AREANAME";
            this.Column3.FilteringEnabled = false;
            this.Column3.HeaderText = "区域名称";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.Width = 200;
            // 
            // ROUTECODE
            // 
            this.ROUTECODE.DataPropertyName = "ROUTECODE";
            this.ROUTECODE.FilteringEnabled = false;
            this.ROUTECODE.HeaderText = "线路代码";
            this.ROUTECODE.Name = "ROUTECODE";
            this.ROUTECODE.ReadOnly = true;
            this.ROUTECODE.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ROUTECODE.Width = 120;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ROUTENAME";
            this.Column5.FilteringEnabled = false;
            this.Column5.HeaderText = "线路名称";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column5.Width = 250;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ORDERCOUNT";
            this.Column4.HeaderText = "客户数";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.DataPropertyName = "QUANTITY";
            this.Column12.HeaderText = "总数量";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // RouteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 572);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.pnlToolBar);
            this.Name = "RouteForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "线路查询";
            this.pnlToolBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMaster)).EndInit();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolBar;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.DataGridView dgvMaster;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.BindingSource bsMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column1;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column2;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column3;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn ROUTECODE;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
    }
}