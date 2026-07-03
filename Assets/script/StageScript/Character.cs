
using System.Collections;
using Unity.Collections;
using UnityEditor.Timeline;
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
        if (!canMove) return;

        bool began = false;
        bool hold = false;
        bool ended = false;
        Vector2 position = Vector2.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            position = touch.position;

            began = touch.phase == TouchPhase.Began;
            hold = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            ended = touch.phase == TouchPhase.Ended;
        }
        else
        {
            position = Input.mousePosition;

            began = Input.GetMouseButtonDown(0);
            hold = Input.GetMouseButton(0);
            ended = Input.GetMouseButtonUp(0);
        }

        if (began)
            touchStart = position;

        if (hold)
        {
            float distance = Vector2.Distance(touchStart, position);
            Debug.Log(distance);
            if (distance > 100f)
            {
                characterPrefabs.transform.position =
                    findMovePosition(CalVector(position - touchStart), true);
                characterPrefabs.SetActive(true);
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
