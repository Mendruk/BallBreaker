namespace Ball_Breaker
{
    partial class MainForm
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
            this.pictureGameField = new System.Windows.Forms.PictureBox();
            this.buttonRestart = new System.Windows.Forms.Button();
            this.buttonCancelTurn = new System.Windows.Forms.Button();
            this.labelScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGameField)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureGameField
            // 
            this.pictureGameField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureGameField.BackColor = System.Drawing.SystemColors.Window;
            this.pictureGameField.Location = new System.Drawing.Point(12, 12);
            this.pictureGameField.Name = "pictureGameField";
            this.pictureGameField.Size = new System.Drawing.Size(577, 577);
            this.pictureGameField.TabIndex = 0;
            this.pictureGameField.TabStop = false;
            this.pictureGameField.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureGameField_Paint);
            this.pictureGameField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureGameField_MouseClick);
            // 
            // buttonRestart
            // 
            this.buttonRestart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRestart.Location = new System.Drawing.Point(12, 612);
            this.buttonRestart.Name = "buttonRestart";
            this.buttonRestart.Size = new System.Drawing.Size(113, 29);
            this.buttonRestart.TabIndex = 1;
            this.buttonRestart.Text = "Restart";
            this.buttonRestart.UseVisualStyleBackColor = true;
            // 
            // buttonCancelTurn
            // 
            this.buttonCancelTurn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelTurn.Location = new System.Drawing.Point(476, 612);
            this.buttonCancelTurn.Name = "buttonCancelTurn";
            this.buttonCancelTurn.Size = new System.Drawing.Size(113, 29);
            this.buttonCancelTurn.TabIndex = 2;
            this.buttonCancelTurn.Text = "Cancel turn";
            this.buttonCancelTurn.UseVisualStyleBackColor = true;
            // 
            // labelScore
            // 
            this.labelScore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(245, 612);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(61, 20);
            this.labelScore.TabIndex = 3;
            this.labelScore.Text = "Score: 0";
            this.labelScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 653);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.buttonCancelTurn);
            this.Controls.Add(this.buttonRestart);
            this.Controls.Add(this.pictureGameField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Ball Breaker";
            ((System.ComponentModel.ISupportInitialize)(this.pictureGameField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureGameField;
        private Button buttonRestart;
        private Button buttonCancelTurn;
        private Label labelScore;
    }
}