using UnityEngine;

public class PlayerMoverManual : MonoBehaviour
{
    #region Variables
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 50f;

    [HideInInspector] public Vector3 Acceleration;
    [HideInInspector] public Vector3 NewSpeed;

    private Vector3 oldSpeed;
    private float forwardSpeed = 0f;
    private float horizontalSpeed = 0f;
    #endregion

    void Update()
    {
        if (Managers.Instance == null) return;
        
        CalculateForwardSpeed();
        CalculateHorizontalSpeed();
        MoveObject();
        CalculateAcceleration();
    }

    private void CalculateForwardSpeed()
    {
        if (InputManager.Instance.Joystick.Vertical > 0 && forwardSpeed <= maxSpeed && forwardSpeed >= 0) forwardSpeed += acceleration * Time.deltaTime * InputManager.Instance.Joystick.Vertical;
        else if (InputManager.Instance.Joystick.Vertical < 0 && forwardSpeed >= -maxSpeed && forwardSpeed <= 0) forwardSpeed += acceleration * Time.deltaTime * InputManager.Instance.Joystick.Vertical;
        else DecelerateForwardSpeed();
        forwardSpeed = Mathf.Clamp(forwardSpeed, -maxSpeed, maxSpeed);
    }
    private void CalculateHorizontalSpeed()
    {
        if (InputManager.Instance.Joystick.Horizontal > 0 && horizontalSpeed <= maxSpeed && horizontalSpeed >= 0) horizontalSpeed += acceleration * Time.deltaTime * InputManager.Instance.Joystick.Horizontal;
        else if (InputManager.Instance.Joystick.Horizontal < 0 && horizontalSpeed >= -maxSpeed && horizontalSpeed <= 0) horizontalSpeed += acceleration * Time.deltaTime * InputManager.Instance.Joystick.Horizontal;
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
    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 500, 200), "Restart")) LevelManager.Instance.ReloadLevel();
    }
}
