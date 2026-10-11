using UnityEngine;

public class SlowPickup : MonoBehaviour
{
    private void Awake() {
        Debug.Log("Slow was spawned");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
    }
}
