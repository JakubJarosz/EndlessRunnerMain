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

    private void Start() {
        controller.PerformDashVisual += HandlePerformDashVisual;
        controller.PerformSlideVisual += HandlePerformSlideVisual;      
    }

    private void HandlePerformDashVisual() {
        anim.SetTrigger("dash");
    }

    private void HandlePerformSlideVisual(bool isSliding) {
        anim.SetBool("isSliding", isSliding);
    }

    private void Update() {
        anim.SetBool("isGrounded", detection.IsGrounded());
        anim.SetFloat("yVelocity", controller.GetVerticalVelocity);
    }
}
