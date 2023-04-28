# 3D-landmarks-Pose_estimation
"BlazePose3D_Using_2D_Keypoints.ipynb" is a correct code,just we must set the address of 2D video as a file_path 
then this code save the coordinates of all frames of video in a txt file
now, we should give the address of each cordinates to Show_Skeleton.cs  code to show the skelton of animation in unity application
tip: drag and drop  the "Show_Skeleton.cs" into a Main_Camera 
tip: in unity --> we must add multi mini Circles in the scene with the corespond material then assigne each circle to each corespond variable of Show_Skeleton.cs script in Main_Camera
the result of Predicted 3d Landmarks with Blazepose is:



https://user-images.githubusercontent.com/82935599/235225239-ad8b83cc-ba77-460e-adff-eda826ef2bc0.mp4

comparing the ground_truth Skeleton with predicted Skeleton:


https://user-images.githubusercontent.com/82935599/235225446-35619024-0e2d-462a-b84c-54e6f310c27a.mp4

