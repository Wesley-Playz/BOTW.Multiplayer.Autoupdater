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
            textBox2 = new TextBox();
            SuspendLayout();
            // 
            // listBoxVersions
            // 
            listBoxVersions.BorderStyle = BorderStyle.FixedSingle;
            listBoxVersions.FormattingEnabled = true;
            listBoxVersions.ItemHeight = 30;
            listBoxVersions.Location = new Point(139, 152);
            listBoxVersions.Margin = new Padding(7, 8, 7, 8);
            listBoxVersions.Name = "listBoxVersions";
            listBoxVersions.Size = new Size(439, 212);
            listBoxVersions.TabIndex = 0;
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(2, 376);
            buttonDownload.Margin = new Padding(7, 8, 7, 8);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(720, 52);
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
            labelStatus.Margin = new Padding(7, 0, 7, 0);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(0, 30);
            labelStatus.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(2, 0);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(717, 140);
            textBox1.TabIndex = 3;
            textBox1.Text = "\r\nBuilt from the ground up Wesley Hellewell released on Oct 11th 2024\r\nThis auto updater will prepare the mod installation for you\r\nPlease do not close this installer until it is done";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // progressBarDownload
            // 
            progressBarDownload.Location = new Point(326, 572);
            progressBarDownload.Margin = new Padding(3, 4, 3, 4);
            progressBarDownload.Name = "progressBarDownload";
            progressBarDownload.Size = new Size(394, 26);
            progressBarDownload.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(151, 2);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(528, 28);
            textBox2.TabIndex = 5;
            textBox2.Text = "The 'better' Breath of the Wild Multiplayer Autoupdater";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(722, 598);
            Controls.Add(textBox2);
            Controls.Add(progressBarDownload);
            Controls.Add(labelStatus);
            Controls.Add(buttonDownload);
            Controls.Add(listBoxVersions);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(7, 8, 7, 8);
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
        private TextBox textBox2;
    }
}
