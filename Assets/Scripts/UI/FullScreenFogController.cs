using System.Collections;
using UnityEngine;

public class FullScreenFogController : MonoBehaviour
{
    [SerializeField] private Material fogMaterial;

    private const float minFogVisibility = 6.29f;
    private const float maxFogVisibility = 0.15f;

    private const float playerCenterPos = 0f;
    private const float playerDeathPos = -16.5f;

    private float value;
    private bool enableFogLogic = false;

    private void Awake() {
        value = minFogVisibility;
        fogMaterial.SetFloat("_VignettePower", value);
    }

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Update() {
        if (enableFogLogic) {
            float playerCurrentPos = PlayerController.Instance.GetPlayerXPos;
            float t = Mathf.InverseLerp(
            playerDeathPos,
            playerCenterPos,
            playerCurrentPos
        );

            value = Mathf.Lerp(
                maxFogVisibility,
                minFogVisibility,
                t
            );

            fogMaterial.SetFloat("_VignettePower", value);
        }
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Gameplay || obj == GameManager.GameState.Death) {
            StartCoroutine(EnableFogWithDelay());
        } else {
            enableFogLogic = false;
            value = minFogVisibility;
            fogMaterial.SetFloat("_VignettePower", value);
        }
    }

    private IEnumerator EnableFogWithDelay() {
        yield return new WaitForSeconds(3f);
        enableFogLogic = true;
    }
}
