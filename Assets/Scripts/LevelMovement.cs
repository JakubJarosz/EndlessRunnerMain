using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    [SerializeField] private float startingSpeed;

    private void Update() {
        transform.position += startingSpeed * Vector3.left * Time.deltaTime;
    }
}
