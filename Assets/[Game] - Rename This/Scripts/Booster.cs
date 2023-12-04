using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float boostAmount = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        var playerMover = other.GetComponent<PlayerMover>();
        if (playerMover != null) playerMover.MaxSpeedForward += boostAmount;
    }

    private void OnTriggerExit(Collider other)
    {
        var playerMover = other.GetComponent<PlayerMover>();
        if (playerMover != null) playerMover.MaxSpeedForward -= boostAmount;
    }
}