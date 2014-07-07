using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CVLib;

namespace CVWorkBench
{
    class RecipeMan
    {
        public enum Recipes { TrimImage1, TrimImage2, TrimImage3, TrimImage4, RotateResize, Shrink, Not, BlackBorder10PX }

        CVLib.CVMan _CVMan;
        public RecipeMan(CVLib.CVMan man)
        {
            _CVMan = man;
        }
        public Image runRecipe(Recipes recipe, Image img1, bool maskOnly)
        {
            switch (recipe)
            {
                case Recipes.TrimImage1:
                    return TrimImage1(img1, maskOnly);
                case Recipes.TrimImage2:
                    return TrimImage2(img1, maskOnly);
                case Recipes.TrimImage3:
                    return TrimImage3(img1, maskOnly);
                case Recipes.TrimImage4:
                    return TrimImage4(img1, maskOnly);
                case Recipes.RotateResize:
                    return RotateResize(img1, maskOnly);
                case Recipes.Shrink:
                    return _CVMan.ShrinkPic(img1);
                case Recipes.Not:
                    return _CVMan.ModPicMorph(img1, CAPI.MorphMode.NOT, CAPI.MorphStructureEnum.MORPH_CROSS, 0, 0);
                case Recipes.BlackBorder10PX:
                    return _CVMan.ModPicMorph(img1, CAPI.MorphMode.BORDER, CAPI.MorphStructureEnum.MORPH_CROSS, 10, 0);
            }
            throw new NotImplementedException();
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
            if (mask == null) return null;
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
        private Image RotateResize(Image img1, bool ignored)
        {
            Image ret = _CVMan.ModPicBoolean(null, img1, CAPI.BooleanMode.ROTATE_RESIZE, false);
            return ret;
        }
    }
}
