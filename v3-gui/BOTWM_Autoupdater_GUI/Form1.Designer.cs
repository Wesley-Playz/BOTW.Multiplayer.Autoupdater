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
            listBoxVersions.ItemHeight = 15;
            listBoxVersions.Location = new Point(81, 76);
            listBoxVersions.Margin = new Padding(4);
            listBoxVersions.Name = "listBoxVersions";
            listBoxVersions.Size = new Size(257, 107);
            listBoxVersions.TabIndex = 0;
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(1, 188);
            buttonDownload.Margin = new Padding(4);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(420, 26);
            buttonDownload.TabIndex = 1;
            buttonDownload.Text = "Download";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Dock = DockStyle.Bottom;
            labelStatus.Location = new Point(0, 284);
            labelStatus.Margin = new Padding(4, 0, 4, 0);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(0, 15);
            labelStatus.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(1, 0);
            textBox1.Margin = new Padding(2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(418, 70);
            textBox1.TabIndex = 3;
            textBox1.Text = "\r\nBuilt from the ground up by Wesley Hellewell released on Oct 11th 2024\r\nThis auto updater will prepare the mod installation for you\r\nPlease do not close this installer until it is done";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // progressBarDownload
            // 
            progressBarDownload.Location = new Point(190, 286);
            progressBarDownload.Margin = new Padding(2);
            progressBarDownload.Name = "progressBarDownload";
            progressBarDownload.Size = new Size(230, 13);
            progressBarDownload.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Location = new Point(88, 1);
            textBox2.Margin = new Padding(2);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(308, 16);
            textBox2.TabIndex = 5;
            textBox2.Text = "The 'better' Breath of the Wild Multiplayer Autoupdater";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(418, 299);
            Controls.Add(textBox2);
            Controls.Add(progressBarDownload);
            Controls.Add(labelStatus);
            Controls.Add(buttonDownload);
            Controls.Add(listBoxVersions);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
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
