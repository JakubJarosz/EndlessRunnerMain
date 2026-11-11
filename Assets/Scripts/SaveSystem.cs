using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    private List<int> scoreList;

    private void Awake() {
        scoreList = new List<int>() { 1, 2, 3 };
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            //SaveGame();
            Debug.Log("N was pressed");
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            LoadGame();
        }     
    }

    private void SaveGame() {
        //GameStats.Instance.Save();
        //string data = JsonUtility.ToJson(scoreList);
        //Debug.Log(JsonUtility.ToJson(scoreList));
    }

    private void LoadGame() {

    }
}
