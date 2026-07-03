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

    public GameObject pausePanel;
    public GameObject clearPanel;
    public GameObject gameOverPanel;
    public Character character;
    public CountController countController;


    void Start()
    {
        stars = new bool[3]{false, false, false};
        moveCount = 0;
        countController.ChangeCountTMP(moveCount, moveCount_Max);
        countController.ChangeGradeTMP(sGrade,aGrade,bGrade);
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel"))
        {
            character.canMove = false;
            pausePanel.SetActive(true);
        }
    }

    public void Clear()
    {
        character.canMove = false;
        clearPanel.SetActive(true);
        MapDataManager.Instance.UpdateClearResult(stageName,stars,moveCount);
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
