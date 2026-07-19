using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int moveCount;

    public int moveCount_Max;

    private bool[] stars;

    public int sGrade;
    public int aGrade;
    public int bGrade;
    public string stageName;


    public Vector3 endPosition;
    public Vector3[] trap;

    public GameObject pausePanel;
    public GameObject clearPanel;
    public GameObject FinalGrade;
    public GameObject gameOverPanel;
    public Character character;
    public CountController countController;


    void Start()
    {
        stars = new bool[3] { false, false, false };
        moveCount = 0;
        countController.ChangeCountTMP(moveCount, moveCount_Max);
        countController.ChangeGradeTMP(sGrade, aGrade, bGrade);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            character.canMove = false;
            pausePanel.SetActive(true);
        }
    }
    public void IsTappred(Vector3 position)
    {
        foreach (Vector3 p in trap)
        {
            if (p == position)
            {
                GameOver();
                return;
            }
        }
    }
    public void Clear()
    {
        character.canMove = false;

        clearPanel.SetActive(true);
        string rank = "F";
        if(moveCount <= sGrade)
        {
            rank = "S";
        }
        else if(moveCount <= aGrade)
        {
            rank = "A";
        }
        else if(moveCount <= bGrade)
        {
            rank = "B";
        }
        MapDataManager.Instance.UpdateClearResult(stageName, stars, moveCount, rank);
        MapData record = MapDataManager.Instance.GetRecord(stageName);
        if (record.Best > sGrade)
        {
            FinalGrade.transform.Find("S").gameObject.SetActive(false);
        }
        if (record.Best > aGrade)
        {
            FinalGrade.transform.Find("A").gameObject.SetActive(false);
        }
        if (record.Best > bGrade)
        {
            FinalGrade.transform.Find("B").gameObject.SetActive(false);
        }
        int count = 0;
        foreach (bool star in record.star)
        {
            if (star) count++;
        }
        if (count < 3)
        {
            FinalGrade.transform.Find("Star3").gameObject.SetActive(false);
        }
        if (count < 2)
        {
            FinalGrade.transform.Find("Star2").gameObject.SetActive(false);
        }
        if (count < 1)
        {
            FinalGrade.transform.Find("Star1").gameObject.SetActive(false);
        }

    }

    public void GameOver()
    {
        character.canMove = false;
        gameOverPanel.SetActive(true);
    }

    public void changeCountUi()
    {
        countController.ChangeCountTMP(moveCount, moveCount_Max);
    }
    public void changeStar(int starId)
    {
        stars[starId] = true;
    }
}
