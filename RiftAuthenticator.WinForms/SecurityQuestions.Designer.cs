namespace RiftAuthenticator.WinForms
{
    partial class SecurityQuestions
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.SecurityQuestion1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SecurityAnswer1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SecurityAnswer2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SecurityQuestion2 = new System.Windows.Forms.TextBox();
            this.CancelDialog = new System.Windows.Forms.Button();
            this.DoRecovery = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SecurityQuestion1
            // 
            this.SecurityQuestion1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecurityQuestion1.Location = new System.Drawing.Point(118, 12);
            this.SecurityQuestion1.Name = "SecurityQuestion1";
            this.SecurityQuestion1.ReadOnly = true;
            this.SecurityQuestion1.Size = new System.Drawing.Size(452, 20);
            this.SecurityQuestion1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Security question 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Answer 1:";
            // 
            // SecurityAnswer1
            // 
            this.SecurityAnswer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecurityAnswer1.Location = new System.Drawing.Point(118, 38);
            this.SecurityAnswer1.Name = "SecurityAnswer1";
            this.SecurityAnswer1.Size = new System.Drawing.Size(452, 20);
            this.SecurityAnswer1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Answer 2:";
            // 
            // SecurityAnswer2
            // 
            this.SecurityAnswer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecurityAnswer2.Location = new System.Drawing.Point(118, 90);
            this.SecurityAnswer2.Name = "SecurityAnswer2";
            this.SecurityAnswer2.Size = new System.Drawing.Size(452, 20);
            this.SecurityAnswer2.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Security question 2:";
            // 
            // SecurityQuestion2
            // 
            this.SecurityQuestion2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecurityQuestion2.Location = new System.Drawing.Point(118, 64);
            this.SecurityQuestion2.Name = "SecurityQuestion2";
            this.SecurityQuestion2.ReadOnly = true;
            this.SecurityQuestion2.Size = new System.Drawing.Size(452, 20);
            this.SecurityQuestion2.TabIndex = 4;
            // 
            // CancelDialog
            // 
            this.CancelDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelDialog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialog.Location = new System.Drawing.Point(495, 133);
            this.CancelDialog.Name = "CancelDialog";
            this.CancelDialog.Size = new System.Drawing.Size(75, 23);
            this.CancelDialog.TabIndex = 9;
            this.CancelDialog.Text = "Cancel";
            this.CancelDialog.UseVisualStyleBackColor = true;
            // 
            // DoRecovery
            // 
            this.DoRecovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoRecovery.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DoRecovery.Location = new System.Drawing.Point(15, 133);
            this.DoRecovery.Name = "DoRecovery";
            this.DoRecovery.Size = new System.Drawing.Size(75, 23);
            this.DoRecovery.TabIndex = 8;
            this.DoRecovery.Text = "Recover";
            this.DoRecovery.UseVisualStyleBackColor = true;
            this.DoRecovery.Click += new System.EventHandler(this.DoRecovery_Click);
            // 
            // SecurityQuestions
            // 
            this.AcceptButton = this.DoRecovery;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialog;
            this.ClientSize = new System.Drawing.Size(582, 168);
            this.Controls.Add(this.CancelDialog);
            this.Controls.Add(this.DoRecovery);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SecurityAnswer2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SecurityQuestion2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SecurityAnswer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SecurityQuestion1);
            this.Name = "SecurityQuestions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Security Questions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CancelDialog;
        private System.Windows.Forms.Button DoRecovery;
        public System.Windows.Forms.TextBox SecurityQuestion1;
        public System.Windows.Forms.TextBox SecurityAnswer1;
        public System.Windows.Forms.TextBox SecurityAnswer2;
        public System.Windows.Forms.TextBox SecurityQuestion2;
    }
}