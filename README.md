# 3D-landmarks-Pose_estimation

This below link is my thesis presentation for this repository:

https://docs.google.com/presentation/d/e/2PACX-1vRLcMyNIFnfC8-tBpZ2aBrqCC2ou3Um9MNkX2QcJwBNZ3Wa982TMSxePJaMN-mTXw/pub?start=false&loop=false&delayms=3000

![image](https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/f6950543-1106-4b6b-9917-56d709233bf8)


![image](https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/153c04de-33bb-47d4-8342-b42779457ebe)

"BlazePose3D_Using_2D_Keypoints.ipynb" is a correct code(but MediaPipe code doesn't have a precise output),just we must set the address of 2D video as a file_path 
then this code save the coordinates of all frames of video in a txt file
now, we should give the address of each cordinates to Show_Skeleton.cs  code to show the skelton of animation in unity application
tip: drag and drop  the "Show_Skeleton.cs" into a Main_Camera to show the skeletons and Camera follow the character
tip: in unity --> we must add multi mini Circles in the scene with the corespond material then assigne each circle to each corespond variable of Show_Skeleton.cs script in Main_Camera
the result of Predicted 3d Landmarks with Blazepose is:




https://user-images.githubusercontent.com/82935599/235225239-ad8b83cc-ba77-460e-adff-eda826ef2bc0.mp4

comparing the ground_truth Skeleton with predicted Skeleton:


https://user-images.githubusercontent.com/82935599/235225446-35619024-0e2d-462a-b84c-54e6f310c27a.mp4

we can learn more about Blazepose ,here:
https://github.com/google/mediapipe/blob/master/docs/solutions/pose.md
https://developers.google.com/mediapipe/solutions/setup_python
https://github.com/google/mediapipe/blob/master/docs/solutions/pose.md 
ai.googleblog.com/2020/08/on-device-real-time-body-pose-tracking.html
 to find a best angle and best distances of camera, I captured 750 different angles and distances of animation like below:
 

https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/cfea708a-2332-423c-b466-b06fc71c7834


 then I've given the video to my model (BlazePose3D_Using_2D_Keypoints_all.ipynb) to predict the 3d coordinates and after that I've ploted the loss function to gain the accuarcy of model.
below plot is displaying the ground truth skeleton with all loss function for all landmarks,red circle= high loss function(bad) , green=good loss function and good result:

https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/fc09b137-cd5d-4dbf-8175-b23c52df37f8

![image](https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/6e56e690-5617-4695-a9b6-cd25a2cc4bc1)


Camera Configurations:

Quantifying model performance with a 3D grid of points surrounding the animation skeleton.

 Points converted to spherical coordinates: Radius, Azimuth, and Elevation.
  ![Screenshot from 2023-08-29 23-18-56](https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/126a420c-f96e-496c-9029-1901719ed713)

Analysis of camera configurations and their impact on character joint prediction accuracy.

Optimal camera distance, elevation, and azimuth for best results


![image](https://github.com/RaziehSh1987/3D-landmarks-Pose_estimation/assets/82935599/03bf59bb-4b43-41d2-85a2-abee7e68f755)


