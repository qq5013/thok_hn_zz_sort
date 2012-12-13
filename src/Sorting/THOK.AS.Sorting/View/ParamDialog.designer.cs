namespace THOK.AS.Sorting.View
{
    partial class ParamDialog
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
            this.cbParam = new System.Windows.Forms.ComboBox();
            this.lblParam = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbParam
            // 
            this.cbParam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParam.FormattingEnabled = true;
            this.cbParam.Items.AddRange(new object[] {
            "自动",
            "手动"});
            this.cbParam.Location = new System.Drawing.Point(66, 42);
            this.cbParam.Name = "cbParam";
            this.cbParam.Size = new System.Drawing.Size(121, 20);
            this.cbParam.TabIndex = 0;
            // 
            // lblParam
            // 
            this.lblParam.AutoSize = true;
            this.lblParam.Location = new System.Drawing.Point(65, 19);
            this.lblParam.Name = "lblParam";
            this.lblParam.Size = new System.Drawing.Size(77, 12);
            this.lblParam.TabIndex = 1;
            this.lblParam.Text = "选择上传方式";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(50, 82);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(140, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ParamDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 117);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblParam);
            this.Controls.Add(this.cbParam);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParamDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传方式";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbParam;
        private System.Windows.Forms.Label lblParam;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

    }
}