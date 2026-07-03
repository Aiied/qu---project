using UnityEngine;

public class CloseButton_pause : MonoBehaviour
{
    public GameObject closeView;

    void close()
    {
        closeView.SetActive(false);
        
    }
}
