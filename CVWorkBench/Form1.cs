using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CVLib;

namespace CVWorkBench
{
    public partial class Form1 : Form
    {
        CVLib.CVMan _CVMan;
        Action<Image> _delegate;
        public Form1()
        {
            InitializeComponent();
            _CVMan = new CVMan(".\\");
            //_CVMan.StartVideoStream(showImage);
            _delegate = showImage;
            foreach (CAPI.ImageMode mode in Enum.GetValues(typeof(CAPI.ImageMode)))
                modeComboBox.Properties.Items.Add(mode.ToString());
            foreach (CAPI.ImageColor color in Enum.GetValues(typeof(CAPI.ImageColor)))
                colorComboBox.Properties.Items.Add(color.ToString());
            foreach (var mode in Enum.GetValues(typeof(DevExpress.XtraEditors.Controls.PictureSizeMode)))
                pictureEditModeComboBox.Properties.Items.Add(mode.ToString());
            pictureEdit1.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            modPictureEdit2.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            if (System.IO.File.Exists("Default.jpg")) SetMainImage(Image.FromFile("Default.jpg"));
        }
        private void SetMainImage(Image img)
        {
            pictureEdit1.Image = img;
            sizeLabel.Text = img == null ? "null" : img.Size.Height.ToString() + " x " + img.Size.Width.ToString();
        }
        private void SetModImage(Image img)
        {
            modPictureEdit2.Image = img;
            modSizeLabel.Text = img == null ? "null" : img.Size.Height.ToString() + " x " + img.Size.Width.ToString();
        }
        public void showImage(Image image)
        {
            if (InvokeRequired)
            {
                if (!this.IsDisposed && IsHandleCreated)
                {
                    BeginInvoke(_delegate, image);
                }
            }
            else
            {
                SetMainImage(image);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SetMainImage(_CVMan.SnapPic());
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetModImage(
            _CVMan.ModPicMode(pictureEdit1.Image, (int)
                Enum.Parse(typeof(CAPI.ImageMode), (String)((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue))
                );
        }

        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetModImage(
            _CVMan.ModPicColor(pictureEdit1.Image, (int)
                Enum.Parse(typeof(CAPI.ImageColor), (String)((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue))
                );

        }

        private void transferImage_Click(object sender, EventArgs e)
        {
            SetMainImage(modPictureEdit2.Image);
        }

        private void pictureEditModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = (DevExpress.XtraEditors.Controls.PictureSizeMode)Enum.Parse(typeof(DevExpress.XtraEditors.Controls.PictureSizeMode), (String)((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue);
            pictureEdit1.Properties.SizeMode = mode;
            modPictureEdit2.Properties.SizeMode = mode;
        }
    }
}
