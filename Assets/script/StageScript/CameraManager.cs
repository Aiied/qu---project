using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector3[] position = new Vector3[6];
    public Vector3[] rotation = new Vector3[6];
    Vector2 touchStart;
    Vector2 touchEnd;
    
     private int[] CalVector(Vector2 delta)
{
    int[] result = { 0, 0 };

    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
    {
        result[1] = delta.x < 0 ? 1 : -1;
    }
    else
    {
        result[0] = delta.y < 0 ? 1 : -1;
    }

    return result;
}

    private void ChangeCamPosition(int[] positionIncrease)
    {
        int position_x = AngleData.getPosition_x();
        int position_y = AngleData.getPosition_y();
        if(position_x + positionIncrease[1] >= 0 && position_x + positionIncrease[1] <= 2)
        {
            position_x += positionIncrease[1];
        }
        if(position_y + positionIncrease[0] >= 0 && position_y + positionIncrease[0] <= 1)
        {
            position_y += positionIncrease[0];
        }
        int camNum = position_x + position_y*3;
        transform.position = position[camNum];
        Debug.Log(camNum);
        transform.rotation = Quaternion.Euler(rotation[camNum]);
        AngleData.UpdateAngle(camNum,position_x,position_y);
    }
    void Update()
    {

        bool began = false;
        bool ended = false;
        

        Vector2 position = Vector2.zero;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            position = touch.position;
            if (position.y < 500)
            {
                began = touch.phase == TouchPhase.Began;
                ended = touch.phase == TouchPhase.Ended;
            }
        }
        else
        {
            position = Input.mousePosition;
            if (position.y < 500)
            {
                began = Input.GetMouseButtonDown(0);
                ended = Input.GetMouseButtonUp(0);
            }
        }

        if (began)
            touchStart = position;

        if (ended)
        {
            float distance = Vector2.Distance(touchStart, position);
            if (distance > 100f)
            {
                touchEnd = position;
                ChangeCamPosition(CalVector(touchEnd - touchStart));

            }
        }
    }
    
}
