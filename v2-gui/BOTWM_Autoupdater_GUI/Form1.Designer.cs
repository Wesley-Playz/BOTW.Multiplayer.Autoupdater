namespace BotWMultiplayerUpdaterGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            listBoxVersions = new ListBox();
            buttonDownload = new Button();
            labelStatus = new Label();
            textBox1 = new TextBox();
            progressBarDownload = new ProgressBar();
            SuspendLayout();
            // 
            // listBoxVersions
            // 
            listBoxVersions.BorderStyle = BorderStyle.FixedSingle;
            listBoxVersions.FormattingEnabled = true;
            listBoxVersions.ItemHeight = 30;
            listBoxVersions.Location = new Point(139, 151);
            listBoxVersions.Margin = new Padding(6, 7, 6, 7);
            listBoxVersions.Name = "listBoxVersions";
            listBoxVersions.Size = new Size(439, 212);
            listBoxVersions.TabIndex = 0;
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(1, 377);
            buttonDownload.Margin = new Padding(6, 7, 6, 7);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(720, 53);
            buttonDownload.TabIndex = 1;
            buttonDownload.Text = "Download";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Dock = DockStyle.Bottom;
            labelStatus.Location = new Point(0, 568);
            labelStatus.Margin = new Padding(6, 0, 6, 0);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(0, 30);
            labelStatus.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(1, 0);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(717, 141);
            textBox1.TabIndex = 3;
            textBox1.Text = resources.GetString("textBox1.Text");
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // progressBarDownload
            // 
            progressBarDownload.Location = new Point(326, 572);
            progressBarDownload.Name = "progressBarDownload";
            progressBarDownload.Size = new Size(395, 26);
            progressBarDownload.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(721, 598);
            Controls.Add(progressBarDownload);
            Controls.Add(labelStatus);
            Controls.Add(buttonDownload);
            Controls.Add(listBoxVersions);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 7, 6, 7);
            Name = "Form1";
            Text = "BotW Multiplayer Autoupdater";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ListBox listBoxVersions;
        private System.Windows.Forms.Button buttonDownload;
        private Label labelStatus;
        private TextBox textBox1;
        private ProgressBar progressBarDownload;
    }
}
