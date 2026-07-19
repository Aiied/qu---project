
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Map map;

    public int characterSize;

    public GameManager gameManager;
    public GameObject characterPrefabs;

    public bool canMove = true;

    bool isMoving = false;

    public int gridX;
    public int gridY;
    public int gridZ;

    public float moveSpeed;

    Vector2 touchStart;
    Vector2 touchEnd;

    Vector3 findMovePosition(int[] moveInfor, bool isPreFab)
    {
        int count = 0;
        int dx = moveInfor[0];
        int dy = moveInfor[1];
        int dz = moveInfor[2];

        int curX = gridX;
        int curY = gridY;
        int curZ = gridZ;

        while (true)
        {
            int nextX = curX + dx;
            int nextY = curY + dy;
            int nextZ = curZ + dz;
            if (nextX < 0 || nextX >= map.mapXSize
            || nextY < 0 || nextY >= map.mapYSize
            || nextZ < 0 || nextZ >= map.mapZSize)
            {
                break;
            }
            int checkIndex = nextX + nextY * map.mapXSize + nextZ * map.mapXSize * map.mapYSize;
            if (map.mapData[checkIndex])
            {
                break;
            }
            curX = nextX; curY = nextY; curZ = nextZ;
            count++;
        }
        if (!isPreFab)
        {
            gridX = curX; gridY = curY; gridZ = curZ;
        }
        return new Vector3(
            transform.position.x + (count * dx * characterSize),
            transform.position.y + (count * dy * characterSize),
            transform.position.z + (count * dz * characterSize)
            );
    }

    public void CharacterMove(int[] moveInfor)
    {
        if (isMoving)
        {
            return;
        }

        Vector3 targetPos = findMovePosition(moveInfor, false);

        if (transform.position == targetPos)
        {
            return;
        }
        gameManager.moveCount++;
        StartCoroutine(SmoothMove(targetPos));

    }

    IEnumerator SmoothMove(Vector3 target)
    {
        isMoving = true;
        canMove = false;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
        canMove = true;
        gameManager.changeCountUi();
        gameManager.IsTappred(transform.position);
        if (transform.position == gameManager.endPosition)
        {
            gameManager.Clear();
        }
        if (gameManager.moveCount >= gameManager.moveCount_Max)
        {
            gameManager.GameOver();
        }
        
        
    }

    private int[] CalVector(Vector2 end_start)
{
    float angle = Mathf.Atan2(end_start.y, end_start.x) * Mathf.Rad2Deg;

    int[] returnArr = new int[3];
    float[][] rangeData = AngleData.GetRangeData();

    for (int i = 0; i < 6; i++)
    {
        float start = rangeData[i][0];
        float end = rangeData[i][1];

        if (start == 0 && end == 0)
            continue;

        if (start > 180) start -= 360;
        if (end > 180) end -= 360;

        bool inRange;

        if (start <= end)
        {
            inRange = angle >= start && angle < end;
        }
        else
        {
            inRange = angle >= start || angle < end;
        }

        if (!inRange)
            continue;

        switch (i)
        {
            case 0: returnArr[0] = 1; break;   // +X
            case 1: returnArr[0] = -1; break;  // -X
            case 2: returnArr[1] = 1; break;   // +Y
            case 3: returnArr[1] = -1; break;  // -Y
            case 4: returnArr[2] = 1; break;   // +Z
            case 5: returnArr[2] = -1; break;  // -Z
        }

        break;
    }

    return returnArr;
}
    void Update()
    {
        if (!canMove) return;

        bool began = false;
        bool hold = false;
        bool ended = false;

        Vector2 position = Vector2.zero;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            position = touch.position;
            if (position.y > 400)
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            Star star = other.GetComponent<Star>();
            gameManager.changeStar(star.getStarId());
            Destroy(other.gameObject);
        }
    }

}
