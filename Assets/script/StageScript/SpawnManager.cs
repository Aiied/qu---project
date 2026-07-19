using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject starPrefab;
    int characterSize = 2;
    public int x_CalibrationValue;
    public int y_CalibrationValue;
    public int z_CalibrationValue;



    public void spawnObstacle(RowData row)
    {
        Vector3 spawnPos = new Vector3(
            row.columns[0] * characterSize + x_CalibrationValue,
            row.columns[1] * characterSize + y_CalibrationValue - characterSize,
            row.columns[2] * characterSize + z_CalibrationValue);
        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }

    public void spawnStar(RowData row, int starId)
    {
        Vector3 spawnPos = new Vector3(
            row.columns[0] * characterSize + x_CalibrationValue,
            row.columns[1] * characterSize + y_CalibrationValue - characterSize,
            row.columns[2] * characterSize + z_CalibrationValue);


        GameObject new_star = Instantiate(starPrefab, spawnPos, Quaternion.identity);

        Star script = new_star.GetComponent<Star>();

        script.changeStarId(starId);

    }
}
