using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperAdder : MonoBehaviour
{
    [SerializeField] private int papersToAdd = 10;
    
    private void OnTriggerEnter(Collider other)
    {
        var paperHolder = other.GetComponentInChildren<PaperHolder>();
        if (paperHolder != null)
        {
            paperHolder.InstantiatePapers(papersToAdd);
            Destroy(gameObject);
        }
    }
}
