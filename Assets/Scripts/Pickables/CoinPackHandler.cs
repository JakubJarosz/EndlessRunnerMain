using UnityEngine;

public class CoinPackHandler : MonoBehaviour
{
    private void Awake() {
        int randomIndex = Random.Range(0, transform.childCount);

        transform.GetChild(randomIndex).gameObject.SetActive(true);
    }
}
