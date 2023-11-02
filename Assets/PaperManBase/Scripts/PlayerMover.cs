using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;

    [HideInInspector] public Vector3 Acceleration;
    [HideInInspector] public Vector3 NewSpeed;

    private Vector3 oldSpeed;
    private float forwardSpeed = 0f;
    private float horizontalSpeed = 0f;

    void Update()
    {
        CalculateForwardSpeed();
        CalculateHorizontalSpeed();
        MoveObject();
        CalculateAcceleration();
    }

    private void CalculateForwardSpeed()
    {
        if (Input.GetKey("w") && forwardSpeed <= maxSpeed && forwardSpeed >= 0) forwardSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey("s") && forwardSpeed >= -maxSpeed && forwardSpeed <= 0) forwardSpeed -= acceleration * Time.deltaTime;
        else DecelerateForwardSpeed();
        forwardSpeed = Mathf.Clamp(forwardSpeed, -maxSpeed, maxSpeed);
    }
    private void CalculateHorizontalSpeed()
    {
        if (Input.GetKey("d") && horizontalSpeed <= maxSpeed && horizontalSpeed >= 0) horizontalSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey("a") && horizontalSpeed >= -maxSpeed && horizontalSpeed <= 0) horizontalSpeed -= acceleration * Time.deltaTime;
        else DecelerateHorizontalSpeed();
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
    }
    private void MoveObject()
    {
        transform.position += transform.forward * Time.deltaTime * forwardSpeed;
        transform.position += transform.right * Time.deltaTime * horizontalSpeed;
    }
    private void CalculateAcceleration()
    {
        NewSpeed = new Vector3(horizontalSpeed, 0, forwardSpeed);
        Acceleration = NewSpeed - oldSpeed;
        oldSpeed = NewSpeed;
    }

    private void DecelerateForwardSpeed()
    {
        if (forwardSpeed > 0)
        {
            forwardSpeed -= deceleration * Time.deltaTime;
            if (forwardSpeed < 0) forwardSpeed = 0;
        }
        else if (forwardSpeed < 0)
        {
            forwardSpeed += deceleration * Time.deltaTime;
            if (forwardSpeed > 0) forwardSpeed = 0;
        }
    }
    private void DecelerateHorizontalSpeed()
    {
        if (horizontalSpeed > 0)
        {
            horizontalSpeed -= deceleration * Time.deltaTime;
            if (horizontalSpeed < 0) horizontalSpeed = 0;
        }
        else if (horizontalSpeed < 0)
        {
            horizontalSpeed += deceleration * Time.deltaTime;
            if (horizontalSpeed > 0) horizontalSpeed = 0;
        }
    }
}
