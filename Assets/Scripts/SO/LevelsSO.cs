using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelsSO : ScriptableObject
{
    public List<GameObject> levelparts;

    public GameObject GetRandomPart(GameObject prev) {

        int prevIndex = levelparts.FindIndex(
        prefab => prefab.name == prev.name.Replace("(Clone)", "").Trim()
    );

        int randomIndex = Random.Range(0, levelparts.Count - 1);

    if (prevIndex == -1)
    {
        return levelparts[Random.Range(0, levelparts.Count)];
    }

    if (randomIndex >= prevIndex)
    {
            randomIndex++;
    }

        return levelparts[randomIndex];
    }
}
