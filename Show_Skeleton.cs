using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;


public class Show_Skeleton : MonoBehaviour
{   
    // Array to store the landmarks
    // public Vector3[] landmarks;
    // public Vector3[] LandmaksVectors;
    public string[] lines;
    public string[] lines_original;
    public int previousFrameNumber = 0;
    public int previousFrameNumber_original = 0;
    public string textLine="";
    public string textLine_original = "";
    public string landmarkName="";
    public string landmarkName_original = "";

    public Transform NosePosition;
    public Transform RightEyeInnerPosition;
    public Transform RightEyePosition;
    public Transform RightEyeOuterPosition;
    public Transform LeftEyeInnerPosition;
    public Transform LeftEyePosition;
    public Transform LeftEyeOuterPosition;
    public Transform RightEarPosition;
    public Transform LeftEarPosition;
    public Transform MouthRightPosition;
    public Transform MouthLeftPosition;
    public Transform RightShoulderPosition;
    public Transform LeftShoulderPosition;
    public Transform RightElbowPosition;
    public Transform LeftElbowPosition;
    public Transform RightWristPosition;
    public Transform LeftWristPosition;
    public Transform RightHipPosition;
    public Transform LeftHipPosition;
    public Transform RightKneePosition;
    public Transform LeftKneePosition;
    public Transform RightAnklePosition;
    public Transform LeftAnklePosition;
    public Transform RightPinkyKnucklePosition;
    public Transform LeftPinkyKnucklePosition;
    public Transform RightIndexKnucklePosition;
    public Transform LeftIndexKnucklePosition;
    public Transform RightThumbknucklePosition;
    public Transform LeftThumbknucklePosition;
    public Transform RightHeelPosition;
    public Transform LeftHeelPosition;
    public Transform RightFootIndexPosition;
    public Transform LeftFootIndexPosition;
    // transform for original Animation
    public Transform HeadTop_End;
    public Transform Spine2;
    public Transform LeftShoulder;
    public Transform RightShoulder;
    public Transform LeftElbow;
    public Transform RightElbow;
    public Transform LeftHand;
    public Transform RightHand;
    public Transform Hips;
    public Transform LeftHip;
    public Transform RightHip;
    public Transform LeftKnee;
    public Transform RightKnee;
    public Transform LeftAnkle;
    public Transform RightAnkle;

    public int i = 0;
    public int i_original = 0;
    public int j;
    public int j_original;

    // float timeELaps;
    public GameObject linesObject;
    public GameObject linesObject_original;


// /0: Right ankle--8
// 1: Right knee--8
// 2: Right hip--8
// 3: Left hip--8
// 4: Left knee--8
// 5: Left ankle--8
// 6: Pelvis--
// 7: Thorax
// 8: Upper neck
// 9: Head top--8
// 10: Right wrist--8
// 11: Right elbow--8
// 12: Right shoulder--8
// 13: Left shoulder--8
// 14: Left elbow--8
// 15: Left wrist--8
// 16: Chest(center of the shoulders)--8


    // Start is called before the first frame update
    void Start()
    {
        // //--- string textLine = "Landmark [10,29]: (-0.26858606934547424, -0.19701218605041504, -0.7247674465179443)";    

        // // Read the text file to plot skeleton of the predicted animation
        // string filePath = "Assets/Plot_Skeleton/Script/coordinate.txt";
        string filePath = "Assets/Plot_Skeleton/Script/3d_landmarks_2.txt";
        // 3d_landmarks_1";    coordinate4.txt";
        lines = File.ReadAllLines(filePath);
        // previousFrameNumber = 0;
        print("lines.Length=" + lines.Length);

        // Read the text file to plot skeleton of the original animation-----------------------
        string filepath_original = "Assets/Plot_Skeleton/Script/AnimationTransform.txt";
        lines_original = File.ReadAllLines(filepath_original);
        print("lines_orig.Length=" + lines_original.Length);

    }
    // https://www.youtube.com/watch?v=MFQhpwc6cKE
    // folloew camera
    public  Transform player;
    public float smoothSpeed = 0f;//0.125f;//degree af camera rotation and for camera  position we should change the y and z properties in Inspector
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // folllow camera
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(player);
        // transform.position = player.position;


