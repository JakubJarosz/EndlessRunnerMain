using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameStats;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Death) {
            SaveGame();
        }
    }


    // -------- Save Logic -----------

    [System.Serializable]
    public class SaveData {
        public List<SaveScore> scoreboardList = new List<SaveScore>();
    }

    public void SaveGame() {
        SaveScore saveObject = GameStats.Instance.Save();
        SaveData initialSaveData = LoadGame();
        SaveData updatedSaveData = HandleScoreboardUpdate(initialSaveData, saveObject);
        string json = JsonUtility.ToJson(updatedSaveData);
        File.WriteAllText(Application.dataPath + "/Saves/saveFile.sav", json);
    }

    public SaveData LoadGame() {
        string path = Application.dataPath + "/Saves/saveFile.sav";

        if (!File.Exists(path)) {
            return new SaveData();
        }
        string json = File.ReadAllText(Application.dataPath + "/Saves/saveFile.sav");
        return JsonUtility.FromJson<SaveData>(json);
    }

    private SaveData HandleScoreboardUpdate(SaveData saveData, SaveScore scoreSave) {
        int maxListCount = 10;
        List<SaveScore> list = saveData.scoreboardList;

        if (list.Count == 0) {
            list.Add(scoreSave);
            return saveData;
        }

        for (int i = 0; i < list.Count; i++) {
            if (scoreSave.score > list[i].score) {
                list.Insert(i, scoreSave);
                break;
            } 
        }

        if (list.Count > maxListCount) {
            list.RemoveAt(list.Count - 1);
        }

        return saveData;
    }
}
