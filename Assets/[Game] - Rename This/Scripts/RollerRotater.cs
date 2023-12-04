using UnityEngine;

public class RollerRotater : MonoBehaviour
{
    [SerializeField] private float rollerRotSpeed = 500f;

    void Update()
    {
        transform.Rotate(Vector3.left * Time.deltaTime * rollerRotSpeed);
    }
}
