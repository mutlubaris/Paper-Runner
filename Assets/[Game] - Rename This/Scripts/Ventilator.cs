using UnityEngine;

public class Ventilator : MonoBehaviour
{
    #region Variables
    [SerializeField] private float forceMultiplier = 10f;
    [SerializeField] private bool rotating;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationAngle;

    private float initialRotY;
    private int rotationDir = 1;
    #endregion

    private void Start()
    {
        initialRotY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (!rotating) return;

        transform.Rotate(Vector3.up * rotationDir * Time.deltaTime * rotationSpeed);

        if ((transform.rotation.eulerAngles.y > initialRotY + rotationAngle / 2) || (transform.rotation.eulerAngles.y < initialRotY - rotationAngle / 2))
            rotationDir *= -1;
    }

    private void OnTriggerStay(Collider other)
    {
        var paperHolder = other.transform.parent == null ? other.GetComponentInChildren<PaperHolder>() : other.transform.parent.GetComponentInChildren<PaperHolder>();
        if (paperHolder != null) 
            paperHolder.ExternalForce = ((other.transform.position - transform.position).normalized / 3 - transform.right) / 2 / Vector3.Distance(transform.position, other.transform.position) * forceMultiplier;
    }

    private void OnTriggerExit(Collider other)
    {
        var paperHolder = other.transform.parent == null ? other.GetComponentInChildren<PaperHolder>() : other.transform.parent.GetComponentInChildren<PaperHolder>();
        if (paperHolder != null) 
            paperHolder.ExternalForce = Vector3.zero;
    }
}
