using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.CompleteGameStateChange(GameManager.GameState.Death);
    }
}
