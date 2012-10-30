namespace pptPC
{
    partial class PCMainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(359, 96);
            this.label1.TabIndex = 0;
            this.label1.Text = "运行程序的时候请调整屏幕分辨率为800*600模拟投影仪演示。\r\n\r\n 1、打开电脑端程序， 电脑端程序会自动打开电脑上的蓝牙设备。\r\n\r\n 2、成功打开电脑端程" +
                "序之后，请切换到您要播放的幻灯片上面。\r\n\r\n 3、进行手机端操作。使用中请勿关闭此窗口\r\n\r\n";
            // 
            // PCMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 126);
            this.Controls.Add(this.label1);
            this.Name = "PCMainForm";
            this.Text = "PC-Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
    }
}

