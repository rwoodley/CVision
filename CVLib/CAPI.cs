using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;


namespace CVLib
{
    public class CAPI
    {
        public enum ImageMode { None = 0, Grayscale, ColorMap, Histogram }
        public enum BlurMode { None = 0, Blur = 1, Gaussian = 2, Median = 3, BiLateral = 4 }

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool ModifyPictureColor(String c_inputPath, String c_outputPath, int color);

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ModifyPictureMode(String c_inputPath, String c_outputPath, int mode, int cmmode);

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int HandleVideoFrame(String c_outputPath, double scaleFactor);

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void ModifyPictureMorph(String c_inputPath, String c_outputPath, int morphMode, int element, int kernelSize, int threshold);

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void ModifyPictureBool(String c_inputPath1, String c_inputPath2, String c_outputPath, int bMode);

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void ModifyPictureBlur(String c_inputPath, String c_outputPath, int bMode, int kernelSize);

        [DllImport("FaceRecognition.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void ModifyPictureContours(String c_inputPath, String c_outputPath, bool drawItems, int mode);

        public enum BooleanMode { AND = 1, OR, XOR, SUBTRACT, ADD, CONTOURS, ROTATIONPOINTS }
        public enum MorphStructureEnum { MORPH_RECT=0, MORPH_CROSS=1, MORPH_ELLIPSE=2, ONES = 3 };
        public enum MorphMode
        {
            NONE = 0,
            ERODE = 1,
            DILATE = 2,
            OPEN = 3,
            CLOSE = 4,
            GRADIENT = 5,
            TOPHAT = 6,
            BLACKHAT = 7,
            NOT = 8,
            SOBELX = 9,
            SOBELY = 10,
            LAPLACIAN = 11,
            SCHARRX1 = 12,
            SCHARRY1 = 13,
            CANNY = 14,
            THRESHOLD = 15
        }
        public enum ImageColor
        {
            NONE = -1,
            CV_BGR2BGRA    =0,
            CV_BGRA2BGR    =1,
            CV_BGR2RGBA    =2,
            CV_RGBA2BGR    =3,
            CV_BGR2RGB     =4,
            CV_BGRA2RGBA   =5,
            CV_BGR2GRAY    =6,
            CV_RGB2GRAY    =7,
            CV_GRAY2BGR    =8,
            CV_GRAY2BGRA   =9,
            CV_BGRA2GRAY   =10,
            CV_RGBA2GRAY   =11,

            CV_BGR2BGR565  =12,
            CV_RGB2BGR565  =13,
            CV_BGR5652BGR  =14,
            CV_BGR5652RGB  =15,
            CV_BGRA2BGR565 =16,
            CV_RGBA2BGR565 =17,
            CV_BGR5652BGRA =18,
            CV_BGR5652RGBA =19,

            CV_GRAY2BGR565 =20,
            CV_BGR5652GRAY =21,

            CV_BGR2BGR555  =22,
            CV_RGB2BGR555  =23,
            CV_BGR5552BGR  =24,
            CV_BGR5552RGB  =25,
            CV_BGRA2BGR555 =26,
            CV_RGBA2BGR555 =27,
            CV_BGR5552BGRA =28,
            CV_BGR5552RGBA =29,

            CV_GRAY2BGR555 =30,
            CV_BGR5552GRAY =31,

            CV_BGR2XYZ     =32,
            CV_RGB2XYZ     =33,
            CV_XYZ2BGR     =34,
            CV_XYZ2RGB     =35,

            CV_BGR2YCrCb   =36,
            CV_RGB2YCrCb   =37,
            CV_YCrCb2BGR   =38,
            CV_YCrCb2RGB   =39,

            CV_BGR2HSV     =40,
            CV_RGB2HSV     =41,

            CV_BGR2Lab     =44,
            CV_RGB2Lab     =45,

            CV_BayerBG2BGR =46,
            CV_BayerGB2BGR =47,
            CV_BayerRG2BGR =48,
            CV_BayerGR2BGR =49,

            CV_BGR2Luv     =50,
            CV_RGB2Luv     =51,
            CV_BGR2HLS     =52,
            CV_RGB2HLS     =53,

            CV_HSV2BGR     =54,
            CV_HSV2RGB     =55,

            CV_Lab2BGR     =56,
            CV_Lab2RGB     =57,
            CV_Luv2BGR     =58,
            CV_Luv2RGB     =59,
            CV_HLS2BGR     =60,
            CV_HLS2RGB     =61,

            CV_BayerBG2BGR_VNG =62,
            CV_BayerGB2BGR_VNG =63,
            CV_BayerRG2BGR_VNG =64,
            CV_BayerGR2BGR_VNG =65,

            CV_BGR2HSV_FULL = 66,
            CV_RGB2HSV_FULL = 67,
            CV_BGR2HLS_FULL = 68,
            CV_RGB2HLS_FULL = 69,

            CV_HSV2BGR_FULL = 70,
            CV_HSV2RGB_FULL = 71,
            CV_HLS2BGR_FULL = 72,
            CV_HLS2RGB_FULL = 73,

            CV_LBGR2Lab     = 74,
            CV_LRGB2Lab     = 75,
            CV_LBGR2Luv     = 76,
            CV_LRGB2Luv     = 77,

            CV_Lab2LBGR     = 78,
            CV_Lab2LRGB     = 79,
            CV_Luv2LBGR     = 80,
            CV_Luv2LRGB     = 81,

            CV_BGR2YUV      = 82,
            CV_RGB2YUV      = 83,
            CV_YUV2BGR      = 84,
            CV_YUV2RGB      = 85,

            CV_BayerBG2GRAY = 86,
            CV_BayerGB2GRAY = 87,
            CV_BayerRG2GRAY = 88,
            CV_BayerGR2GRAY = 89,

            //YUV 4:2:0 formats family
            CV_YUV2RGB_NV12 = 90,
            CV_YUV2BGR_NV12 = 91,
            CV_YUV2RGB_NV21 = 92,
            CV_YUV2BGR_NV21 = 93,

            CV_YUV2RGBA_NV12 = 94,
            CV_YUV2BGRA_NV12 = 95,
            CV_YUV2RGBA_NV21 = 96,
            CV_YUV2BGRA_NV21 = 97,

            CV_YUV2RGB_YV12 = 98,
            CV_YUV2BGR_YV12 = 99,
            CV_YUV2RGB_IYUV = 100,
            CV_YUV2BGR_IYUV = 101,

            CV_YUV2RGBA_YV12 = 102,
            CV_YUV2BGRA_YV12 = 103,
            CV_YUV2RGBA_IYUV = 104,
            CV_YUV2BGRA_IYUV = 105,

            CV_YUV2GRAY_420 = 106,

            CV_YUV2RGB_UYVY = 107,
            CV_YUV2BGR_UYVY = 108,

            CV_YUV2RGBA_UYVY = 111,
            CV_YUV2BGRA_UYVY = 112,

            CV_YUV2RGB_YUY2 = 115,
            CV_YUV2BGR_YUY2 = 116,
            CV_YUV2RGB_YVYU = 117,
            CV_YUV2BGR_YVYU = 118,

            CV_YUV2RGBA_YUY2 = 119,
            CV_YUV2BGRA_YUY2 = 120,
            CV_YUV2RGBA_YVYU = 121,
            CV_YUV2BGRA_YVYU = 122,

            CV_YUV2GRAY_UYVY = 123,
            CV_YUV2GRAY_YUY2 = 124,

            // alpha premultiplication
            CV_RGBA2mRGBA = 125,
            CV_mRGBA2RGBA = 126,

            // Edge-Aware Demosaicing
            CV_BayerBG2BGR_EA = 127,
            CV_BayerGB2BGR_EA = 128,
            CV_BayerRG2BGR_EA = 129,
            CV_BayerGR2BGR_EA = 130,

        }
        public enum ColorMap
        {
            COLORMAP_AUTUMN = 0,
            COLORMAP_BONE = 1,
            COLORMAP_JET = 2,
            COLORMAP_WINTER = 3,
            COLORMAP_RAINBOW = 4,
            COLORMAP_OCEAN = 5,
            COLORMAP_SUMMER = 6,
            COLORMAP_SPRING = 7,
            COLORMAP_COOL = 8,
            COLORMAP_HSV = 9,
            COLORMAP_PINK = 10,
            COLORMAP_HOT = 11
        }
    }
}
