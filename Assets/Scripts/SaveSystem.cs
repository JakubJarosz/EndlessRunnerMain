using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using static GameStats;

public class SaveSystem : MonoBehaviour
{

    private List<int> scoreList;

    private void Awake() {
        scoreList = new List<int>() { 1, 2, 3 };
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            LoadGame();
        }     
    }

    [System.Serializable]
    public class SaveData {
        public List<SaveScore> scoreboardList;
    }

    private void SaveGame() {
        GameStats.SaveScore saveObject = GameStats.Instance.Save();
        string json = JsonUtility.ToJson(saveObject);
        Debug.Log(json);
    }

    private void LoadGame() {

    }
}
