using UnityEngine;

public class DetachedPaper : MonoBehaviour
{
    #region Variables
    private int rotationDirectionX = 1;
    private int rotationDirectionZ = 1;
    private float rotationChangeAngleX = .3f;
    private float rotationChangeAngleZ = .1f;
    private float lateralMoveMultiplier = 15f;
    private float gravityMultiplier = 2f;
    private float initialSpeedMultiplier = 1f;
    private float stopHeightY = 0.01f;
    private float rotationY;
    private bool movementStopped = false;

    public Vector3 InitialSpeed;
    #endregion

    private void Start()
    {
        rotationY = Random.Range(-1f, 1f);
    }

    void Update()
    {
        if (movementStopped) return;
        SetRotationDirection();
        SetRotationAndSpeed();
    }

    private void SetRotationDirection()
    {
        if (transform.rotation.x > rotationChangeAngleX) rotationDirectionX = -1;
        if (transform.rotation.x < -rotationChangeAngleX) rotationDirectionX = 1;
        if (transform.rotation.z > rotationChangeAngleZ) rotationDirectionZ = -1;
        if (transform.rotation.z < -rotationChangeAngleZ) rotationDirectionZ = 1;
    }
    private void SetRotationAndSpeed()
    {
        initialSpeedMultiplier -= Time.deltaTime;
        initialSpeedMultiplier = Mathf.Clamp(initialSpeedMultiplier, 0, 1);

        transform.Rotate(new Vector3((Mathf.Abs(transform.rotation.x) + Random.Range(.1f, .2f)) * rotationDirectionX, rotationY, (Mathf.Abs(transform.rotation.z) + Random.Range(.1f, .2f)) * rotationDirectionZ));
        transform.position += ((transform.forward * -transform.forward.y * lateralMoveMultiplier + Vector3.down * gravityMultiplier) / (initialSpeedMultiplier + .1f) / 10 + InitialSpeed * initialSpeedMultiplier) * Time.deltaTime;
        if (transform.position.y <= stopHeightY)
        {
            transform.LookAt(Vector3.up);
            transform.position = new Vector3(transform.position.x, stopHeightY, transform.position.z);
            movementStopped = true;
        }
    }
}
