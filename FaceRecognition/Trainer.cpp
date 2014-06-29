#include "opencv2/core/core.hpp"
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/contrib/contrib.hpp"
#include "opencv2/imgproc/imgproc_c.h"
#include "opencv2/imgproc/types_c.h"

#include <stdio.h>
#include <math.h>
#include <iostream>
#include <fstream>
#include <sstream>

using namespace cv;
using namespace std;

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
	__declspec(dllexport) void ModifyPictureBool(char * inFileName1, char * inFileName2, char * outFileName, int bMode) {
		String filename = string(inFileName1);
		Mat img1 = imread(filename);
		filename = string(inFileName2);
		Mat img2 = imread(filename);
		Mat outImg;
		try {
			if (bMode == 1)
				bitwise_and(img1, img2, outImg);
			if (bMode == 2)
				bitwise_or(img1, img2, outImg);
			if (bMode == 3)
				bitwise_xor(img1, img2, outImg);
			if (bMode == 4)
				subtract(img1, img2, outImg);
			if (bMode == 5)
				add(img1, img2, outImg);
			imwrite(outFileName, outImg);
		}
		catch (cv::Exception& e) {
				cerr << e.msg << endl;
		}
	}
	__declspec(dllexport) void ModifyPictureMorph(char * inFileName, char * outFileName, int morphMode, int element, int kernelSize, int th) {
		String filename = string(inFileName);
		Mat img = imread(filename);
		Mat outImg;
		Mat st_elem;
		if (morphMode < 8) {
			if (element == 3)
				st_elem = Mat(kernelSize, kernelSize, CV_8UC1);	// see: http://stackoverflow.com/questions/18469734/opencv-matones-function
			else
				st_elem = getStructuringElement(element, Size(kernelSize, kernelSize));
		}
		try {
			if (morphMode == 1)
				erode(img, outImg, st_elem);
			if (morphMode == 2)
				dilate(img, outImg, st_elem);
			if (morphMode == 3)
				morphologyEx(img, outImg, MORPH_OPEN, st_elem);
			if (morphMode == 4)
				morphologyEx(img, outImg, MORPH_CLOSE, st_elem);
			if (morphMode == 5)
				morphologyEx(img, outImg, MORPH_GRADIENT, st_elem);
			if (morphMode == 6)
				morphologyEx(img, outImg, MORPH_TOPHAT, st_elem);
			if (morphMode == 7)
				morphologyEx(img, outImg, MORPH_BLACKHAT, st_elem);
			if (morphMode == 8)
				bitwise_not(img, outImg);
			if (morphMode == 9)  // sobel X
				Sobel(img, outImg, CV_64F, 1, 0, kernelSize);
			if (morphMode == 10)  // sobel Y
				Sobel(img, outImg, CV_64F, 0, 1, kernelSize);
			if (morphMode == 11) // laplace
				Laplacian(img, outImg, CV_64F, kernelSize);
			if (morphMode == 12) 
				Scharr(img, outImg, CV_64F, 1, 0);
			if (morphMode == 13) 
				Scharr(img, outImg, CV_64F, 0, 1);
			if (morphMode == 14)
				Canny(img, outImg, th, th * 3, 3);
			if (morphMode == 15)
				threshold(img, outImg, th,255,THRESH_BINARY);
		}
		catch (cv::Exception& e) {
				cerr << e.msg << endl;
		}

		imwrite(outFileName, outImg);
	}
	__declspec(dllexport) void ModifyPictureBlur(char * inFileName, char * outFileName, int mode, int kernelSize) {
		try {
			String filename = string(inFileName);
			Mat img = imread(filename);
			Mat outImg;
			if (mode == 1)
				blur(img, outImg, Size(kernelSize, kernelSize));
			if (mode == 2)
				GaussianBlur(img, outImg, Size(kernelSize, kernelSize), 1.5, 1.5);
			if (mode == 3)
				medianBlur(img, outImg, kernelSize);
			if (mode == 4)
				bilateralFilter(img, outImg, kernelSize, kernelSize*2, kernelSize/2);

			imwrite(outFileName, outImg);
		}
		catch (cv::Exception& e) {
				cerr << e.msg << endl;
		}
	}
	__declspec(dllexport) void ModifyPictureMode(char * inFileName, char * outFileName, int mode, int cmmode) {
		String filename = string(inFileName);
		Mat img = imread(filename);
		Mat outImg;
		if (mode == 1)
			cvtColor( img, outImg, CV_RGB2GRAY );
		if (mode == 2)
			applyColorMap(img, outImg, cmmode);
		if (mode == 3) {
			Mat tmp;
			cvtColor( img, tmp, CV_RGB2GRAY );
			equalizeHist(tmp, outImg);
		}

		imwrite(outFileName, outImg);
	}
	__declspec(dllexport) void ModifyPictureContours(char * inFileName, char * outFileName, bool drawItems, int mode) {
		String filename = string(inFileName);
		Mat img = imread(filename);
		int chan = img.channels();
		cvtColor(img,img, CV_RGB2GRAY);		// seems to be required, even if .net says only 8bpp
		chan = img.channels();
		//bool chk = CV_MAT_TYPE(img.type)== CV_32SC1;
		try {
			/// Find contours  
			vector<vector<Point> > contours;
			vector<Vec4i> hierarchy;
			RNG rng(12345);
			findContours( img, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0) );
			/// Draw contours
			float rows = (float) img.rows;
			float cols = (float) img.cols;
			int nContours = 0;
			// pass 1
			float minDistanceFromCenter = 9999999;	// best blob is the on nearest the center.
			for( int i = 0; i< contours.size(); i++ )
			{
				double area = contourArea(contours[i]);
				if (area < 20000) continue;
				if (nContours++ > 100) break;

				Moments mu = moments(contours[i], false);
				Point2f massCenter = Point2f( mu.m10/mu.m00 , mu.m01/mu.m00 );
				if (pointPolygonTest(contours[i], massCenter, false) < 0) continue;

				float distanceFromCenter = pow(pow(massCenter.y-rows/2,2.0) + pow(massCenter.x-cols/2,2),.5);
				if (distanceFromCenter < minDistanceFromCenter) minDistanceFromCenter = distanceFromCenter;
			}
			for( int i = 0; i< contours.size(); i++ )	// pass 2
			{
				double area = contourArea(contours[i]);
				if (area < 20000) continue;
				if (nContours++ > 100) break;
				Moments mu = moments(contours[i], false);
				Point2f massCenter = Point2f( mu.m10/mu.m00 , mu.m01/mu.m00 );
				if (pointPolygonTest(contours[i], massCenter, false) < 0) continue;

				// new check
				float distanceFromCenter = pow(pow(massCenter.y-rows/2,2.0) + pow(massCenter.x-cols/2,2),.5);
				if (distanceFromCenter > minDistanceFromCenter * 1.001) continue;

				Scalar color = Scalar( 255,255,255 );
				Mat drawing = Mat::zeros( img.size(), CV_8UC3 );
				drawContours( drawing, contours, i, color, CV_FILLED, 8, hierarchy, 0, Point() );

				Mat contourMat = Mat(contours[i]);
				Point2f center; float radius;
				minEnclosingCircle(contourMat, center, radius);
				if (drawItems) {

					// draw artifacts, for debugging only.
					//vector<Point> contour_poly;
					//approxPolyDP( contourMat, contour_poly, 3, true );

					circle( drawing, massCenter, 10, Scalar( 255,0,0), -1, 8, 0 );
					Rect rect = boundingRect(contourMat);
					rectangle( drawing, rect.tl(), rect.br(), Scalar( 0,255,0), 2, 8, 0 );
					RotatedRect rotRect = minAreaRect(contourMat);
				    Point2f rect_points[4]; rotRect.points( rect_points );
				    for( int j = 0; j < 4; j++ )
					    line( drawing, rect_points[j], rect_points[(j+1)%4], Scalar( 255,255,255), 1, 8 );
					radius /= 1.03; // see: http://code.opencv.org/issues/3362
					circle( drawing, center, (int)radius, Scalar(0,0,255), 2, 8, 0 );
				}
				if (mode == 1)
					imwrite(outFileName, drawing);
				else {
					// draw rotation points
					Mat drawing2 = Mat::zeros( img.size(), CV_8UC3 );
					drawContours( drawing2, contours, i, color, 2, 8, hierarchy, 0, Point() );
					Mat drawing3 = Mat::zeros( img.size(), CV_8UC3 );
					circle(drawing3, center, (int)radius, Scalar(0,0,255), 2, 8, 0 );
					Mat outImg;
					bitwise_and(drawing2, drawing3, outImg);	// get intersection of contour and containing circle.
					cvtColor(outImg, outImg, CV_RGB2GRAY);		// seems to be required, even if .net says only 8bpp

					// interesection will not be a point, might be a cluster of points. so get contours, and mass centers
					//medianBlur(outImg, outImg, 17);
					dilate(outImg, outImg, getStructuringElement(MORPH_ELLIPSE, Size(77, 77)));	// mush together all points so we end up w only 2 mass centers

					findContours( outImg, contours, hierarchy, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, Point(0, 0) );
					Mat drawing4 = Mat::zeros( img.size(), CV_8UC3 );
					vector<Point2f> rotationPoints;
					Point2f leftmostPoint;
					int leftmostPointIndex = -1;
					for( int i = 0; i< contours.size(); i++ )
					{
						Moments mu = moments(contours[i], false);
						Point2f massCenter = Point2f( mu.m10/mu.m00 , mu.m01/mu.m00 );
						rotationPoints.push_back(massCenter);
						if (leftmostPointIndex == -1 || massCenter.x < leftmostPoint.x) {
							leftmostPoint = massCenter;
							leftmostPointIndex = i;
						}
						circle( drawing4, massCenter, 10, Scalar( 255,0,0), -1, 8, 0 );	// mass centers are our points.
					}
					if (rotationPoints.size() == 2) {
						float angle = 90;
						if (rotationPoints[0].x != rotationPoints[1].x) {
							angle = atan((rotationPoints[0].y - rotationPoints[1].y)/(rotationPoints[0].x - rotationPoints[1].x));
							angle = 180*angle/3.1415;
						}
						float distance = sqrt(
							pow(rotationPoints[0].x - rotationPoints[1].x, 2) +
							pow(rotationPoints[0].y - rotationPoints[1].y, 2));
						if (distance > 0) {
							float factor = cols/distance;
							Mat r = getRotationMatrix2D(leftmostPoint, angle, 1.0);
							warpAffine(drawing4, drawing4, r, Size(factor*cols, factor*rows));
						}
						//if (distance > 0) {
						//	resize(drawing4, drawing4, Size(), cols/distance, cols/distance);
						//}
					}
					imwrite(outFileName, drawing4);
				}
				break;
			}
		}
		catch (cv::Exception& e) {
				cerr << e.msg << endl;
		}
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
