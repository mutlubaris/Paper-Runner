using System;
using UnityEngine;

public class PaperHolder : MonoBehaviour
{
    #region Variables
    [SerializeField] [Range(0, 1000)] private int firstPaperAmount = 10;
    [SerializeField] private GameObject[] paperPrefabs = null;
    [SerializeField] private PlayerMover playerMover = null;
    [SerializeField] private float paperLeanMagnitude = 1;
    [SerializeField] private float paperLeanTime = 1;
    [SerializeField] private float gapBetweenPapers = 0.015f;
    [SerializeField] private float externalForceImpactMultiplier = 30f;
    [SerializeField] private float accelerationImpactMultiplier = .5f;
    [SerializeField] private float speedImpactMultiplier = .01f;
    [SerializeField] private float singlePaperDetachAngle = 0.4f;
    [SerializeField] private float multiplePaperDetachAngle = 0.6f;
    [SerializeField] private float paperAngleRandomness = 5f;
    [SerializeField] private float paperSpeedAfterCollision = 3f;
    
    private Transform detachedPapersTransform;
    private GameObject lastObject;
    private Transform currentChild;
    private float paperVectorX;
    private float paperVectorZ;
    private float totalForceZ;
    private float totalForceX;
    private int totalPaperAmount;
    private PaperCounterPanel paperCounterPanel;
    private PaperManCameraController paperManCameraController;

    [HideInInspector] public Vector3 ExternalForce;
    #endregion

    private void Start()
    {
        paperManCameraController = FindObjectOfType<PaperManCameraController>();
        lastObject = gameObject;
        CreateDetachedPaperHolder();
        InstantiatePapers(firstPaperAmount);
    }

    private void Update()
    {
        CalculateRotationMagnitude();
        RotatePapers();
        UpdatePanel();
    }

