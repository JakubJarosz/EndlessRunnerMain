using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float amplitude;

    private float startY;

    private void Start() {
        startY = transform.position.y;
    }

    private void OnEnable() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void OnDisable() {
        GameManager.Instance.GameStateChanged -= Instance_GameStateChanged;
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Death) {
            Animator anim = GetComponentInChildren<Animator>();
            anim.enabled = false;
        }
    }

    private void Update() {
        Movement();
    }

    private void OnTriggerEnter2D() {
        GameStats.Instance.AddCoin();
        Destroy(gameObject);
    }

    private void Movement() {
        if (GameManager.Instance.state != GameManager.GameState.Gameplay) return;

        float offset = Mathf.Sin(Time.time * moveSpeed) * amplitude;
        Vector3 pos = transform.position;
        pos.y = startY + offset;
        transform.position = pos;
    }
}
