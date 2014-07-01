using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVLib
{
    public class CVMan
    {
        ImageCache _imageCache;
        Action<Image> _callback;
        int _ukey = 0;
        public CVMan(String tmpDirectory)
        {
            _imageCache = new ImageCache(tmpDirectory);
        }
        private string getUPath()
        {
            _ukey++;
            //_ukey = _ukey % 10;
            return _imageCache.GetImagePath(_ukey);
        }
        public Image ModPicColor(Image img, int color)
        {
            String inPath = getUPath();
            img.Save(inPath);
            String outPath = getUPath();
            if (CAPI.ModifyPictureColor(inPath, outPath, color))
                return Image.FromFile(outPath);
            else
                return null;
        }
        public Image ModPicMode(Image img, CAPI.ImageMode mode, CAPI.ColorMap cmmode)
        {
            if (mode == CAPI.ImageMode.None) return null;
            String inPath = getUPath();
            img.Save(inPath);
            String outPath = getUPath();
            CAPI.ModifyPictureMode(inPath, outPath, (int) mode, (int) cmmode);
            return Image.FromFile(outPath);
        }
        public Image ModPicMorph(Image img, CAPI.MorphMode mode, CAPI.MorphStructureEnum stelem, int kernelSize, int threshold)
        {
            String inPath = getUPath();
            img.Save(inPath);
            String outPath = getUPath();
            CAPI.ModifyPictureMorph(inPath, outPath, (int) mode, (int) stelem, kernelSize, threshold);
            return Image.FromFile(outPath);
        }
        public Image ModPicBlur(Image img, CAPI.BlurMode mode, int kernelSize)
        {
            String inPath = getUPath();
            img.Save(inPath);
            String outPath = getUPath();
            CAPI.ModifyPictureBlur(inPath, outPath, (int)mode, (int)kernelSize);
            return Image.FromFile(outPath);
        }
        public Image ModPicBoolean(Image img1, Image img2, CAPI.BooleanMode bmode, bool drawArtifacts)
        {
            if (img2 == null) return null;
            String inPath2 = getUPath();
            img2.Save(inPath2);
            String outPath = getUPath();
            if (bmode == CAPI.BooleanMode.CONTOURS)
                CAPI.ModifyPictureContours(inPath2, outPath, drawArtifacts, 1);
            else if (bmode == CAPI.BooleanMode.ROTATIONPOINTS)
                CAPI.ModifyPictureContours(inPath2, outPath, drawArtifacts, 2);
            else if (bmode == CAPI.BooleanMode.ROTATE_RESIZE)
                CAPI.ModifyPictureContours(inPath2, outPath, drawArtifacts, 3);
            else
            {
                if (img1 == null) return null;
                String inPath1 = getUPath();
                img1.Save(inPath1);
                CAPI.ModifyPictureBool(inPath1, inPath2, outPath, (int)bmode);
            }
            return System.IO.File.Exists(outPath) ? Image.FromFile(outPath) : null;
        }
        public Image ShrinkPic(Image image)
        {
            int newWidth = 300; int newHeight = 225;
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        public Image SnapPic()
        {
            String outPath = getUPath();
            CAPI.HandleVideoFrame(outPath, 1);
            return Image.FromFile(outPath);
        }
        public void StartVideoStream(Action<Image> callback) {
            _callback = callback;

            Action handleFrame = () =>
            {
                while (true)
                {
                    Image img = SnapPic();
                    callback(img);
                    System.Threading.Thread.Sleep(100);
                }
            };
            handleFrame.BeginInvoke(handleFrame.EndInvoke, null);
        }
    }
}
