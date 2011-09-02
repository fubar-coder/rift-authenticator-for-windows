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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Accounts));
            this.AccountGrid = new System.Windows.Forms.DataGridView();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formattedSerialKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iAccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ApplyAccountChanges = new System.Windows.Forms.Button();
            this.CancelAccountChanges = new System.Windows.Forms.Button();
            this.MoceAccountUp = new System.Windows.Forms.Button();
            this.MoveAccountDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AccountGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iAccountBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountGrid
            // 
            this.AccountGrid.AllowUserToAddRows = false;
            this.AccountGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.AccountGrid, "AccountGrid");
            this.AccountGrid.AutoGenerateColumns = false;
            this.AccountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AccountGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descriptionDataGridViewTextBoxColumn,
            this.deviceIdDataGridViewTextBoxColumn,
            this.formattedSerialKeyDataGridViewTextBoxColumn});
            this.AccountGrid.DataSource = this.iAccountBindingSource;
            this.AccountGrid.MultiSelect = false;
            this.AccountGrid.Name = "AccountGrid";
            this.AccountGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            resources.ApplyResources(this.descriptionDataGridViewTextBoxColumn, "descriptionDataGridViewTextBoxColumn");
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // deviceIdDataGridViewTextBoxColumn
            // 
            this.deviceIdDataGridViewTextBoxColumn.DataPropertyName = "DeviceId";
            resources.ApplyResources(this.deviceIdDataGridViewTextBoxColumn, "deviceIdDataGridViewTextBoxColumn");
            this.deviceIdDataGridViewTextBoxColumn.Name = "deviceIdDataGridViewTextBoxColumn";
            this.deviceIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // formattedSerialKeyDataGridViewTextBoxColumn
            // 
            this.formattedSerialKeyDataGridViewTextBoxColumn.DataPropertyName = "FormattedSerialKey";
            resources.ApplyResources(this.formattedSerialKeyDataGridViewTextBoxColumn, "formattedSerialKeyDataGridViewTextBoxColumn");
            this.formattedSerialKeyDataGridViewTextBoxColumn.Name = "formattedSerialKeyDataGridViewTextBoxColumn";
            this.formattedSerialKeyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // iAccountBindingSource
            // 
            this.iAccountBindingSource.DataSource = typeof(RiftAuthenticator.Library.IAccount);
            // 
            // ApplyAccountChanges
            // 
            resources.ApplyResources(this.ApplyAccountChanges, "ApplyAccountChanges");
            this.ApplyAccountChanges.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ApplyAccountChanges.Name = "ApplyAccountChanges";
            this.ApplyAccountChanges.UseVisualStyleBackColor = true;
            this.ApplyAccountChanges.Click += new System.EventHandler(this.ApplyAccountChanges_Click);
            // 
            // CancelAccountChanges
            // 
            resources.ApplyResources(this.CancelAccountChanges, "CancelAccountChanges");
            this.CancelAccountChanges.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelAccountChanges.Name = "CancelAccountChanges";
            this.CancelAccountChanges.UseVisualStyleBackColor = true;
            // 
            // MoceAccountUp
            // 
            resources.ApplyResources(this.MoceAccountUp, "MoceAccountUp");
            this.MoceAccountUp.Name = "MoceAccountUp";
            this.MoceAccountUp.UseVisualStyleBackColor = true;
            this.MoceAccountUp.Click += new System.EventHandler(this.MoceAccountUp_Click);
            // 
            // MoveAccountDown
            // 
            resources.ApplyResources(this.MoveAccountDown, "MoveAccountDown");
            this.MoveAccountDown.Name = "MoveAccountDown";
            this.MoveAccountDown.UseVisualStyleBackColor = true;
            this.MoveAccountDown.Click += new System.EventHandler(this.MoveAccountDown_Click);
            // 
            // Accounts
            // 
            this.AcceptButton = this.ApplyAccountChanges;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelAccountChanges;
            this.Controls.Add(this.MoveAccountDown);
            this.Controls.Add(this.MoceAccountUp);
            this.Controls.Add(this.CancelAccountChanges);
            this.Controls.Add(this.ApplyAccountChanges);
            this.Controls.Add(this.AccountGrid);
            this.Name = "Accounts";
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