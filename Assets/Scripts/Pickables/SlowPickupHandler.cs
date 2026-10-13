using UnityEngine;

public class SlowPickupHandler : MonoBehaviour
{
    public void SpawnSlowPickup() {
        int randomIndex = Random.Range(0, transform.childCount);

        transform.GetChild(randomIndex).gameObject.SetActive(true);
    }


}
