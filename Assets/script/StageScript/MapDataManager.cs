using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

[Serializable]
public class MapData
{
    public string stageId;
    public bool isCleared;
    public bool[] star;
    public int Best;
    public string rank;
}

[Serializable]
public class MapFile
{
    public List<MapData> records = new List<MapData>();
}

public class MapDataManager : MonoBehaviour
{
    
    public static MapDataManager Instance { get; private set; }

    [SerializeField] private List<string> defaultStageIds = new List<string>();

    private readonly Dictionary<string, MapData> recordByStageId = new Dictionary<string, MapData>();
    private string savePath;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "stage_records.json");
        Load();
    }

    public IReadOnlyDictionary<string, MapData> GetAllRecords()
    {
        return recordByStageId;
    }

    public MapData GetRecord(string stageId)
    {
        if (string.IsNullOrWhiteSpace(stageId))
        {
            Debug.LogError("stageId is null or empty.");
            return null;
        }

        if (!recordByStageId.TryGetValue(stageId, out MapData record))
        {
            record = CreateDefaultRecord(stageId);
            recordByStageId.Add(stageId, record);
            Save();
        }
        return record;

    }

    public void UpdateClearResult(string stageId, bool[] star, int time, string rank)
    {
        MapData record = GetRecord(stageId);
        if (record == null)
        {
            return;
        }

        record.isCleared = true;
        if(record.Best >= time)
        {
            record.rank = rank;
        }
        record.Best = Mathf.Min(record.Best, time);
        

        if (star != null)
        {
            int length = Mathf.Min(record.star.Length, star.Length);
            for (int i = 0; i < length; i++)
            {
                if (star[i])
                {
                    record.star[i] = true;
                }
            }
        }
        Save();
    }

    public void ResetAllRecords()
    {
        recordByStageId.Clear();

        for (int i = 0; i < defaultStageIds.Count; i++)
        {
            string stageId = defaultStageIds[i];
            if (string.IsNullOrWhiteSpace(stageId))
            {
                continue;
            }

            recordByStageId[stageId] = CreateDefaultRecord(stageId);
        }

        Save();
    }

    private void Load()
    {
        if (!File.Exists(savePath))
        {
            ResetAllRecords();
            return;
        }

        string json = File.ReadAllText(savePath);
        MapFile recordFile = JsonUtility.FromJson<MapFile>(json);

        recordByStageId.Clear();

        if (recordFile != null && recordFile.records != null)
        {
            foreach (MapData record in recordFile.records)
            {
                if (record == null || string.IsNullOrWhiteSpace(record.stageId))
                {
                    continue;
                }

                if (record.star == null || record.star.Length != 3)
                {
                    record.star = new bool[3];
                }

                recordByStageId[record.stageId] = record;
            }
        }

        EnsureDefaultStagesExist();
    }
    private void Save()
    {
        MapFile recordFile = new MapFile
        {
            records = new List<MapData>(recordByStageId.Values)
        };

        string json = JsonUtility.ToJson(recordFile, true);
        File.WriteAllText(savePath, json);
    }

    private void EnsureDefaultStagesExist()
    {
        for (int i = 0; i < defaultStageIds.Count; i++)
        {
            string stageId = defaultStageIds[i];
            if (string.IsNullOrWhiteSpace(stageId) || recordByStageId.ContainsKey(stageId))
            {
                continue;
            }

            MapData record = CreateDefaultRecord(stageId);
            recordByStageId.Add(stageId, record);
        }
        Save();
    }
    private MapData CreateDefaultRecord(string stageId)
    {
        return new MapData
        {
            stageId = stageId,
            isCleared = false,
            Best = 9999,
            star = new bool[3] { false, false, false },
            rank = "F"
        };
    }


}

