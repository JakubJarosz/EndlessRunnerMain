using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float amplitude;

    private float startY;

    private void Start() {
        startY = transform.position.y;
    }

    private void Update() {
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameStats.Instance.AddCoin();
        Destroy(gameObject);
    }

    private void Movement() {
        float offset = Mathf.Sin(Time.time * moveSpeed) * amplitude;
        Vector3 pos = transform.position;
        pos.y = startY + offset;
        transform.position = pos;
    }
}
