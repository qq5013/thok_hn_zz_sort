namespace THOK.AS.Sorting.View
{
    partial class ChannelQueryForm
    {
        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        /// <param name="disposing">���Ӧ�ͷ��й���Դ��Ϊ true������Ϊ false��</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows ������������ɵĴ���

        /// <summary>
        /// �����֧������ķ��� - ��Ҫ
        /// ʹ�ô���༭���޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.bsMain = new System.Windows.Forms.BindingSource(this.components);
            this.btnExit = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.Column7 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column1 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column2 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column3 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column4 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column5 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.Column6 = new DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn();
            this.pnlTool.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnExchange);
            this.pnlTool.Controls.Add(this.btnRefresh);
            this.pnlTool.Size = new System.Drawing.Size(733, 53);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.dgvMain);
            this.pnlContent.Size = new System.Drawing.Size(733, 229);
            // 
            // pnlMain
            // 
            this.pnlMain.Size = new System.Drawing.Size(733, 282);
            // 
            // dgvMain
            // 
            this.dgvMain.AllowUserToAddRows = false;
            this.dgvMain.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.BackgroundColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMain.ColumnHeadersHeight = 20;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgvMain.DataSource = this.bsMain;
            this.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMain.Location = new System.Drawing.Point(0, 0);
            this.dgvMain.MultiSelect = false;
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.ReadOnly = true;
            this.dgvMain.RowHeadersWidth = 30;
            this.dgvMain.RowTemplate.Height = 23;
            this.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.Size = new System.Drawing.Size(733, 229);
            this.dgvMain.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Image = global::THOK.AS.Sorting.Properties.Resources.Exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(113, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 47);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "�˳�";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnExchange
            // 
            this.btnExchange.Image = global::THOK.AS.Sorting.Properties.Resources.Info;
            this.btnExchange.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExchange.Location = new System.Drawing.Point(59, 2);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(48, 47);
            this.btnExchange.TabIndex = 14;
            this.btnExchange.Text = "����";
            this.btnExchange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExchange.UseVisualStyleBackColor = true;
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::THOK.AS.Sorting.Properties.Resources.Chart;
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRefresh.Location = new System.Drawing.Point(5, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(48, 47);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "ˢ��";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "CHANNELCODE";
            this.Column7.FilteringEnabled = false;
            this.Column7.HeaderText = "�̵�����";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.Width = 80;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CHANNELNAME";
            this.Column1.FilteringEnabled = false;
            this.Column1.HeaderText = "�̵�����";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "CHANNELTYPE";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.FilteringEnabled = false;
            this.Column2.HeaderText = "�̵����";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "LINECODE";
            this.Column3.FilteringEnabled = false;
            this.Column3.HeaderText = "������";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "CIGARETTECODE";
            this.Column4.FilteringEnabled = false;
            this.Column4.HeaderText = "���̴���";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "CIGARETTENAME";
            this.Column5.FilteringEnabled = false;
            this.Column5.HeaderText = "��������";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "QUANTITY";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column6.FilteringEnabled = false;
            this.Column6.HeaderText = "����";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 60;
            // 
            // ChannelQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(733, 282);
            this.Name = "ChannelQueryForm";
            this.pnlTool.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.BindingSource bsMain;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column7;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column1;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column2;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column3;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column4;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column5;
        private DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn Column6;
    }
}
