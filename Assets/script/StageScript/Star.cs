using UnityEngine;

public class Star : MonoBehaviour
{
    private int starId;
    public void changeStarId(int starId)
    {
        this.starId = starId;
    }
    public int getStarId()
    {
        return starId;
    }
}
