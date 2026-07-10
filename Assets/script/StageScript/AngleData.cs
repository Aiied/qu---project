using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class AngleData : MonoBehaviour
{
    private static readonly float[][][] allAngleData =
    {
        new float[][]
        {
            new float[]{0,0},
            new float[]{0,0},
            new float[]{45,135},
            new float[]{225,315},
            new float[]{135,225},
            new float[]{315,45}
        },
        new float[][]
        {
            new float[]{0,60},
            new float[]{180,240},
            new float[]{60,120},
            new float[]{240,300},
            new float[]{120,180},
            new float[]{300,360}
        },
        new float[][]
        {
            new float[]{315,45},
            new float[]{135,225},
            new float[]{45,135},
            new float[]{225,315},
            new float[]{0,0},
            new float[]{0,0}
        },
        new float[][]
        {
            new float[]{45,135},
            new float[]{225,315},
            new float[]{0,0},
            new float[]{0,0},
            new float[]{135,225},
            new float[]{315,45}
        },
        new float[][]
        {
            new float[]{0,90},
            new float[]{180,270},
            new float[]{0,0},
            new float[]{0,0},
            new float[]{90,180},
            new float[]{270,360}
        },
        new float[][]
        {
            new float[]{315,45},
            new float[]{135,225},
            new float[]{0,0},
            new float[]{0,0},
            new float[]{45,135},
            new float[]{225,315}
        }
    };

    
    

    private static int angleNum = 1;
    private static int position_x = 1;
    private static int position_y = 0;
    private static float[][] angle = allAngleData[4];
   

    void Awake()
    {
        angleNum = 1;
        position_x = 1;
        position_y = 0;
        angle = allAngleData[angleNum];
    }



    public static void UpdateAngle(int value, int position_x_increase, int position_y_increase)
    {
        angleNum = value;
        angle = allAngleData[value];
        position_x = position_x_increase;
        position_y = position_y_increase;
    }

    public static int getAngleNum()
    {
        return angleNum;
    }

    public static int getPosition_x()
    {
        return position_x;
    }

    public static int getPosition_y()
    {
        return position_y;
    }

    public static float[][] GetRangeData()
    {
        return angle;
    }

};
