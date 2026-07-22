using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    IReadOnlyDictionary<string, MapData> allData;
    void Awake()
    {
        allData = MapDataManager.Instance.GetAllRecords();
        GameObject stage;
        GameObject Grade;
        foreach (MapData data in allData.Values)
        {
            stage = transform.Find(data.stageId).gameObject;
            Grade = stage.transform.Find("Grade").gameObject;
            foreach (Transform child in Grade.transform)
            {
                child.gameObject.SetActive(false);
            }
            if (data.rank != "F")
                Grade.transform.Find(data.rank).gameObject.SetActive(true);
            int Count = 0;
            foreach (bool star in data.star)
            {
                if (star) ++Count;
            }
            Grade.transform.Find(Count.ToString()).gameObject.SetActive(true);


        }

    }
}