        // plot skeleton
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying =false;
            }
        }
        // ................plot predicted skeleton.......................
        
        if (i == lines.Length-1)
        {
            i=0;
            // Destroy(linesObject);
            previousFrameNumber = 0;
            print("33333");
            Debug.Log("Landmark not found");
            // if (EditorApplication.isPlaying)
            // {
            // EditorApplication.isPlaying = false;
            // }
        }
        else
        if (i < lines.Length)
        {    
            // erase all lines for an old frame
            Destroy(linesObject);
            // plot all lines for new frame
            linesObject = new GameObject("Lines");
            j = 0;
            
            textLine = lines[i];
            previousFrameNumber = FindFrameNumber(textLine);

            // plot landmark and lines for one frame-------------------------
            i = plot_skelet(i, textLine,j);//for 33 landamrks
            // i= plot_skelet_17_landmarks(i, textLine, j);//for 17 landamrks
            
            // i=i+1;
        } 

        // ................plot original skeleton.......................
        if (i_original == lines_original.Length-1)
        {   i_original=0;
            Destroy(linesObject_original);
            previousFrameNumber_original = 0;
            print("33333_original");
            Debug.Log("Landmark not found");
           
        }
        else
        if (i_original < lines_original.Length)
        {
           
            // erase all lines for an old frame
            Destroy(linesObject_original);
            // Destroy(linesObject);

            // plot all lines for new frame
            linesObject_original = new GameObject("Lines");
            
            j_original = 0;

            // print("222221111");
            textLine_original = lines_original[i_original];

            previousFrameNumber_original = FindFrameNumber(textLine_original);
            // print(textLine_original + "^^^^^^^^^^^^^^^^^" + previousFrameNumber + "^^^");


            // plot landmark and lines for one frame---------------------------@@@@@@@@@@@2In khat zir ro bad az capture video az comment kharej konam@@@@@
            i_original = plot_skelet_original(i_original, textLine_original, j_original);

            // i_original=i_original+1;
        }

    }

    // plot skeleton of the predicted animation-----------------------
    int  plot_skelet(int ii,string textlinee,int j)
    {
        // set default value for the list of  landmarksVectors with Vector3 type with muximum length of 32
        Vector3[] LandmaksVectors = new Vector3[33];

        while(FindFrameNumber(textlinee) == previousFrameNumber && ii<lines.Length)
        {
            // if (ii == lines.Length)
            // {
            //     Debug.Log("Landmark not found");
            //     if (EditorApplication.isPlaying)
            //     {
            //         EditorApplication.isPlaying = false;
            //     }
            // }
            // else if (ii<lines.Length)
            // {
            // float myFloatNum = -1.3f;
            textlinee = lines[ii];
            // float z = ExtractOneVector(textlinee)[0];
            // float x = ExtractOneVector(textlinee)[1];
            // float y = ExtractOneVector(textlinee)[2];//+ myFloatNum;
            float z = ExtractOneVector(textlinee)[0];
            float y = - ExtractOneVector(textlinee)[1];
            float x = ExtractOneVector(textlinee)[2];//+ myFloatNum;


            print("Landmark ["+FindLandmarkNumber(textlinee)+"]"+"frame number:["+FindFrameNumber(textlinee)+"]");

            switch (FindLandmarkNumber(textlinee))
            {
                    case 0:
                        // landmarkName = "Nose";
                        LandmaksVectors[0] = new Vector3(x,y,z);
                        break;
                    case 1:
                        // landmarkName = "Left Eye  inner";
                        LandmaksVectors[1] = new Vector3(x, y, z);
                        break;
                    case 2:
                        // landmarkName = "Left Eye";
                        LandmaksVectors[2] = new Vector3(x, y, z);
                        break;
                    case 3:
                        // landmarkName = "Left Eye outer";
                        LandmaksVectors[3] = new Vector3(x, y, z);
                        break;
                    case 4:
                        // landmarkName = "Right Eye inner";
                        LandmaksVectors[4] = new Vector3(x, y, z);
                        break;
                    case 5:
                        // landmarkName = "Right Eye";
                        LandmaksVectors[5] = new Vector3(x, y, z);
                        break;
                    case 6:
                        // landmarkName = "Right Eye outer";
                        LandmaksVectors[6] = new Vector3(x, y, z);
                        break;
                    case 7:
                        // landmarkName = "Left Ear";
                        LandmaksVectors[7] = new Vector3(x, y, z);
                        break;
                    case 8:
                        // landmarkName = "Right Ear";
                        LandmaksVectors[8] = new Vector3(x, y, z);
                        break;
                    case 9:
                        // landmarkName = "mouth Left";
                        LandmaksVectors[9] = new Vector3(x, y, z);
                        break;
                    case 10:
                        // landmarkName = "mouth Right";
                        LandmaksVectors[10] = new Vector3(x, y, z);
                        break;
                    case 11:
                        // landmarkName = "Left Shoulder";
                        LandmaksVectors[11] = new Vector3(x, y, z);
                        break;
                    case 12:
                        // landmarkName = "Right Shoulder";
                        LandmaksVectors[12] = new Vector3(x, y, z);
                        break;
                    case 13:
                        // landmarkName = "Left Elbow";
                        LandmaksVectors[13] = new Vector3(x, y, z);
                        break;
                    case 14:
                        // landmarkName = "Right Elbow";
                        LandmaksVectors[14] = new Vector3(x, y, z);
                        break;
                    case 15:
                        // landmarkName = "Left Wrist";   
                        LandmaksVectors[15] = new Vector3(x, y, z);
                        break;
                    case 16:
                        // landmarkName = "Right Wrist";
                        LandmaksVectors[16] = new Vector3(x, y, z);
                        break;
                    case 17:
                        // landmarkName = "Left pinky knuckle#1";
                        LandmaksVectors[17] = new Vector3(x, y, z);
                        break;
                    case 18:
                        // landmarkName = "Right pinky knuckle#1";
                        LandmaksVectors[18] = new Vector3(x, y, z);
                        break;
                    case 19:
                        // landmarkName = "Left index knuckle#1";
                        LandmaksVectors[19] = new Vector3(x, y, z);
                        break;
                    case 20:
                        // landmarkName = "Right index knuckle#1";
                        LandmaksVectors[20] = new Vector3(x, y, z);
                        break;
                    case 21:
                        // landmarkName = "Left thumb knuckle#2";
                        LandmaksVectors[21] = new Vector3(x, y, z);
                        break;
                    case 22:
                        // landmarkName = "Right thumb knuckle#2";
                        LandmaksVectors[22] = new Vector3(x, y, z);
                        break;
                    case 23:
                        // landmarkName = "Left hip";
                        LandmaksVectors[23] = new Vector3(x, y, z);
                        break;
                    case 24:
                        // landmarkName = "Right hip";
                        LandmaksVectors[24] = new Vector3(x, y, z);
                        break;
                    case 25:
                        // landmarkName = "Left knee";
                        LandmaksVectors[25] = new Vector3(x, y, z);
                        break;
                    case 26:
                        // landmarkName = "Right knee";
                        LandmaksVectors[26] = new Vector3(x, y, z);
                        break;
                    case 27:
                        // landmarkName = "Left ankle";
                        LandmaksVectors[27] = new Vector3(x, y, z);
                        break;
                    case 28:
                        // landmarkName = "Right ankle";
                        LandmaksVectors[28] = new Vector3(x, y, z);
                        break;
                    case 29:
                        // landmarkName = "Left heel";
                        LandmaksVectors[29] = new Vector3(x, y, z);
                        break;
                    case 30:
                        // landmarkName = "Right heel";
                        LandmaksVectors[30] = new Vector3(x, y, z);
                        break;
                    case 31:
                        // landmarkName = "Left foot index";
                        LandmaksVectors[31] = new Vector3(x, y, z);
                        break;
                    case 32:
                        // landmarkName = "Right foot index";
                        LandmaksVectors[32] = new Vector3(x, y, z);
                        break;
                    default:
                        // landmarkName = "Unknown";
                        UnityEditor.EditorApplication.isPlaying = false;
                        break;
              

                }

            print("====" + ii);          
            ii=ii+1;
            
        }
        // bayad ye tanzimi bezaram k age vestori meqdar nadasht bayad circle landmark toye sahne ham unvisible kone
        NosePosition.position = LandmaksVectors[0];
        print("------"+NosePosition.position);
        LeftEyeInnerPosition.position = LandmaksVectors[1];
        LeftEyePosition.position = LandmaksVectors[2];
        LeftEyeOuterPosition.position = LandmaksVectors[3];
        RightEyeInnerPosition.position = LandmaksVectors[4];
        RightEyePosition.position = LandmaksVectors[5];
        RightEyeOuterPosition.position = LandmaksVectors[6];
        LeftEarPosition.position = LandmaksVectors[7];
        RightEarPosition.position = LandmaksVectors[8];
        MouthLeftPosition.position = LandmaksVectors[9];
        MouthRightPosition.position = LandmaksVectors[10];
        LeftShoulderPosition.position = LandmaksVectors[11];
        RightShoulderPosition.position = LandmaksVectors[12];
        LeftElbowPosition.position = LandmaksVectors[13];
        RightElbowPosition.position = LandmaksVectors[14];
        LeftWristPosition.position = LandmaksVectors[15];
        RightWristPosition.position = LandmaksVectors[16];
        LeftPinkyKnucklePosition.position = LandmaksVectors[17];
        RightPinkyKnucklePosition.position = LandmaksVectors[18];
        LeftIndexKnucklePosition.position = LandmaksVectors[19];
        RightIndexKnucklePosition.position = LandmaksVectors[20];
        LeftThumbknucklePosition.position = LandmaksVectors[21];
        RightThumbknucklePosition.position = LandmaksVectors[22];
        LeftHipPosition.position = LandmaksVectors[23];
        RightHipPosition.position = LandmaksVectors[24];
        LeftKneePosition.position = LandmaksVectors[25];
        RightKneePosition.position = LandmaksVectors[26];
        LeftAnklePosition.position = LandmaksVectors[27];
        RightAnklePosition.position = LandmaksVectors[28];
        LeftHeelPosition.position = LandmaksVectors[29];
        RightHeelPosition.position = LandmaksVectors[30];
        LeftFootIndexPosition.position = LandmaksVectors[31];
        RightFootIndexPosition.position = LandmaksVectors[32];
        print("-----"+LandmaksVectors);

        // Debug.DrawLine(NosePosition.position,LeftFootIndexPosition.position,Color.red);
        // print(FindFrameNumber(textLine)); 
        // Application.Quit();

        // Vector3 pointA = LandmaksVectors[j];
        // Vector3 pointB = LandmaksVectors[j + 1];
        
        Draw_Line(NosePosition.position,LeftEyeInnerPosition.position,j);
        Draw_Line( LeftEyeInnerPosition.position,LeftEyePosition.position, j+1);
        Draw_Line( LeftEyePosition.position,LeftEyeOuterPosition.position, j+2);
        Draw_Line( LeftEyeOuterPosition.position,LeftEarPosition.position, j+3);
        Draw_Line( NosePosition.position,RightEyeInnerPosition.position, j+4);
        Draw_Line( RightEyeInnerPosition.position,RightEyePosition.position, j+5);
        Draw_Line( RightEyePosition.position,RightEyeOuterPosition.position, j+6);
        Draw_Line( RightEyeOuterPosition.position,RightEarPosition.position, j+7);
        Draw_Line( MouthLeftPosition.position,MouthRightPosition.position, j+8);
        Draw_Line( LeftShoulderPosition.position,RightShoulderPosition.position, j+9);
        Draw_Line( LeftShoulderPosition.position,LeftElbowPosition.position, j+10);
        Draw_Line( LeftElbowPosition.position,LeftWristPosition.position, j+11);
        Draw_Line( LeftWristPosition.position,LeftPinkyKnucklePosition.position, j+12);
        Draw_Line( LeftWristPosition.position,LeftIndexKnucklePosition.position, j+13);
        Draw_Line( LeftWristPosition.position,LeftThumbknucklePosition.position, j+14);
        Draw_Line( RightShoulderPosition.position,RightElbowPosition.position, j+15);
        Draw_Line( RightElbowPosition.position,RightWristPosition.position, j+16);
        Draw_Line( RightWristPosition.position,RightPinkyKnucklePosition.position, j+17);
        Draw_Line( RightWristPosition.position,RightIndexKnucklePosition.position, j+18);
        Draw_Line( RightWristPosition.position,RightThumbknucklePosition.position, j+19);
        Draw_Line( LeftShoulderPosition.position,LeftHipPosition.position, j+20);
        Draw_Line( RightShoulderPosition.position,RightHipPosition.position, j+21);
        Draw_Line( LeftHipPosition.position,RightHipPosition.position, j+22);
        Draw_Line( LeftHipPosition.position,LeftKneePosition.position, j+23);
        Draw_Line( LeftKneePosition.position,LeftAnklePosition.position, j+24);
        Draw_Line( LeftAnklePosition.position,LeftHeelPosition.position, j+25);
        Draw_Line( LeftAnklePosition.position,LeftFootIndexPosition.position, j+26);
        Draw_Line( RightHipPosition.position,RightKneePosition.position, j+27);
        Draw_Line( RightKneePosition.position,RightAnklePosition.position, j+28);
        Draw_Line( RightAnklePosition.position,RightHeelPosition.position, j+29);
        Draw_Line( RightAnklePosition.position,RightFootIndexPosition.position, j+30);
        


        ii=ii-1;
        return ii;
    }


    // plot skeleton of the Original animation-----------------------
    int plot_skelet_original(int ii_original, string textline_original, int j_original)
    {
        // set default value for the list of  landmarksVectors with Vector3 type with muximum length of 32
        Vector3[] LandmaksVectors_original = new Vector3[15];
        print("0000-FrameNum="+FindFrameNumber(textline_original));
        print("1111-previousFrameNumber="+previousFrameNumber_original);
        print("2222-ii="+ii_original);
        print("3333-lines.Length="+lines_original.Length);
       
        while (FindFrameNumber(textline_original) == previousFrameNumber_original && ii_original < lines_original.Length)
        {
            // if (ii == lines.Length)
            // {
            //     Debug.Log("Landmark not found");
            //     if (EditorApplication.isPlaying)
            //     {
            //         EditorApplication.isPlaying = false;
            //     }
            // }
            // else if (ii<lines.Length)
            // {
            // float myFloatNum = -1.3f;
            textline_original = lines_original[ii_original];
            float x_original = ExtractOneVector(textline_original)[0];
            float y_original = ExtractOneVector(textline_original)[1];
            float z_original = ExtractOneVector(textline_original)[2];//+ myFloatNum;
            print("4444x,y,z="+x_original+","+y_original+","+z_original);

            // print("Landmark [" + FindLandmark_Original(textlinee_original) + "]" + "frame number:[" + FindFrameNumber(textlinee_original) + "]");
 
            print("555-Landmark="+FindLandmark_Original(textline_original));
            switch (FindLandmark_Original(textline_original))
            {
                case "mixamorig2:HeadTop_End":
                    // landmarkName = "Head";
                    LandmaksVectors_original[0] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:Spine2":
                    // landmarkName = "Spin2";
                    LandmaksVectors_original[1] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:LeftShoulder":
                    // landmarkName = "Left Shoulder";
                    LandmaksVectors_original[2] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:RightShoulder":
                    // landmarkName = "Right Shoulder";
                    LandmaksVectors_original[3] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:LeftForeArm":
                    // landmarkName = "Left Elbow";
                    LandmaksVectors_original[4] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:RightForeArm":
                    // landmarkName = "Right Elbow";
                    LandmaksVectors_original[5] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:LeftHand":
                    // landmarkName = "Left hand";   
                    LandmaksVectors_original[6] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:RightHand":
                    // landmarkName = "Right hand";
                    LandmaksVectors_original[7] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:Hips":
                    // landmarkName = "Hips";
                    LandmaksVectors_original[8] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:LeftUpLeg":
                    // landmarkName = "Left hip";
                    LandmaksVectors_original[9] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:RightUpLeg":
                    // landmarkName = "Right hip";
                    LandmaksVectors_original[10] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:LeftLeg":
                    // landmarkName = "Left knee";
                    LandmaksVectors_original[11] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:RightLeg":
                    // landmarkName = "Right knee";
                    LandmaksVectors_original[12] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:LeftFoot":
                    // landmarkName = "Left ankle";
                    LandmaksVectors_original[13] = new Vector3(x_original, y_original, z_original);
                    break;
                case "mixamorig2:RightFoot":
                    // landmarkName = "Right ankle";
                    LandmaksVectors_original[14] = new Vector3(x_original, y_original, z_original);
                    break;
                default:
                    // landmarkName = "Unknown";
                    UnityEditor.EditorApplication.isPlaying = false;
                    break;


            }

        // //    print("====" + ii_original);
            ii_original = ii_original + 1;

        }
        // bayad ye tanzimi bezaram k age vestori meqdar nadasht bayad circle landmark toye sahne ham unvisible kone
        HeadTop_End.position = LandmaksVectors_original[0];
        print("------" + HeadTop_End.position);
        Spine2.position = LandmaksVectors_original[1];
        LeftShoulder.position = LandmaksVectors_original[2];
        RightShoulder.position = LandmaksVectors_original[3];
        LeftElbow.position = LandmaksVectors_original[4];
        RightElbow.position = LandmaksVectors_original[5];
        LeftHand.position = LandmaksVectors_original[6];
        RightHand.position = LandmaksVectors_original[7];
        Hips.position = LandmaksVectors_original[8];
        LeftHip.position = LandmaksVectors_original[9];
        RightHip.position = LandmaksVectors_original[10];
        LeftKnee.position = LandmaksVectors_original[11];
        RightKnee.position = LandmaksVectors_original[12];
        LeftAnkle.position = LandmaksVectors_original[13];
        RightAnkle.position = LandmaksVectors_original[14];
        
        // print("-----" + LandmaksVectors_original);

        // Debug.DrawLine(NosePosition.position,LeftFootIndexPosition.position,Color.red);
        // print(FindFrameNumber(textLine)); 
        // Application.Quit();

        // Vector3 pointA = LandmaksVectors[j];
        // Vector3 pointB = LandmaksVectors[j + 1];

        Draw_Line_Original(HeadTop_End.position, Spine2.position, j_original);
        Draw_Line_Original(Spine2.position, Hips.position, j_original + 1);
        Draw_Line_Original(LeftHand.position, LeftElbow.position, j_original + 2);
        Draw_Line_Original(LeftElbow.position, LeftShoulder.position, j_original + 3);
        Draw_Line_Original(LeftShoulder.position, Spine2.position, j_original + 4);
        Draw_Line_Original(Spine2.position, RightShoulder.position, j_original + 5);
        Draw_Line_Original(RightShoulder.position, RightElbow.position, j_original + 6);
        Draw_Line_Original(RightElbow.position, RightHand.position, j_original + 7);
        Draw_Line_Original(LeftAnkle.position,LeftKnee.position, j_original + 8);
        Draw_Line_Original(LeftKnee.position, LeftHip.position, j_original + 9);
        Draw_Line_Original(LeftHip.position, Hips.position, j_original + 10);
        Draw_Line_Original(Hips.position, RightHip.position, j_original + 11);
        Draw_Line_Original(RightHip.position, RightKnee.position, j_original + 12);
        Draw_Line_Original(RightKnee.position, RightAnkle.position, j_original + 13);
        

        // ii_original = ii_original- 1;
        return ii_original;
       
    }

    // plot skeleton of the Original animation-----------------------
    // int plot_skelet_17_landmarks(int ii, string textlinee, int j)
    // {
    //     // set default value for the list of  landmarksVectors with Vector3 type with muximum length of 32
    //     Vector3[] LandmaksVectors = new Vector3[16];

    //     while (FindFrameNumber(textlinee) == previousFrameNumber && ii < lines.Length)
    //     {
    //         textlinee = lines[ii];
    //         float z = ExtractOneVector(textlinee)[0];
    //         float x = ExtractOneVector(textlinee)[1];
    //         float y = ExtractOneVector(textlinee)[2];//+ myFloatNum;

    //         print("Landmark [" + FindLandmarkNumber(textlinee) + "]" + "frame number:[" + FindFrameNumber(textlinee) + "]");

    //         switch (FindLandmarkNumber(textlinee))
    //         {
    //             case 9:
    //                 // landmarkName = "Nose_head_top";
    //                 LandmaksVectors[0] = new Vector3(x, y, z);
    //                 break;
    //             case 13:
    //                 // landmarkName = "Left Shoulder";
    //                 LandmaksVectors[11] = new Vector3(x, y, z);
    //                 break;
    //             case 12:
    //                 // landmarkName = "Right Shoulder";
    //                 LandmaksVectors[12] = new Vector3(x, y, z);
    //                 break;
    //             case 14:
    //                 // landmarkName = "Left Elbow";
    //                 LandmaksVectors[13] = new Vector3(x, y, z);
    //                 break;
    //             case 11:
    //                 // landmarkName = "Right Elbow";
    //                 LandmaksVectors[14] = new Vector3(x, y, z);
    //                 break;
    //             case 15:
    //                 // landmarkName = "Left Wrist";   
    //                 LandmaksVectors[15] = new Vector3(x, y, z);
    //                 break;
    //             case 10:
    //                 // landmarkName = "Right Wrist";
    //                 LandmaksVectors[16] = new Vector3(x, y, z);
    //                 break;
    //             case 3:
    //                 // landmarkName = "Left hip";
    //                 LandmaksVectors[23] = new Vector3(x, y, z);
    //                 break;
    //             case 2:
    //                 // landmarkName = "Right hip";
    //                 LandmaksVectors[24] = new Vector3(x, y, z);
    //                 break;
    //             case 4:
    //                 // landmarkName = "Left knee";
    //                 LandmaksVectors[25] = new Vector3(x, y, z);
    //                 break;
    //             case 1:
    //                 // landmarkName = "Right knee";
    //                 LandmaksVectors[26] = new Vector3(x, y, z);
    //                 break;
    //             case 5:
    //                 // landmarkName = "Left ankle";
    //                 LandmaksVectors[27] = new Vector3(x, y, z);
    //                 break;
    //             case 0:
    //                 // landmarkName = "Right ankle";
    //                 LandmaksVectors[28] = new Vector3(x, y, z);
    //                 break;
                
    //             case 16:
    //                 // landmarkName = "chest_between_shoulder";
    //                 LandmaksVectors[31] = new Vector3(x, y, z);
    //                 break;
    //             case 6:
    //                 // landmarkName = "pelvix_between_hip";
    //                 LandmaksVectors[32] = new Vector3(x, y, z);
    //                 break;
    //             case 7:
    //                 // landmarkName = "thorax";
    //                 LandmaksVectors[32] = new Vector3(x, y, z);
    //                 break;
    //             case 8:
    //                 // landmarkName = "upper_neck";
    //                 LandmaksVectors[32] = new Vector3(x, y, z);
    //                 break;
    //             default:
    //                 // landmarkName = "Unknown";
    //                 UnityEditor.EditorApplication.isPlaying = false;
    //                 break;


    //         }

    //         print("====" + ii);
    //         ii = ii + 1;

    //     }
    //     // bayad ye tanzimi bezaram k age vestori meqdar nadasht bayad circle landmark toye sahne ham unvisible kone
    //     NosePosition.position = LandmaksVectors[0];
    //     print("------" + NosePosition.position);
    //     LeftShoulderPosition.position = LandmaksVectors[11];
    //     RightShoulderPosition.position = LandmaksVectors[12];
    //     LeftElbowPosition.position = LandmaksVectors[13];
    //     RightElbowPosition.position = LandmaksVectors[14];
    //     LeftWristPosition.position = LandmaksVectors[15];
    //     RightWristPosition.position = LandmaksVectors[16];
    //     LeftHipPosition.position = LandmaksVectors[23];
    //     RightHipPosition.position = LandmaksVectors[24];
    //     LeftKneePosition.position = LandmaksVectors[25];
    //     RightKneePosition.position = LandmaksVectors[26];
    //     LeftAnklePosition.position = LandmaksVectors[27];
    //     RightAnklePosition.position = LandmaksVectors[28];
        
    //     print("-----" + LandmaksVectors);

    //     // Debug.DrawLine(NosePosition.position,LeftFootIndexPosition.position,Color.red);
    //     // print(FindFrameNumber(textLine)); 
    //     // Application.Quit();

    //     // Vector3 pointA = LandmaksVectors[j];
    //     // Vector3 pointB = LandmaksVectors[j + 1];

    //     Draw_Line(NosePosition.position, LeftEyeInnerPosition.position, j);
    //     Draw_Line(LeftEyeInnerPosition.position, LeftEyePosition.position, j + 1);
    //     Draw_Line(LeftEyePosition.position, LeftEyeOuterPosition.position, j + 2);
    //     Draw_Line(LeftEyeOuterPosition.position, LeftEarPosition.position, j + 3);
    //     Draw_Line(NosePosition.position, RightEyeInnerPosition.position, j + 4);
    //     Draw_Line(RightEyeInnerPosition.position, RightEyePosition.position, j + 5);
    //     Draw_Line(RightEyePosition.position, RightEyeOuterPosition.position, j + 6);
    //     Draw_Line(RightEyeOuterPosition.position, RightEarPosition.position, j + 7);
    //     Draw_Line(MouthLeftPosition.position, MouthRightPosition.position, j + 8);
    //     Draw_Line(LeftShoulderPosition.position, RightShoulderPosition.position, j + 9);
    //     Draw_Line(LeftShoulderPosition.position, LeftElbowPosition.position, j + 10);
    //     Draw_Line(LeftElbowPosition.position, LeftWristPosition.position, j + 11);
    //     Draw_Line(LeftWristPosition.position, LeftPinkyKnucklePosition.position, j + 12);
    //     Draw_Line(LeftWristPosition.position, LeftIndexKnucklePosition.position, j + 13);
    //     Draw_Line(LeftWristPosition.position, LeftThumbknucklePosition.position, j + 14);
    //     Draw_Line(RightShoulderPosition.position, RightElbowPosition.position, j + 15);
    //     Draw_Line(RightElbowPosition.position, RightWristPosition.position, j + 16);
    //     Draw_Line(RightWristPosition.position, RightPinkyKnucklePosition.position, j + 17);
    //     Draw_Line(RightWristPosition.position, RightIndexKnucklePosition.position, j + 18);
    //     Draw_Line(RightWristPosition.position, RightThumbknucklePosition.position, j + 19);
    //     Draw_Line(LeftShoulderPosition.position, LeftHipPosition.position, j + 20);
    //     Draw_Line(RightShoulderPosition.position, RightHipPosition.position, j + 21);
    //     Draw_Line(LeftHipPosition.position, RightHipPosition.position, j + 22);
    //     Draw_Line(LeftHipPosition.position, LeftKneePosition.position, j + 23);
    //     Draw_Line(LeftKneePosition.position, LeftAnklePosition.position, j + 24);
    //     Draw_Line(LeftAnklePosition.position, LeftHeelPosition.position, j + 25);
    //     Draw_Line(LeftAnklePosition.position, LeftFootIndexPosition.position, j + 26);
    //     Draw_Line(RightHipPosition.position, RightKneePosition.position, j + 27);
    //     Draw_Line(RightKneePosition.position, RightAnklePosition.position, j + 28);
    //     Draw_Line(RightAnklePosition.position, RightHeelPosition.position, j + 29);
    //     Draw_Line(RightAnklePosition.position, RightFootIndexPosition.position, j + 30);



    //     ii = ii - 1;
    //     return ii;

    // }


    // Find Frame Number for predicted and original landmarks
    static int FindFrameNumber(string textLinee)
    {
        //---- string textLine = "Landmark [10,29]: (-0.26858606934547424, -0.19701218605041504, -0.7247674465179443)";
        // Landmark[0, mixamorig2: LeftHand]: (-1.754781, 2.062487, -0.1283437)

        int startIndex = textLinee.IndexOf('[') + 1;
        int endIndex = textLinee.IndexOf(',');

        string FrameNum = textLinee.Substring(startIndex, endIndex - startIndex);
        // print(">>>>>>>"+FrameNum);
        int FrameNumber = int.Parse(FrameNum);

        return FrameNumber;
    }

    // Read LANDMARK Number for predicted landmarks
    static int FindLandmarkNumber(string textLine)
    {
        int startIndex = textLine.IndexOf(',') + 1;
        int endIndex = textLine.IndexOf(']');

        string secondString = textLine.Substring(startIndex, endIndex - startIndex);
        // convert string to int
        int landmarkNumber = int.Parse(secondString);
        return landmarkNumber;
    }

    // Read LANDMARK Name for original landmarks
    static String FindLandmark_Original(string textline_original)
    {
        int startIndex = textline_original.IndexOf(',') + 2;
        int endIndex = textline_original.IndexOf(']');

        string secondString = textline_original.Substring(startIndex, endIndex - startIndex);
        print("666-"+secondString);
        // convert string to int
        string landmarkNumber = secondString;
        return landmarkNumber;
    }

    //Read Each 3D Vector
    static float[] ExtractOneVector(string textLine)
    {
        int startIndex = textLine.IndexOf('(') + 1;
        int endIndex = textLine.IndexOf(')');

        string valuesString = textLine.Substring(startIndex, endIndex - startIndex);
        string[] valuesArray = valuesString.Split(',');

        float[] values = new float[3];
        values[0] = float.Parse(valuesArray[0]);
        values[1] = float.Parse(valuesArray[1]);
        values[2] = float.Parse(valuesArray[2]);

        return values;
    }

    void Draw_Line(Vector3 pointA,Vector3 pointB,int i)
    {
        // Vector3 pointA = LandmaksVectors[j];
        // Vector3 pointB = LandmaksVectors[j + 1];

        // Create a new game object to hold the line renderer component
        GameObject lineObject = new GameObject("Line " + i);


        // Add the line renderer component to the game object
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // Set the positions of the line renderer's start and end points
        lineRenderer.SetPositions(new Vector3[] { pointA, pointB});
        // lineRenderer.SetPositions(pointA, pointB );

        // Set other properties of the line renderer, such as color and width
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        // ////////////////////////
        // Set the parent of the line object to the lines object, so they will be grouped together
        lineObject.transform.parent = linesObject.transform;
        // lineObject.transform.parent = linesObject_original.transform;
        
    }

    void Draw_Line_Original(Vector3 pointA, Vector3 pointB, int i)
    {
        // Vector3 pointA = LandmaksVectors[j];
        // Vector3 pointB = LandmaksVectors[j + 1];

        // Create a new game object to hold the line renderer component
        GameObject lineObject_original = new GameObject("Line " + i);


        // Add the line renderer component to the game object
        LineRenderer lineRenderer_original = lineObject_original.AddComponent<LineRenderer>();

        // Set the positions of the line renderer's start and end points
        lineRenderer_original.SetPositions(new Vector3[] { pointA, pointB });
        // lineRenderer.SetPositions(pointA, pointB );

        // Set other properties of the line renderer, such as color and width
        lineRenderer_original.startColor = Color.red;
        lineRenderer_original.endColor = Color.red;
        lineRenderer_original.startWidth = 0.02f;
        lineRenderer_original.endWidth = 0.02f;
        // ////////////////////////
        // Set the parent of the line object to the lines object, so they will be grouped together
        lineObject_original.transform.parent = linesObject_original.transform;
        // lineObject.transform.parent = linesObject_original.transform;

    }
   
    
}