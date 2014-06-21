#include "opencv2/core/core.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/imgproc/imgproc_c.h"

#include <stdio.h>
#include <iostream>
#include <fstream>
#include <sstream>

using namespace cv;
using namespace std;

static Mat norm_0_255(InputArray _src) {
	// from http://docs.opencv.org/modules/contrib/doc/facerec/facerec_tutorial.html
    Mat src = _src.getMat();
    // Create and return normalized image:
    Mat dst;
    switch(src.channels()) {
    case 1:
        cv::normalize(_src, dst, 0, 255, NORM_MINMAX, CV_8UC1);
        break;
    case 3:
        cv::normalize(_src, dst, 0, 255, NORM_MINMAX, CV_8UC3);
        break;
    default:
        src.copyTo(dst);
        break;
    }
    return dst;
}
void getAdjustedFace(InputArray imageSrc, OutputArray imageResult, int height, int width, int mode) {
	int rows = height, cols = width;
	if (rows == 0) {
		rows = 112; cols = 92;
	}
	if (mode == 1) {	// plain
		resize(imageSrc, imageResult, cvSize(cols, height));
		return;
	}
	// Either convert the image to greyscale, or use the existing greyscale image.
	Mat imageGrey = imageSrc.getMat();
	if (imageSrc.channels() != 1) cvtColor( imageSrc, imageGrey, CV_RGB2GRAY );
	if (mode == 2)	{	// grayscale 
		resize(imageGrey, imageResult, cvSize(cols, rows));
		return;
	}
	Mat imgTemp;
	resize(imageGrey, imgTemp, cvSize(cols, rows));
	if (mode == 3)	{	// color map
		applyColorMap(imgTemp, imageResult, COLORMAP_JET);
		return;
	}
	if (mode == 4) {	// histogram
		equalizeHist(imgTemp, imageResult);
		return;
	}

}


extern "C" {

	Ptr<FaceRecognizer> _model;
	bool _firstTime = true;

	__declspec(dllexport) bool ModifyPictureColor(char * inFileName, char * outFileName, int color) {
		String filename = string(inFileName);
		try {
			Mat img = imread(filename);
			Mat outImg;
			cvtColor(img, outImg, color);
			imwrite(outFileName, outImg);
			return true;
		}
		catch (cv::Exception& e) {
			try {
				Mat img = imread(filename, 0);
				Mat outImg;
				cvtColor(img, outImg, color);
				imwrite(outFileName, outImg);
				return true;
			}
			catch (cv::Exception& e) {
				return false;
			}
		}
	}
	__declspec(dllexport) void ModifyPictureMode(char * inFileName, char * outFileName, int mode) {
		String filename = string(inFileName);
		Mat img = imread(filename);
		Mat imgNormalized;
		Mat outImg;
		if (mode == 1)
			GaussianBlur(img, outImg, Size(7,7), 1.5, 1.5);
		if (mode == 2)
			cvtColor( img, outImg, CV_RGB2GRAY );
		if (mode == 3)
			applyColorMap(img, outImg, COLORMAP_JET);
		if (mode == 4) {
			Mat tmp;
			cvtColor( img, tmp, CV_RGB2GRAY );
			equalizeHist(tmp, outImg);
		}
		if (mode == 5)
			Canny(img, outImg, 0, 30, 3);

		imwrite(outFileName, outImg);
	}
	// scaleFactor: 
	// for 320x240 video stream, scaleFactor should be 1
	// for 640x480 video stream, scaleFactor should be 2
	// etc. Allows you to capture high quality pictures, but face detection will divide by the scale factor and hence be faster. It will work on
	// a smaller picture.
	__declspec(dllexport) int HandleVideoFrame(char * outFileName, double scaleFactor) {
		try {
			VideoCapture capture(0); //
			double videoWidth = 320 * scaleFactor;
			double videoHeight = 240 * scaleFactor;
			capture.set(CV_CAP_PROP_FRAME_WIDTH, videoWidth);
			capture.set(CV_CAP_PROP_FRAME_HEIGHT, videoHeight);
			Mat frame;
			capture >> frame;
			imwrite(outFileName, frame);
		}
		catch (cv::Exception& e) {
				cerr << e.msg << endl;
		}
		return 0;
	}
}
