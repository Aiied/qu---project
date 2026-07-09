using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector3[] position = new Vector3[6];
    public Vector3[] rotation = new Vector3[6];
    
     private int[] CalVector(Vector2 end_start)
    {
        float acTan = Mathf.Atan2(end_start.y, end_start.x) * Mathf.Rad2Deg;
        int[] returnArr = new int[3] { 0, 0, 0 };
        if (acTan > 0 && acTan <= 60)
        {
            returnArr[0] = 1;
        }
        else if (acTan > 60 && acTan <= 120)
        {
            returnArr[1] = 1;
        }
        else if (acTan > 120 && acTan <= 180)
        {
            returnArr[2] = 1;
        }
        else if (acTan > (-180) && acTan <= (-120))
        {
            returnArr[0] = -1;
        }
        else if (acTan > (-120) && acTan <= (-60))
        {
            returnArr[1] = -1;
        }
        else if (acTan > (-60) && acTan <= 0)
        {
            returnArr[2] = -1;
        }
        return returnArr;

    }
    void Update()
    {

        bool began = false;
        bool hold = false;
        bool ended = false;
        Vector2 touchStart;
        Vector2 touchEnd;

        Vector2 position = Vector2.zero;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            position = touch.position;
            if (position.y < 500)
            {
                began = touch.phase == TouchPhase.Began;
                hold = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
                ended = touch.phase == TouchPhase.Ended;
            }
        }
        else
        {
            position = Input.mousePosition;
            if (position.y > 400)
            {
                began = Input.GetMouseButtonDown(0);
                hold = Input.GetMouseButton(0);
                ended = Input.GetMouseButtonUp(0);
            }
        }

        if (began)
            touchStart = position;

        if (hold)
        {
            float distance = Vector2.Distance(touchStart, position);
            Debug.Log(distance);
            if (distance > 100f)
            {
            }
            else
            {
                characterPrefabs.SetActive(false);
            }
        }

        if (ended)
        {
            float distance = Vector2.Distance(touchStart, position);
            if (distance > 100f)
            {
                touchEnd = position;
                CharacterMove(CalVector(touchEnd - touchStart));
            }
        }
    }
    
}
