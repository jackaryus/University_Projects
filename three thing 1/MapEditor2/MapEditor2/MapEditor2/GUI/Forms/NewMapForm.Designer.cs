namespace MapEditor2.GUI.Forms
{
    partial class NewMapForm
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
            this.NameLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.MapNameField = new System.Windows.Forms.TextBox();
            this.MapWidthField = new System.Windows.Forms.NumericUpDown();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.MapHeightField = new System.Windows.Forms.NumericUpDown();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MapWidthField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapHeightField)).BeginInit();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(12, 9);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(38, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name:";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(12, 41);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(38, 13);
            this.WidthLabel.TabIndex = 1;
            this.WidthLabel.Text = "Width:";
            // 
            // MapNameField
            // 
            this.MapNameField.Location = new System.Drawing.Point(56, 6);
            this.MapNameField.Name = "MapNameField";
            this.MapNameField.Size = new System.Drawing.Size(153, 20);
            this.MapNameField.TabIndex = 2;
            // 
            // MapWidthField
            // 
            this.MapWidthField.Location = new System.Drawing.Point(56, 39);
            this.MapWidthField.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.MapWidthField.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.MapWidthField.Name = "MapWidthField";
            this.MapWidthField.Size = new System.Drawing.Size(43, 20);
            this.MapWidthField.TabIndex = 3;
            this.MapWidthField.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(109, 41);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(41, 13);
            this.HeightLabel.TabIndex = 4;
            this.HeightLabel.Text = "Height:";
            // 
            // MapHeightField
            // 
            this.MapHeightField.Location = new System.Drawing.Point(156, 39);
            this.MapHeightField.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.MapHeightField.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.MapHeightField.Name = "MapHeightField";
            this.MapHeightField.Size = new System.Drawing.Size(43, 20);
            this.MapHeightField.TabIndex = 5;
            this.MapHeightField.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(32, 77);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(116, 77);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 113);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.MapHeightField);
            this.Controls.Add(this.HeightLabel);
            this.Controls.Add(this.MapWidthField);
            this.Controls.Add(this.MapNameField);
            this.Controls.Add(this.WidthLabel);
            this.Controls.Add(this.NameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewMapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Map";
            this.Load += new System.EventHandler(this.NewMapForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MapWidthField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapHeightField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.TextBox MapNameField;
        private System.Windows.Forms.NumericUpDown MapWidthField;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.NumericUpDown MapHeightField;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}