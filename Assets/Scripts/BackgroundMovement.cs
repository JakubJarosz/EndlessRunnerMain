using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    //[SerializeField] private LevelMovement levelMovement;
    private float startingPos;

    void Start()
    {
        startingPos = transform.position.x;
    }

    void FixedUpdate() {
        transform.position += speed * Time.deltaTime * Vector3.left;
    }
}
