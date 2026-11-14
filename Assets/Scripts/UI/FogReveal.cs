using UnityEngine;

public class FogReveal : MonoBehaviour {
    [SerializeField] private Material fogMaterial;
    [SerializeField] private float duration;

    private const float fogRevealMin = 0f;
    private const float fogRevealMax = 0.92f;
    private float elapsedTime;
    private bool startReaveal;

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Update() {
        if (startReaveal) {
            if (elapsedTime >= duration) {
                startReaveal = false;
            }
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            float value = Mathf.Lerp(fogRevealMin, fogRevealMax, t);

            fogMaterial.SetFloat("_FogReveal", value);
            Debug.Log(fogMaterial.GetFloat("_FogReveal"));
        }
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Gameplay) {
            startReaveal = true;
        } else {
            startReaveal = false;
            elapsedTime = 0f;
        }
    }
}
