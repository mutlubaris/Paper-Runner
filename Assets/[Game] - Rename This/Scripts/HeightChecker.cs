using UnityEngine;

public class HeightChecker : MonoBehaviour
{
    private PaperHolder paperStackController;

    private void Start()
    {
        paperStackController = transform.parent.GetComponentInChildren<PaperHolder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Obstacle") return;
        var impactPointMin = other.GetComponent<Collider>().bounds.min.y;
        var impactPointMax = other.GetComponent<Collider>().bounds.max.y;
        paperStackController.CheckPaperStackHeight(impactPointMin, impactPointMax);
    }
}