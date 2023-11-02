using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    [SerializeField] private float forceMultiplier = 10f;

    private void OnTriggerStay(Collider other)
    {
        var paperHolder = other.GetComponentInChildren<PaperHolder>();
        if (paperHolder != null) paperHolder.ExternalForce = transform.forward / Vector3.Distance(transform.position, other.transform.position) * forceMultiplier;
    }

    private void OnTriggerExit(Collider other)
    {
        var paperHolder = other.GetComponentInChildren<PaperHolder>();
        if (paperHolder != null) paperHolder.ExternalForce = Vector3.zero;
    }
}
