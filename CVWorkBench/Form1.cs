using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        RecipeMan _recipeMan;
        Action<Image> _delegate;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1450,750);        // why is this needed?
            _CVMan = new CVMan(".\\");
            _recipeMan = new RecipeMan(_CVMan);
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

            foreach (var recipeNo in Enum.GetValues(typeof(RecipeMan.Recipes)))
                recipeCompboBox.Properties.Items.Add(recipeNo.ToString());
            recipeCompboBox.SelectedIndex = 0;

            pictureEdit1.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            pictureEdit2.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            pictureEdit3.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            SetMainImage();

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
            inputDirectoryEdit.EditValue = "InputImages";
            outputDirectoryTextEdit.EditValue = "OutputImages";
        }
        private String getImageText(System.Drawing.Image img) {
            if (img == null) return "null";
            return String.Format("{0}x{1}. {2}",
                img.Size.Height.ToString(), img.Size.Width.ToString(), img.PixelFormat, Image.GetPixelFormatSize(img.PixelFormat));
        }
        private void SetMainImage()
        {
            String fileName = (String) onetimeFileNameEdit.EditValue;
            if (System.IO.File.Exists(fileName)) SetMainImage(Image.FromFile(fileName));
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

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            RecipeMan.Recipes selection = (RecipeMan.Recipes)Enum.Parse(typeof(RecipeMan.Recipes), (String)recipeCompboBox.EditValue);
            if (MessageBox.Show(String.Format("Will process files in {0} using {1} recipe, writing to {2}",
                (String)inputDirectoryEdit.EditValue,
                selection.ToString(),
                (String)outputDirectoryTextEdit.EditValue), "Confirm", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK) return;
            foreach (string file in System.IO.Directory.EnumerateFiles((String) inputDirectoryEdit.EditValue, "*.jpg"))
            {
                Image img = Image.FromFile(file);
                Image outimg = _recipeMan.runRecipe(selection, img, false);
                outimg.Save(outputDirectoryTextEdit.EditValue + "/" + System.IO.Path.GetFileName(file));
            }
        }

        private void oneTimeButton_Click(object sender, EventArgs e)
        {
            RecipeMan.Recipes selection = (RecipeMan.Recipes)Enum.Parse(typeof(RecipeMan.Recipes), (String)recipeCompboBox.EditValue);
            SetModImage(_recipeMan.runRecipe(selection, pictureEdit1.Image, true));
        }

        private void inputDirectoryButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    inputDirectoryEdit.EditValue = dlg.SelectedPath;
                }
            }
        }

        private void outputDirectoryButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    outputDirectoryTextEdit.EditValue = dlg.SelectedPath;
                }
            }
        }

        private void onetimeFileNameEdit_EditValueChanged(object sender, EventArgs e)
        {
            SetMainImage();
        }

        private void rotateResizeButton_Click(object sender, EventArgs e)
        {
            Image img = _CVMan.ModPicBoolean(null, pictureEdit1.Image, CAPI.BooleanMode.ROTATE_RESIZE, false);
            SetModImage(img);
        }

        private void shrinkButton_Click(object sender, EventArgs e)
        {
            SetModImage(_CVMan.ShrinkPic(pictureEdit1.Image));
        }



    }
}
