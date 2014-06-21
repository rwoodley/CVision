using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CVLib
{
    class ImageCache
    {
        static ImageCache _imageCache;
        static String _imageCachePathRoot;
        // Yes singleton anti-pattern
        internal static ImageCache getImageCache(String tempPathRoot)
        {
            if (_imageCache == null)
            {
                _imageCache = new ImageCache(tempPathRoot);
                _imageCachePathRoot = tempPathRoot;
            }
            if (_imageCachePathRoot != tempPathRoot)
                throw new ApplicationException("Programming Error");
            return _imageCache;
        }
        internal static ImageCache getImageCache() { return _imageCache; }  // better be initialized if you call this.
        // The easiest way to pass images between C++ and C# is via the file system.
        // Each invocation of the program gets its own directory to use as long as it is running.
        private String _tempImageDirectoryRoot;
        String _tempImageDirectory = null;
        public ImageCache(String tempDirRoot)
        {
            _tempImageDirectoryRoot = tempDirRoot;
        }
        public String GetTempImagePath()
        {
            if (_tempImageDirectory == null)
            {
                _tempImageDirectory = _tempImageDirectoryRoot + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                System.IO.Directory.CreateDirectory(_tempImageDirectory);
            }
            return _tempImageDirectory + "\\";
        }
        internal String GetImagePath(int ukey)
        {
            String tmpPath = GetTempImagePath() + ukey + ".jpg";
            return tmpPath;
        }
    }
}
