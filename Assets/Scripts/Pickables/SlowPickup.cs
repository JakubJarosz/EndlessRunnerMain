using System;
using UnityEngine;

public class SlowPickup : MonoBehaviour
{
    public static event Action SlowPickupCollected;

    private void OnTriggerEnter2D(Collider2D collision) {
        SlowPickupCollected?.Invoke();
        Destroy(gameObject);
    }
}