    private void CreateDetachedPaperHolder()
    {
        var newObject = new GameObject();
        newObject.name = "Detached Papers";
        detachedPapersTransform = newObject.transform;
    }
    private void CalculateRotationMagnitude()
    {
        totalForceZ = -ExternalForce.z + playerMover.Acceleration.z * accelerationImpactMultiplier + playerMover.Speed.z * speedImpactMultiplier;
        totalForceX = -ExternalForce.x + playerMover.Acceleration.x * accelerationImpactMultiplier + playerMover.Speed.x * speedImpactMultiplier;

        if (Mathf.Abs(paperVectorX - totalForceZ) > 0)
        {
            int additionSign = ((paperVectorX > totalForceZ) ? -1 : 1);
            paperVectorX += additionSign * Time.deltaTime / paperLeanTime;
            if (additionSign != ((paperVectorX > totalForceZ) ? -1 : 1)) paperVectorX = totalForceZ;
        }
        else paperVectorX = totalForceZ;

        if (Mathf.Abs(paperVectorZ - totalForceX) > 0)
        {
            int additionSign = ((paperVectorZ > totalForceX) ? -1 : 1);
            paperVectorZ += additionSign * Time.deltaTime / paperLeanTime;
            if (additionSign != ((paperVectorZ > totalForceX) ? -1 : 1)) paperVectorZ = totalForceX;
        }
        else paperVectorZ = totalForceX;
    }
    private void RotatePapers()
    {
        if (transform.childCount == 0) return;

        currentChild = transform.GetChild(0);
        int forLoopCounter = 1;

        for (int i = 0; i < forLoopCounter; i++)
        {
            var currentRotY = currentChild.transform.localRotation.eulerAngles.y;
            currentChild.transform.localRotation = Quaternion.identity;
            if (Mathf.Abs(currentChild.transform.rotation.x) > multiplePaperDetachAngle || Mathf.Abs(currentChild.transform.rotation.z) > multiplePaperDetachAngle)
            {
                lastObject = currentChild.transform.parent.gameObject;
                DetachPapersAbovePoint(currentChild, 0);
                forLoopCounter--;
            }
            else
            {
                currentChild.transform.Rotate(new Vector3(-paperVectorX, 0, paperVectorZ) * paperLeanMagnitude + Vector3.up * currentRotY);
                if (currentChild.transform.childCount != 0)
                {
                    forLoopCounter++;
                    currentChild = currentChild.GetChild(0);
                }
                if (Mathf.Abs(currentChild.transform.rotation.x) > singlePaperDetachAngle || Mathf.Abs(currentChild.transform.rotation.z) > singlePaperDetachAngle) DetachTopPaper();
            }
        }
    }
    private void UpdatePanel()
    {
        if (Managers.Instance == null || !LevelManager.Instance.IsLevelStarted) return;

        if (paperCounterPanel == null) paperCounterPanel = UIIDHolder.Panels[UIIDHolder.PaperCounterPanel] as PaperCounterPanel;
        paperCounterPanel.paperAmountText.SetText(totalPaperAmount.ToString());
    }
    private void DetachTopPaper()
    {
        HapticManager.Haptic(HapticTypes.SoftImpact);
        totalPaperAmount--;
        var objectToDetach = lastObject;
        lastObject = objectToDetach.transform.parent.gameObject;
        objectToDetach.transform.parent = detachedPapersTransform;
        objectToDetach.AddComponent<AutoDestroy>();
        var detachedPaper = objectToDetach.AddComponent<DetachedPaper>();
        detachedPaper.InitialSpeed = playerMover.Speed + objectToDetach.transform.up * ExternalForce.magnitude * externalForceImpactMultiplier * 30 * UnityEngine.Random.Range(1, 1.2f) + ExternalForce * externalForceImpactMultiplier;
    }
    private void DetachPapersAbovePoint(Transform firstPaper, float impactPointMax)
    {
        totalPaperAmount = Convert.ToInt32(firstPaper.parent.name.Substring(firstPaper.parent.name.IndexOf(' ') + 1));
        var currentPaper = firstPaper;
        int forLoopCounter = 1;

        for (int i = 0; i < forLoopCounter; i++)
        {
            currentPaper.parent = detachedPapersTransform;
            currentPaper.gameObject.AddComponent<AutoDestroy>();
            var detachedPaper = currentPaper.gameObject.AddComponent<DetachedPaper>();

            if (currentPaper.transform.position.y < impactPointMax)
                detachedPaper.InitialSpeed = transform.right * UnityEngine.Random.Range(-paperSpeedAfterCollision, paperSpeedAfterCollision) + transform.forward * UnityEngine.Random.Range(-paperSpeedAfterCollision, -0);
            else detachedPaper.InitialSpeed = playerMover.Speed * Mathf.Min((currentPaper.transform.position.y  - impactPointMax), 10) * 50;

            if (currentPaper.childCount != 0)
            {
                forLoopCounter++;
                currentPaper = currentPaper.GetChild(0);
            }
        }
    }
    public void CheckPaperStackHeight(float impactPointMin, float impactPointMax)
    {
        if (transform.childCount == 0) return;

        currentChild = transform.GetChild(0);
        int forLoopCounter = 1;

        for (int i = 0; i < forLoopCounter; i++)
        {
            if (currentChild.transform.position.y > impactPointMin)
            {
                lastObject = currentChild.transform.parent.gameObject;
                paperManCameraController.ShakeCamera(5, 1, 10);
                DetachPapersAbovePoint(currentChild, impactPointMax);
            }
            else if (currentChild.transform.childCount != 0)
            {
                forLoopCounter++;
                currentChild = currentChild.GetChild(0);
            }
        }
    }
    public void InstantiatePapers(int numberOfPapersToSpawn)
    {
        Vector3 spawnPos = lastObject.transform.position + Vector3.up * gapBetweenPapers / 2;

        for (int i = 0; i < numberOfPapersToSpawn; i++)
        {
            totalPaperAmount++;
            var newObject = Instantiate(paperPrefabs[UnityEngine.Random.Range(0, paperPrefabs.Length - 1)], spawnPos, Quaternion.identity);
            newObject.name = "Paper " + totalPaperAmount;
            newObject.transform.Rotate(Vector3.up * (UnityEngine.Random.Range(-paperAngleRandomness, paperAngleRandomness) + lastObject.transform.rotation.eulerAngles.y));
            newObject.transform.parent = lastObject.transform;
            lastObject = newObject;
            spawnPos += Vector3.up * gapBetweenPapers;
        }
    }
}
