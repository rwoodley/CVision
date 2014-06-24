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
            this.Size = new Size(1450,750);        // why is this needed?
            _CVMan = new CVMan(".\\");
            //_CVMan.StartVideoStream(showImage);
            _delegate = showImage;
            foreach (CAPI.ImageMode mode in Enum.GetValues(typeof(CAPI.ImageMode)))
                modeComboBox.Properties.Items.Add(mode.ToString());
            foreach (CAPI.ImageColor color in Enum.GetValues(typeof(CAPI.ImageColor)))
                colorComboBox.Properties.Items.Add(color.ToString());
            foreach (CAPI.ColorMap colorMap in Enum.GetValues(typeof(CAPI.ColorMap)))
                colorMapComboBox.Properties.Items.Add(colorMap.ToString());
            foreach (var mode in Enum.GetValues(typeof(DevExpress.XtraEditors.Controls.PictureSizeMode)))
                pictureEditModeComboBox.Properties.Items.Add(mode.ToString());
            foreach (var morphMode in Enum.GetValues(typeof(CAPI.MorphMode)))
                morphModeComboBox.Properties.Items.Add(morphMode.ToString());
            foreach (var se in Enum.GetValues(typeof(CAPI.MorphStructureEnum)))
                structuringElementComboBox.Properties.Items.Add(se.ToString());
            foreach (var bmode in Enum.GetValues(typeof(CAPI.BooleanMode)))
                booleanComboBox.Properties.Items.Add(bmode.ToString());
            foreach (var blurmode in Enum.GetValues(typeof(CAPI.BlurMode)))
                blurComboBox.Properties.Items.Add(blurmode.ToString());

            pictureEdit1.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            pictureEdit2.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            pictureEdit3.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            if (System.IO.File.Exists("Default.jpg")) SetMainImage(Image.FromFile("Default.jpg"));

            morphModeComboBox.SelectedIndex = 0;
            structuringElementComboBox.SelectedIndex = 0;
            modeComboBox.SelectedIndex = 0;
            colorMapComboBox.SelectedIndex = 1;
            colorMapLabel.Visible = false;
            colorMapComboBox.Visible = false;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            pictureEdit3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            booleanComboBox.SelectedIndex = 0;
            blurComboBox.SelectedIndex = 0;
            kernelSizeEdit.EditValue = 7;
        }
        private String getImageText(System.Drawing.Image img) {
            if (img == null) return "null";
            return String.Format("{0}x{1}. {2}",
                img.Size.Height.ToString(), img.Size.Width.ToString(), img.PixelFormat, Image.GetPixelFormatSize(img.PixelFormat));
        }
        private void SetMainImage(Image img)
        {
            pictureEdit1.Image = img;
            sizeLabel1.Text = getImageText(img);
        }
        private void SetModImage(Image img)
        {
            pictureEdit2.Image = img;
            sizeLabel2.Text = getImageText(img);
            UpdateImage3();
        }
        private void UpdateImage3()
        {
            if (booleanComboBox.EditValue == null) return;
            var bmode = (CAPI.BooleanMode)Enum.Parse(typeof(CAPI.BooleanMode), (String)booleanComboBox.EditValue);
            pictureEdit3.Image = _CVMan.ModPicBoolean(pictureEdit1.Image, pictureEdit2.Image, bmode, true);
            sizeLabel3.Text = getImageText(pictureEdit3.Image);
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
            blurComboBox.SelectedIndex = 0;
            morphModeComboBox.SelectedIndex = 0;
            CAPI.ImageMode mode = (CAPI.ImageMode) Enum.Parse(typeof(CAPI.ImageMode), (String)modeComboBox.EditValue);
            colorMapLabel.Visible = mode == CAPI.ImageMode.ColorMap;
            colorMapComboBox.Visible = mode == CAPI.ImageMode.ColorMap;
            CAPI.ColorMap cmmode = (colorMapComboBox.EditValue == null) ? CAPI.ColorMap.COLORMAP_AUTUMN : (CAPI.ColorMap)Enum.Parse(typeof(CAPI.ColorMap), (String)colorMapComboBox.EditValue);
            Image img = _CVMan.ModPicMode(pictureEdit1.Image, mode, cmmode);
            SetModImage(img);
        }
        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            blurComboBox.SelectedIndex = 0;
            morphModeComboBox.SelectedIndex = 0;
            SetModImage(
            _CVMan.ModPicColor(pictureEdit1.Image, (int)
                Enum.Parse(typeof(CAPI.ImageColor), (String)((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue))
                );

        }
        private void transferImage_Click(object sender, EventArgs e)
        {
            SetMainImage(pictureEdit2.Image);
        }
        private void pictureEditModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = (DevExpress.XtraEditors.Controls.PictureSizeMode)Enum.Parse(typeof(DevExpress.XtraEditors.Controls.PictureSizeMode), (String)((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue);
            pictureEdit1.Properties.SizeMode = mode;
            pictureEdit2.Properties.SizeMode = mode;
            pictureEdit3.Properties.SizeMode = mode;
        }
        private void colorMapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modeComboBox_SelectedIndexChanged(null, null);
        }
        private void morphModeComboBox_SelectedIndexChanged(object senderx, EventArgs ex)
        {
            blurComboBox.SelectedIndex = 0;
            modeComboBox.SelectedIndex = 0;
            if (morphModeComboBox.EditValue == null) return;
            var mode = (CAPI.MorphMode) Enum.Parse(typeof(CAPI.MorphMode), (String)morphModeComboBox.EditValue);
            if (mode == CAPI.MorphMode.NONE) return;
            var se = (CAPI.MorphStructureEnum) Enum.Parse(typeof(CAPI.MorphStructureEnum), (String)structuringElementComboBox.EditValue);
            SetModImage(
                _CVMan.ModPicMorph(pictureEdit1.Image, mode, se, 
                    getInt(kernelSizeEdit.EditValue), getInt(thresholdEdit.EditValue))
                    );
        }

        private void booleanComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateImage3();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            pictureEdit1.Image = pictureEdit3.Image;
        }

        private void blurComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            modeComboBox.SelectedIndex = 0;
            colorComboBox.SelectedIndex = 0;
            var mode = (CAPI.BlurMode)Enum.Parse(typeof(CAPI.BlurMode), (String)blurComboBox.EditValue);
            if (mode == CAPI.BlurMode.None) return;
            SetModImage(_CVMan.ModPicBlur(pictureEdit1.Image, mode, getInt(kernelSizeEdit.EditValue)));
        }
        static int getInt(object ev) {
            int ks;
            if (ev == null) return 7;
            if (ev is string)
            {
                if (!int.TryParse((String)ev, out ks)) ks = 7;
            }
            else
                ks = (int)ev;
            return ks;
        }
        private void kernelSizeEdit_EditValueChanged(object sender, EventArgs e)
        {
            var blurmode = (CAPI.BlurMode)Enum.Parse(typeof(CAPI.BlurMode), (String)blurComboBox.EditValue);
            if (blurmode == CAPI.BlurMode.None)
                morphModeComboBox_SelectedIndexChanged(null, null);
            else
                blurComboBox_SelectedIndexChanged(null, null);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            var result = TrimImage1(pictureEdit1.Image, true);
            SetModImage(result);
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            var result = TrimImage2(pictureEdit1.Image, true);
            SetModImage(result);

        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            var result = TrimImage3(pictureEdit1.Image, true);
            SetModImage(result);
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            var result = TrimImage4(pictureEdit1.Image, true);
            SetModImage(result);
        }

        private Image TrimImage1(Image img1, bool maskOnly)
        {
            Image img = _CVMan.ModPicMode(img1, CAPI.ImageMode.Grayscale, CAPI.ColorMap.COLORMAP_AUTUMN);
            img = _CVMan.ModPicMode(img1, CAPI.ImageMode.Histogram, CAPI.ColorMap.COLORMAP_AUTUMN);
            img = _CVMan.ModPicMorph(img, CAPI.MorphMode.THRESHOLD, CAPI.MorphStructureEnum.MORPH_CROSS, 0, 200);
            img = _CVMan.ModPicBlur(img, CAPI.BlurMode.Median, 27);
            img = _CVMan.ModPicMorph(img, CAPI.MorphMode.DILATE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 17, 0);
            img = _CVMan.ModPicMorph(img, CAPI.MorphMode.ERODE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 7, 0);
            Image mask = _CVMan.ModPicBoolean(img, img, CAPI.BooleanMode.CONTOURS, false);
            mask = _CVMan.ModPicBlur(mask, CAPI.BlurMode.Median, 27);
            Image result = _CVMan.ModPicBoolean(img1, mask, CAPI.BooleanMode.AND, false);

            return maskOnly ? mask : result;
        }
        private Image TrimImage2(Image img1, bool maskOnly)
        {
            // The above works in many cases, but not all. Futher cleanup:
            Image result = _CVMan.ModPicMode(img1, CAPI.ImageMode.Histogram, CAPI.ColorMap.COLORMAP_AUTUMN);
            result = _CVMan.ModPicMorph(result, CAPI.MorphMode.THRESHOLD, CAPI.MorphStructureEnum.MORPH_CROSS, 0, 200);
            result = _CVMan.ModPicMorph(result, CAPI.MorphMode.ERODE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 23, 0);
            result = _CVMan.ModPicMorph(result, CAPI.MorphMode.DILATE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 23, 0);
            Image mask = _CVMan.ModPicBoolean(result, result, CAPI.BooleanMode.CONTOURS, false);
            result = _CVMan.ModPicBoolean(img1, mask, CAPI.BooleanMode.AND, false);

            return maskOnly ? mask : result;
        }
        private Image TrimImage3(Image img1, bool maskOnly)
        {
            // The above works in many cases, but not all. Futher cleanup:
            Image result = _CVMan.ModPicMode(img1, CAPI.ImageMode.Histogram, CAPI.ColorMap.COLORMAP_AUTUMN);
            Image mask = _CVMan.ModPicMorph(result, CAPI.MorphMode.THRESHOLD, CAPI.MorphStructureEnum.MORPH_CROSS, 0, 160);
            mask = _CVMan.ModPicMorph(mask, CAPI.MorphMode.DILATE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 17, 0);
            mask = _CVMan.ModPicBlur(mask, CAPI.BlurMode.Median, 17);
            mask = _CVMan.ModPicMorph(mask, CAPI.MorphMode.ERODE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 13, 0);
            mask = _CVMan.ModPicBoolean(mask, mask, CAPI.BooleanMode.CONTOURS, false);
            result = _CVMan.ModPicBoolean(img1, mask, CAPI.BooleanMode.AND, false);

            return maskOnly ? mask : result;
        }
        private Image TrimImage4(Image img1, bool maskOnly)
        {
            Image mask = _CVMan.ModPicMode(img1, CAPI.ImageMode.Histogram, CAPI.ColorMap.COLORMAP_AUTUMN);
            mask = _CVMan.ModPicMorph(mask, CAPI.MorphMode.ERODE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 13, 0);
            mask = _CVMan.ModPicMorph(mask, CAPI.MorphMode.THRESHOLD, CAPI.MorphStructureEnum.MORPH_CROSS, 0, 120);
            mask = _CVMan.ModPicMorph(mask, CAPI.MorphMode.DILATE, CAPI.MorphStructureEnum.MORPH_ELLIPSE, 17, 0);
            mask = _CVMan.ModPicBlur(mask, CAPI.BlurMode.Median, 17);
            mask = _CVMan.ModPicBoolean(mask, mask, CAPI.BooleanMode.CONTOURS, false);
            Image result = _CVMan.ModPicBoolean(img1, mask, CAPI.BooleanMode.AND, false);

            return maskOnly ? mask : result;
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            foreach (string file in System.IO.Directory.EnumerateFiles("InputImages", "*.jpg"))
            {
                Image img = Image.FromFile(file);
                Image outimg = TrimImage4(img, false);
                //Image mask = TrimImage3(outimg, true);
                //mask = TrimImage1(outimg, true);
                //Image result = _CVMan.ModPicBoolean(img, mask, CAPI.BooleanMode.AND);

                outimg.Save("OutputImages/" + System.IO.Path.GetFileName(file));
            }
        }



    }
}
