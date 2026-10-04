using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private PlayerDetection detection;
    private PlayerController controller;

    private Animator anim;

    private void Awake() {
        controller = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        anim.SetBool("isGrounded", detection.IsGrounded());
        anim.SetFloat("yVelocity", controller.GetVerticalVelocity());
    }
}
