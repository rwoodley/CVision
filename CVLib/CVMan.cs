using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Image ModPicMode(Image img, int mode)
        {
            String inPath = getUPath();
            img.Save(inPath);
            String outPath = getUPath();
            CAPI.ModifyPictureMode(inPath, outPath, mode);
            return Image.FromFile(outPath);
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
