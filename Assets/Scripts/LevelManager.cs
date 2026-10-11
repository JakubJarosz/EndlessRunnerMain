using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject initialPart;
    [SerializeField] private Transform levelParent;
    [SerializeField] private LevelsSO levelSO;

    private int maxLevelParts = 5;
    private GameObject prevAddedPart;

    private Queue<GameObject> levelPartsQueue = new Queue<GameObject>();

    private void Awake() {
        for (int i = 0; i < maxLevelParts; i++) {
            if (i == 0) {
                // Enqueue the initial part of the level, the part is already instantiated in the scene, so we just need to add it to the queue
                prevAddedPart = initialPart;
                levelPartsQueue.Enqueue(initialPart);
            } else {
                AddNewPart();
            }
        }
    }

    private void Update() {
        RemovePart();

        if (levelPartsQueue.Count < maxLevelParts) {
            AddNewPart();
        }
    }

    private void AddNewPart() {
        GameObject newPart = levelSO.GetRandomPart(prevAddedPart);
        prevAddedPart = Instantiate(newPart, new Vector3(prevAddedPart.transform.position.x + 50f, -8.9f, 0), Quaternion.identity, levelParent);
        levelPartsQueue.Enqueue(prevAddedPart);

        // Slow pick up logic, if current speed is above X setActive one slow pickup
        if (levelParent.GetComponent<LevelMovement>().currentSpeed >= 13f) {
            prevAddedPart.GetComponentInChildren<SlowPickupHandler>().SpawnSlowPickup();
        }
    }

    private void RemovePart() {
        GameObject part = levelPartsQueue.Peek();
        
        if (part.transform.position.x <= -100f) {
            GameObject partToDestroy = levelPartsQueue.Dequeue();
            //Debug.Log(part);
            Destroy(partToDestroy);
        }
    }
}
