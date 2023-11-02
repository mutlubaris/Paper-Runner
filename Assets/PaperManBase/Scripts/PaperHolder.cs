using UnityEngine;

public class PaperHolder : MonoBehaviour
{
    [SerializeField] [Range(0,1000)] private int firstPaperAmount = 10;
    [SerializeField] private GameObject paperPrefab = null;
    [SerializeField] private PlayerMover playerMover = null;
    [SerializeField] private float leanMagnitude = 1;
    [SerializeField] private float leanTime = 1;
    [SerializeField] private float paperHeight = 0.015f;
    [SerializeField] private float rotationSensitivity = 0.1f;
    [SerializeField] private float externalForceImpactMultiplier = 30f;
    [SerializeField] private float detachAngleThreshold = 0.6f;

    private GameObject lastObject;
    private Transform currentChild;
    private float paperVectorX;
    private float paperVectorZ;
    private float totalForceZ;
    private float totalForceX;

    public Vector3 ExternalForce;

    private void Start()
    {
        lastObject = gameObject;
        InstantiatePapers(firstPaperAmount);
    }

    private void Update()
    {
        CalculateRotationMagnitude();
        RotatePapers();

        if (Input.GetKeyDown("space") && lastObject != gameObject) DetachTopPaper();
    }

    public void InstantiatePapers(int numberOfPapers)
    {
        Vector3 spawnPos = lastObject.transform.position + Vector3.up * paperHeight / 2;

        for (int i = 0; i < numberOfPapers; i++)
        {
            var newObject = Instantiate(paperPrefab, spawnPos, Quaternion.identity);
            newObject.transform.parent = lastObject.transform;
            lastObject = newObject;
            spawnPos += Vector3.up * paperHeight;
        }
    }
    private void CalculateRotationMagnitude()
    {
        totalForceZ = -ExternalForce.z + playerMover.Acceleration.z;
        totalForceX = -ExternalForce.x + playerMover.Acceleration.x;

        if (Mathf.Abs(paperVectorX - totalForceZ) > rotationSensitivity) paperVectorX += ((paperVectorX > totalForceZ) ? -1 : 1) * Time.deltaTime / leanTime;
        else paperVectorX = totalForceZ;
        if (Mathf.Abs(paperVectorZ - totalForceX) > rotationSensitivity) paperVectorZ += ((paperVectorZ > totalForceX) ? -1 : 1) * Time.deltaTime / leanTime;
        else paperVectorZ = totalForceX;
    }
    private void RotatePapers()
    {
        if (transform.childCount == 0) return;

        currentChild = transform.GetChild(0);
        int forLoopCounter = 1;

        for (int i = 0; i < forLoopCounter; i++)
        {
            currentChild.transform.localRotation = Quaternion.identity;
            if (currentChild.transform.childCount != 0 && currentChild.transform.GetChild(0).childCount != 0)
            {
                currentChild.transform.Rotate(new Vector3(-paperVectorX, 0, paperVectorZ) * leanMagnitude);
                forLoopCounter++;
                currentChild = currentChild.GetChild(0);
            }
            else if (currentChild.transform.childCount != 0)
            {
                currentChild.transform.Rotate(new Vector3(-paperVectorX + ((-playerMover.NewSpeed.z + ExternalForce.z * externalForceImpactMultiplier) * Random.Range(0.1f, 0.3f)), 0,
                    paperVectorZ + ((playerMover.NewSpeed.x - ExternalForce.x * externalForceImpactMultiplier) * Random.Range(0.1f, 0.3f))) * leanMagnitude);
                forLoopCounter++;
                currentChild = currentChild.GetChild(0);
            }
            else
            {
                currentChild.transform.Rotate(new Vector3(-paperVectorX + ((-playerMover.NewSpeed.z + ExternalForce.z * externalForceImpactMultiplier) * Random.Range(0.3f, 0.5f)), 0,
                    paperVectorZ + ((playerMover.NewSpeed.x - ExternalForce.x * externalForceImpactMultiplier) * Random.Range(0.3f, 0.5f))) * leanMagnitude);
                if (Mathf.Abs(currentChild.transform.rotation.x) > detachAngleThreshold || Mathf.Abs(currentChild.transform.rotation.z) > detachAngleThreshold) DetachTopPaper();
            }
        }
    }
    private void DetachTopPaper()
    {
        var objectToDetach = lastObject;
        lastObject = objectToDetach.transform.parent.gameObject;
        objectToDetach.transform.parent = null;
        var paperMover = objectToDetach.AddComponent<PaperMover>();
        paperMover.InitialSpeed = playerMover.NewSpeed + transform.up * ExternalForce.magnitude * externalForceImpactMultiplier / 2 + ExternalForce * externalForceImpactMultiplier;
        objectToDetach.AddComponent<AutoDestroy>();
    }
}
