using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using static GameStats;

public class SaveSystem : MonoBehaviour
{
    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            SaveGame();
        }    
    }

    [System.Serializable]
    public class SaveData {
        public List<SaveScore> scoreboardList;
    }

    public void SaveGame() {
        GameStats.SaveScore saveObject = GameStats.Instance.Save();
        SaveData saveData = LoadGame();
        saveData.scoreboardList.Add(saveObject);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + "/Saves/saveFile.sav", json);
        Debug.Log(json);
    }

    public SaveData LoadGame() {
        string json = File.ReadAllText(Application.dataPath + "/Saves/saveFile.sav");
        return JsonUtility.FromJson<SaveData>(json);
    }
}
