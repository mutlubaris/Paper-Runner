using UnityEngine;

public class BladeRotator : MonoBehaviour
{
    [SerializeField] private float bladeRotSpeed = 2500f;
    
    void Update()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * bladeRotSpeed);
    }
}
