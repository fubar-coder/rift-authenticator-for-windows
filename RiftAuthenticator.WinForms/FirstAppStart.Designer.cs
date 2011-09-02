namespace RiftAuthenticator.WinForms
{
    partial class FirstAppStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstAppStart));
            this.label1 = new System.Windows.Forms.Label();
            this.CreateNewAuth = new System.Windows.Forms.RadioButton();
            this.RecoverOldAuth = new System.Windows.Forms.RadioButton();
            this.DoNothing = new System.Windows.Forms.RadioButton();
            this.SelectUserAction = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CreateNewAuth
            // 
            resources.ApplyResources(this.CreateNewAuth, "CreateNewAuth");
            this.CreateNewAuth.Checked = true;
            this.CreateNewAuth.Name = "CreateNewAuth";
            this.CreateNewAuth.TabStop = true;
            this.CreateNewAuth.UseVisualStyleBackColor = true;
            // 
            // RecoverOldAuth
            // 
            resources.ApplyResources(this.RecoverOldAuth, "RecoverOldAuth");
            this.RecoverOldAuth.Name = "RecoverOldAuth";
            this.RecoverOldAuth.UseVisualStyleBackColor = true;
            // 
            // DoNothing
            // 
            resources.ApplyResources(this.DoNothing, "DoNothing");
            this.DoNothing.Name = "DoNothing";
            this.DoNothing.UseVisualStyleBackColor = true;
            // 
            // SelectUserAction
            // 
            resources.ApplyResources(this.SelectUserAction, "SelectUserAction");
            this.SelectUserAction.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SelectUserAction.Name = "SelectUserAction";
            this.SelectUserAction.UseVisualStyleBackColor = true;
            // 
            // FirstAppStart
            // 
            this.AcceptButton = this.SelectUserAction;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SelectUserAction);
            this.Controls.Add(this.DoNothing);
            this.Controls.Add(this.RecoverOldAuth);
            this.Controls.Add(this.CreateNewAuth);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FirstAppStart";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SelectUserAction;
        internal System.Windows.Forms.RadioButton CreateNewAuth;
        internal System.Windows.Forms.RadioButton RecoverOldAuth;
        internal System.Windows.Forms.RadioButton DoNothing;
    }
}