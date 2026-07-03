using UnityEngine;
using UnityEngine.Profiling;

public class Map : MonoBehaviour
{
    [System.NonSerialized]
    public int mapXSize = 7;
    [System.NonSerialized]
    public int mapYSize = 7;
    [System.NonSerialized]
    public int mapZSize = 7;

    public int[] endPoint;


    public bool[] mapData;

    public GridManager obstacleData;
    public GridManager starData;
    public string stageId;

    public SpawnManager spawnManager;

   
    public void Awake()
    {
        mapData = new bool[mapXSize*mapYSize*mapZSize];
        MapData record = MapDataManager.Instance.GetRecord(stageId);
        foreach(RowData row in obstacleData.rows)
        {
            mapData[row.columns[0] + row.columns[1]*mapXSize + row.columns[2]*mapXSize*mapYSize] = true;
            spawnManager.spawnObstacle(row);
          
        }
        for(int i = 0; i<mapXSize; i++)
        {
            for(int j = 0; j<mapYSize; j++)
            {
                for(int k = 0; k<mapZSize; k++)
                {
                    if(j == 0 || i == mapXSize-1 || k == mapZSize-1)
                    {
                        mapData[i + j*mapXSize + k*mapXSize*mapYSize] = true;
                    }
                }
            }
        }
        mapData[endPoint[0] + mapXSize*endPoint[1] + mapXSize*mapYSize*endPoint[2]] = false;


        for(int i = 0; i<3; i++)
        {
            if(record.star[i] == true)
            {
                spawnManager.spawnEmptyStar(starData.rows[i], i);
            }
            else
            {
                spawnManager.spawnStar(starData.rows[i], i);
            }
        }
        

    } 
    
}





