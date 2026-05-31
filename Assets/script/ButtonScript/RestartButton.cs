using System.Diagnostics;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void reloadScene()
    {
        GameObject target = GameObject.FindWithTag("GameOverPanel");
        if (target != null)
        {
            target.SetActive(false);
        }
        SceneManager.LoadScene("stage1");
    }
}
