
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
        if (moveCount <= sGrade)
        {
            rank = "S";
        }
        else if (moveCount <= aGrade)
        {
            rank = "A";
        }
        else if (moveCount <= bGrade)
        {
            rank = "B";
        }
        MapDataManager.Instance.UpdateClearResult(stageName, stars, moveCount, rank);
        MapData record = MapDataManager.Instance.GetRecord(stageName);
        if (rank != "F")
            FinalGrade.transform.Find(rank).gameObject.SetActive(true);
        int count = 0;
        foreach (bool star in record.star)
        {
            if (star) count++;
        }
        if (count == 3)
        {
            FinalGrade.transform.Find("Star3").gameObject.SetActive(true);
        }
        else if (count == 2)
        {
            FinalGrade.transform.Find("Star2").gameObject.SetActive(true);
        }
        else if (count == 1)
        {
            FinalGrade.transform.Find("Star1").gameObject.SetActive(true);
        }
        else
        {
            FinalGrade.transform.Find("Star0").gameObject.SetActive(true);
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
