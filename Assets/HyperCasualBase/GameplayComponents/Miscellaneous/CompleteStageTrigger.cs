using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class CompleteStageTrigger : MonoBehaviour
{

    public bool isSuccess;

    private void OnTriggerEnter(Collider other)
    {
        var playerMover = other.GetComponent<PlayerMover>();
        if (playerMover != null)
        {
            GameManager.Instance.CompilateStage(isSuccess);
            playerMover.MaxSpeedForward = 0;
        }
    }
}
