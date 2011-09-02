namespace RiftAuthenticator.WinForms
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.Email = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CancelDialog = new System.Windows.Forms.Button();
            this.DoLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Email
            // 
            resources.ApplyResources(this.Email, "Email");
            this.Email.Name = "Email";
            // 
            // Password
            // 
            resources.ApplyResources(this.Password, "Password");
            this.Password.Name = "Password";
            this.Password.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CancelDialog
            // 
            resources.ApplyResources(this.CancelDialog, "CancelDialog");
            this.CancelDialog.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelDialog.Name = "CancelDialog";
            this.CancelDialog.UseVisualStyleBackColor = true;
            // 
            // DoLogin
            // 
            resources.ApplyResources(this.DoLogin, "DoLogin");
            this.DoLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DoLogin.Name = "DoLogin";
            this.DoLogin.UseVisualStyleBackColor = true;
            this.DoLogin.Click += new System.EventHandler(this.DoLogin_Click);
            // 
            // Login
            // 
            this.AcceptButton = this.DoLogin;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelDialog;
            this.Controls.Add(this.CancelDialog);
            this.Controls.Add(this.DoLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Email);
            this.Name = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CancelDialog;
        private System.Windows.Forms.Button DoLogin;
        public System.Windows.Forms.TextBox Email;
        public System.Windows.Forms.TextBox Password;
    }
}