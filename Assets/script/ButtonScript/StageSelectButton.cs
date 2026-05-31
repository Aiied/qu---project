using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
    public void ChangeStage()
    {
        GameObject target = GameObject.FindWithTag("ClearPanel");
        if (target != null)
        {
            target.SetActive(false);
        }
        SceneManager.LoadScene("Stage1");
    }
}
