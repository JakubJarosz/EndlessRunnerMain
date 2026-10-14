using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Death);
    }
}
