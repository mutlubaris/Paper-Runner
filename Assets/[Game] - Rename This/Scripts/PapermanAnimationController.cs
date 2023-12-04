using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapermanAnimationController : MonoBehaviour
{
    Animator animator;

    PlayerMover playerMover;
    private CapsuleCollider CapsuleCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMover = transform.root.GetComponent<PlayerMover>();
        CapsuleCollider = transform.root.GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        GameManager.Instance.OnStageSuccess.AddListener(Dance);
        GameManager.Instance.OnStageFail.AddListener(Fail);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        GameManager.Instance.OnStageSuccess.RemoveListener(Dance);
        GameManager.Instance.OnStageFail.RemoveListener(Fail);
    }

    private void Update()
    {
        animator.SetFloat("Speed", playerMover.forwardSpeed);
        animator.SetBool("IsGrounded", IsGrounded());
    }

    private void Fail()
    {
        animator.SetTrigger("Fail");
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Dance()
    {
        animator.SetTrigger("Dance");
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * CapsuleCollider.height), Vector3.down);
        RaycastHit hit;
        Debug.DrawRay(transform.position + (Vector3.up * CapsuleCollider.height), Vector3.down, Color.red);
        return Physics.SphereCast(ray, CapsuleCollider.height / 2, out hit, CapsuleCollider.bounds.extents.y + 0.3f);
    }
}
