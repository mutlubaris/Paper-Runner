using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private void OnTriggerEnter(Collider other)
    {
        var playerMover = other.GetComponent<PlayerMover>();
        if (playerMover != null)
        {
            playerMover.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * jumpForce;
            playerMover.GetComponentInChildren<PapermanAnimationController>().Jump();
        }

    }
}