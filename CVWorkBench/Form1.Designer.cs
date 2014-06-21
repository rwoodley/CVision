namespace CVWorkBench
{
    partial class Form1
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
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.modPictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.modeComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.colorComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.transferImage = new DevExpress.XtraEditors.SimpleButton();
            this.sizeLabel = new DevExpress.XtraEditors.LabelControl();
            this.modSizeLabel = new DevExpress.XtraEditors.LabelControl();
            this.pictureEditModeComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modPictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modeComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditModeComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(89, 41);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(375, 296);
            this.pictureEdit1.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(389, 353);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Snap";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // modPictureEdit2
            // 
            this.modPictureEdit2.Location = new System.Drawing.Point(650, 41);
            this.modPictureEdit2.Name = "modPictureEdit2";
            this.modPictureEdit2.Size = new System.Drawing.Size(375, 296);
            this.modPictureEdit2.TabIndex = 4;
            // 
            // modeComboBox
            // 
            this.modeComboBox.Location = new System.Drawing.Point(481, 122);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.modeComboBox.Size = new System.Drawing.Size(152, 22);
            this.modeComboBox.TabIndex = 5;
            this.modeComboBox.SelectedIndexChanged += new System.EventHandler(this.modeComboBox_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(481, 100);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 16);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "Mode:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(481, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(35, 16);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "Color:";
            // 
            // colorComboBox
            // 
            this.colorComboBox.Location = new System.Drawing.Point(481, 63);
            this.colorComboBox.Name = "colorComboBox";
            this.colorComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorComboBox.Size = new System.Drawing.Size(152, 22);
            this.colorComboBox.TabIndex = 7;
            this.colorComboBox.SelectedIndexChanged += new System.EventHandler(this.colorComboBox_SelectedIndexChanged);
            // 
            // transferImage
            // 
            this.transferImage.Location = new System.Drawing.Point(481, 314);
            this.transferImage.Name = "transferImage";
            this.transferImage.Size = new System.Drawing.Size(152, 23);
            this.transferImage.TabIndex = 9;
            this.transferImage.Text = "<<";
            this.transferImage.Click += new System.EventHandler(this.transferImage_Click);
            // 
            // sizeLabel
            // 
            this.sizeLabel.Location = new System.Drawing.Point(89, 353);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(23, 16);
            this.sizeLabel.TabIndex = 10;
            this.sizeLabel.Text = "hxw";
            // 
            // modSizeLabel
            // 
            this.modSizeLabel.Location = new System.Drawing.Point(650, 353);
            this.modSizeLabel.Name = "modSizeLabel";
            this.modSizeLabel.Size = new System.Drawing.Size(23, 16);
            this.modSizeLabel.TabIndex = 11;
            this.modSizeLabel.Text = "hxw";
            // 
            // pictureEditModeComboBox
            // 
            this.pictureEditModeComboBox.Location = new System.Drawing.Point(200, 355);
            this.pictureEditModeComboBox.Name = "pictureEditModeComboBox";
            this.pictureEditModeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.pictureEditModeComboBox.Size = new System.Drawing.Size(100, 22);
            this.pictureEditModeComboBox.TabIndex = 12;
            this.pictureEditModeComboBox.SelectedIndexChanged += new System.EventHandler(this.pictureEditModeComboBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 437);
            this.Controls.Add(this.pictureEditModeComboBox);
            this.Controls.Add(this.modSizeLabel);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.transferImage);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.colorComboBox);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.modPictureEdit2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.pictureEdit1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modPictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modeComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEditModeComboBox.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.PictureEdit modPictureEdit2;
        private DevExpress.XtraEditors.ComboBoxEdit modeComboBox;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit colorComboBox;
        private DevExpress.XtraEditors.SimpleButton transferImage;
        private DevExpress.XtraEditors.LabelControl sizeLabel;
        private DevExpress.XtraEditors.LabelControl modSizeLabel;
        private DevExpress.XtraEditors.ComboBoxEdit pictureEditModeComboBox;
    }
}

