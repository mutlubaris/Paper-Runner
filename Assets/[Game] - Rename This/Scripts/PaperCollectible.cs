using UnityEngine;

public class PaperCollectible : MonoBehaviour
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
