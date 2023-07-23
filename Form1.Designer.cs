namespace JsonPdf6
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(192, 158);
            button1.Name = "button1";
            button1.Size = new Size(314, 181);
            button1.TabIndex = 0;
            button1.Text = "Upload a JSON file";
            button1.Click += UploadJSON_Click;
            // 
            // button2
            // 
            button2.Location = new Point(543, 665);
            button2.Name = "button2";
            button2.Size = new Size(150, 38);
            button2.TabIndex = 1;
            button2.Text = "Exit";
            button2.UseVisualStyleBackColor = true;
            button2.Click += ExitButton_Click;
            // 
            // button3
            // 
            button3.Location = new Point(245, 391);
            button3.Name = "button3";
            button3.Size = new Size(199, 29);
            button3.TabIndex = 2;
            button3.Text = "Display PDF";
            button3.UseVisualStyleBackColor = true;
            button3.Click += DisplayPDF_Click;
            // 
            // button4
            // 
            button4.Location = new Point(22, 47);
            button4.Name = "button4";
            button4.Size = new Size(105, 96);
            button4.TabIndex = 3;
            button4.Text = "Upload Logo";
            button4.UseVisualStyleBackColor = true;
            button4.Click += BtnUploadLogo_Click;
            // 
            // button5
            // 
            button5.Location = new Point(287, 536);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 4;
            button5.Text = "button5";
            button5.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(717, 715);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
    }
}