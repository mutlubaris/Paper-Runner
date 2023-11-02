using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMover : MonoBehaviour
{
    private int rotationDirectionX = 1;
    private int rotationDirectionZ = 1;

    private float rotationChangeAngleX = .3f;
    private float rotationChangeAngleZ = .1f;
    private float lateralMoveMultiplier = 15f;
    private float gravityMultiplier = 1f;
    private float initialSpeedMultiplier = 2f;
    private float stopHeightY = 0.01f;
    private bool movementStopped = false;

    public Vector3 InitialSpeed;

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

        transform.Rotate(new Vector3((Mathf.Abs(transform.rotation.x) + Random.Range(.1f, .2f)) * rotationDirectionX, 0, (Mathf.Abs(transform.rotation.z) + Random.Range(.1f, .2f)) * rotationDirectionZ));
        transform.position += (transform.forward * -transform.forward.y * lateralMoveMultiplier / (initialSpeedMultiplier + .1f) / 10 + Vector3.down * gravityMultiplier + InitialSpeed * initialSpeedMultiplier) * Time.deltaTime;
        if (transform.position.y <= stopHeightY)
        {
            transform.LookAt(Vector3.up);
            transform.position = new Vector3(transform.position.x, stopHeightY, transform.position.z);
            movementStopped = true;
        }
    }
}
