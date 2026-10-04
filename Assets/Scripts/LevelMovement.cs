using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Update() {
        transform.position += speed * Vector3.left * Time.deltaTime;
    }
}
