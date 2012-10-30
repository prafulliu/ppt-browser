namespace Ppt_Mobile
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.btStart = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.panPicture = new System.Windows.Forms.Panel();
            this.picBoxUp = new System.Windows.Forms.PictureBox();
            this.panButtons = new System.Windows.Forms.Panel();
            this.btUp = new System.Windows.Forms.Button();
            this.btNext = new System.Windows.Forms.Button();
            this.panPicture.SuspendLayout();
            this.panButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem4);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.MenuItems.Add(this.btStart);
            this.menuItem1.Text = "菜单";
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "启动服务";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // btStart
            // 
            this.btStart.Text = "开始演示";
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "退出";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // panPicture
            // 
            this.panPicture.AutoScroll = true;
            this.panPicture.Controls.Add(this.picBoxUp);
            this.panPicture.Dock = System.Windows.Forms.DockStyle.Top;
            this.panPicture.Location = new System.Drawing.Point(0, 0);
            this.panPicture.Name = "panPicture";
            this.panPicture.Size = new System.Drawing.Size(240, 197);
            // 
            // picBoxUp
            // 
            this.picBoxUp.Image = ((System.Drawing.Image)(resources.GetObject("picBoxUp.Image")));
            this.picBoxUp.Location = new System.Drawing.Point(0, 3);
            this.picBoxUp.Name = "picBoxUp";
            this.picBoxUp.Size = new System.Drawing.Size(400, 300);
            this.picBoxUp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBoxUp_MouseMove);
            this.picBoxUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoxUp_MouseDown);
            this.picBoxUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBoxUp_MouseUp);
            // 
            // panButtons
            // 
            this.panButtons.BackColor = System.Drawing.SystemColors.Info;
            this.panButtons.Controls.Add(this.btUp);
            this.panButtons.Controls.Add(this.btNext);
            this.panButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panButtons.Location = new System.Drawing.Point(0, 203);
            this.panButtons.Name = "panButtons";
            this.panButtons.Size = new System.Drawing.Size(240, 65);
            // 
            // btUp
            // 
            this.btUp.Location = new System.Drawing.Point(44, 17);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(58, 35);
            this.btUp.TabIndex = 2;
            this.btUp.Text = "上一页";
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // btNext
            // 
            this.btNext.Location = new System.Drawing.Point(134, 17);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(59, 35);
            this.btNext.TabIndex = 1;
            this.btNext.Text = "下一页";
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.panButtons);
            this.Controls.Add(this.panPicture);
            this.Menu = this.mainMenu1;
            this.Name = "formMain";
            this.Text = "Form1";
            this.panPicture.ResumeLayout(false);
            this.panButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panPicture;
        private System.Windows.Forms.Panel panButtons;
        private System.Windows.Forms.PictureBox picBoxUp;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem btStart;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btNext;
    }
}

