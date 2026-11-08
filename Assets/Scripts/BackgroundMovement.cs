using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float paralaxMultip;
    private float length;

    private LevelMovement levelMovement;
    void Start()
    {
        levelMovement = LevelMovement.Instance;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate() {
        if (GameManager.Instance.state != GameManager.GameState.Gameplay) return;

        transform.position += Vector3.left * (paralaxMultip * levelMovement.currentSpeed * Time.deltaTime);

        if (transform.position.x < -length) {
            transform.position += Vector3.right * (length * 2f);
        }
    }
}
