namespace RiftAuthenticator.WinForms
{
    partial class Accounts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AccountGrid = new System.Windows.Forms.DataGridView();
            this.ApplyAccountChanges = new System.Windows.Forms.Button();
            this.CancelAccountChanges = new System.Windows.Forms.Button();
            this.MoceAccountUp = new System.Windows.Forms.Button();
            this.MoveAccountDown = new System.Windows.Forms.Button();
            this.iAccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formattedSerialKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.AccountGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iAccountBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountGrid
            // 
            this.AccountGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountGrid.AutoGenerateColumns = false;
            this.AccountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AccountGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descriptionDataGridViewTextBoxColumn,
            this.deviceIdDataGridViewTextBoxColumn,
            this.formattedSerialKeyDataGridViewTextBoxColumn});
            this.AccountGrid.DataSource = this.iAccountBindingSource;
            this.AccountGrid.Location = new System.Drawing.Point(12, 12);
            this.AccountGrid.Name = "AccountGrid";
            this.AccountGrid.Size = new System.Drawing.Size(357, 170);
            this.AccountGrid.TabIndex = 0;
            // 
            // ApplyAccountChanges
            // 
            this.ApplyAccountChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ApplyAccountChanges.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ApplyAccountChanges.Location = new System.Drawing.Point(12, 188);
            this.ApplyAccountChanges.Name = "ApplyAccountChanges";
            this.ApplyAccountChanges.Size = new System.Drawing.Size(75, 23);
            this.ApplyAccountChanges.TabIndex = 1;
            this.ApplyAccountChanges.Text = "OK";
            this.ApplyAccountChanges.UseVisualStyleBackColor = true;
            // 
            // CancelAccountChanges
            // 
            this.CancelAccountChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelAccountChanges.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelAccountChanges.Location = new System.Drawing.Point(329, 188);
            this.CancelAccountChanges.Name = "CancelAccountChanges";
            this.CancelAccountChanges.Size = new System.Drawing.Size(75, 23);
            this.CancelAccountChanges.TabIndex = 2;
            this.CancelAccountChanges.Text = "Cancel";
            this.CancelAccountChanges.UseVisualStyleBackColor = true;
            // 
            // MoceAccountUp
            // 
            this.MoceAccountUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MoceAccountUp.Location = new System.Drawing.Point(375, 12);
            this.MoceAccountUp.Name = "MoceAccountUp";
            this.MoceAccountUp.Size = new System.Drawing.Size(29, 25);
            this.MoceAccountUp.TabIndex = 3;
            this.MoceAccountUp.Text = "↑";
            this.MoceAccountUp.UseVisualStyleBackColor = true;
            // 
            // MoveAccountDown
            // 
            this.MoveAccountDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveAccountDown.Location = new System.Drawing.Point(375, 157);
            this.MoveAccountDown.Name = "MoveAccountDown";
            this.MoveAccountDown.Size = new System.Drawing.Size(29, 25);
            this.MoveAccountDown.TabIndex = 4;
            this.MoveAccountDown.Text = "↓";
            this.MoveAccountDown.UseVisualStyleBackColor = true;
            // 
            // iAccountBindingSource
            // 
            this.iAccountBindingSource.DataSource = typeof(RiftAuthenticator.Library.IAccount);
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // deviceIdDataGridViewTextBoxColumn
            // 
            this.deviceIdDataGridViewTextBoxColumn.DataPropertyName = "DeviceId";
            this.deviceIdDataGridViewTextBoxColumn.HeaderText = "Device ID";
            this.deviceIdDataGridViewTextBoxColumn.Name = "deviceIdDataGridViewTextBoxColumn";
            this.deviceIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // formattedSerialKeyDataGridViewTextBoxColumn
            // 
            this.formattedSerialKeyDataGridViewTextBoxColumn.DataPropertyName = "FormattedSerialKey";
            this.formattedSerialKeyDataGridViewTextBoxColumn.HeaderText = "Serial Key";
            this.formattedSerialKeyDataGridViewTextBoxColumn.Name = "formattedSerialKeyDataGridViewTextBoxColumn";
            this.formattedSerialKeyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Accounts
            // 
            this.AcceptButton = this.ApplyAccountChanges;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelAccountChanges;
            this.ClientSize = new System.Drawing.Size(416, 223);
            this.Controls.Add(this.MoveAccountDown);
            this.Controls.Add(this.MoceAccountUp);
            this.Controls.Add(this.CancelAccountChanges);
            this.Controls.Add(this.ApplyAccountChanges);
            this.Controls.Add(this.AccountGrid);
            this.Name = "Accounts";
            this.Text = "Accounts";
            ((System.ComponentModel.ISupportInitialize)(this.AccountGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iAccountBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView AccountGrid;
        private System.Windows.Forms.BindingSource iAccountBindingSource;
        private System.Windows.Forms.Button ApplyAccountChanges;
        private System.Windows.Forms.Button CancelAccountChanges;
        private System.Windows.Forms.Button MoceAccountUp;
        private System.Windows.Forms.Button MoveAccountDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn formattedSerialKeyDataGridViewTextBoxColumn;
    }
}