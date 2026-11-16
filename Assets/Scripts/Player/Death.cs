using System.Collections;
using UnityEngine;

public class Death : MonoBehaviour
{
    private BoxCollider2D collider;

    private void Awake() {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Gameplay) {
            StartCoroutine(DelayCollider());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.CompleteGameStateChange(GameManager.GameState.Death);
    }

    private IEnumerator DelayCollider() {
        collider.enabled = false;
        yield return new WaitForSeconds(3f);
        collider.enabled = true;
    }
}
