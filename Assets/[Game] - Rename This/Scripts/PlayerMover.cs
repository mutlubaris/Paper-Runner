using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    #region Variables
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 50f;
    [SerializeField] private float horizontalPosLimit = 10f;

    private Vector3 oldSpeed;
    private Vector3 oldPos;
    private Vector3 newPos;
    private float initialPosX;
    public float forwardSpeed = 0f;
    private float horizontalSpeed = 0f;
    private Vector3? oldMousePos = null;
    private Vector3 newMousePos;

    [HideInInspector] public Vector3 Acceleration;
    [HideInInspector] public Vector3 Speed;
    [HideInInspector] public float Sensitivity;

    public float MaxSpeedForward = 30f;
    #endregion

    private void Start()
    {
        initialPosX = transform.position.x;
        oldPos = transform.position;
        newPos = transform.position;
        Sensitivity = 1f;
    }

    private void Update()
    {
        if (Managers.Instance == null || !LevelManager.Instance.IsLevelStarted) return;

        CalculateForwardSpeed();
        CalculateHorizontalSpeed();
        MoveObject();
        CalculateAcceleration();
    }

    private void CalculateForwardSpeed()
    {
        if (forwardSpeed < MaxSpeedForward)
        {
            forwardSpeed += acceleration * Time.deltaTime;
            if (forwardSpeed > MaxSpeedForward) forwardSpeed = MaxSpeedForward;
        }
        else if (forwardSpeed > MaxSpeedForward)
        {
            forwardSpeed -= deceleration * Time.deltaTime;
            if (forwardSpeed < MaxSpeedForward) forwardSpeed = MaxSpeedForward;
        }
    }
    private void CalculateHorizontalSpeed()
    {
        if (GameManager.Instance.IsStageCompleted)
        {
            horizontalSpeed = 0;
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            oldMousePos = null;
            horizontalSpeed = 0;
            return;
        }
        if (!Input.GetMouseButton(0) || (transform.position.x > initialPosX + horizontalPosLimit / 2) || (transform.position.x < initialPosX - horizontalPosLimit / 2)) return;
        newMousePos = Input.mousePosition;
        if (oldMousePos != null) horizontalSpeed = (newMousePos.x - oldMousePos.GetValueOrDefault().x) * Sensitivity;
        oldMousePos = Input.mousePosition;
    }
    private void MoveObject()
    {
        transform.position += transform.forward * Time.deltaTime * forwardSpeed;
        transform.position += transform.right * Time.deltaTime * horizontalSpeed;

        var clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, initialPosX - horizontalPosLimit / 2, initialPosX + horizontalPosLimit / 2);
        transform.position = clampedPos;
    }
    private void CalculateAcceleration()
    {
        newPos = transform.position;
        Speed = (newPos - oldPos) / Time.deltaTime / 100;
        Acceleration = (Speed - oldSpeed) / Time.deltaTime / 100;
        oldSpeed = Speed;
        oldPos = transform.position;
    }
}
