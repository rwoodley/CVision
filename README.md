CVision   
=======   

Computer Vision WorkBench and Utilities. Written in WinForms. Wraps OpenCV.   

![ScreenShot](screenshot.png?raw=true)   

Features:   
- Has all OpenCV color options and color maps. 
- Histogram Equalization   
- 4 Blurs   
- Many Morph Modes:   
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
- You can specify morph type (RECT, CROSS, ELLIPSE) as well as kernel size and threshold (where appropriate).   
- Ability to group operations into 'recipes'.   
- Boolean operations.   
- Intelligent contours and rotation.  
- Ability to process in batch mode across a directory full of images. 
- All intermediate transformations are saved.   

Just built it for my purposes. There is plenty to improve.

