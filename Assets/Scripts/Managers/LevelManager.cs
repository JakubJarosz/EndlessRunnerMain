using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject initialPart;
    [SerializeField] private Transform levelParent;
    [SerializeField] private LevelsSO levelSO;

    // level variables
    private int maxLevelParts = 5;
    private GameObject prevAddedPart;

    // SlowPickup variables
    private LevelMovement levelMovement;
    public bool canPickupSpawn { get; private set; }
    private int spawnAfterSlowCount;

    private Queue<GameObject> levelPartsQueue = new Queue<GameObject>();

    private void Awake() {
        levelMovement = levelParent.GetComponent<LevelMovement>();
        canPickupSpawn = true;

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

        // handle adding new part to always have maxLevelParts amount in runtime
        if (levelPartsQueue.Count < maxLevelParts) {
            AddNewPart();

            // handle slowpickup count
            if (levelMovement.currentSpeed >= 12f) {
                spawnAfterSlowCount++;
            }
        }

        // if slowpickup count is higher or equal then x allow for another slowpickup spawn
        // it will spawn after X - 1 parts will spawn 
        if (spawnAfterSlowCount >= 4) {
            canPickupSpawn = true;
        }
    }

    private void AddNewPart() {
        GameObject newPart = levelSO.GetRandomPart(prevAddedPart);
        prevAddedPart = Instantiate(newPart, new Vector3(prevAddedPart.transform.position.x + 50f, -8.9f, 0), Quaternion.identity, levelParent);
        levelPartsQueue.Enqueue(prevAddedPart);

        // Slow pick up logic, if current speed is above X setActive one slow pickup
        if (levelMovement.currentSpeed >= 12f && canPickupSpawn) {
            prevAddedPart.GetComponentInChildren<SlowPickupHandler>().SpawnSlowPickup();
            canPickupSpawn = false;
            spawnAfterSlowCount = 0;
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
