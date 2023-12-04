using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private float destructionTimer = 0f;
    private float destructionTime = 3f;

    private void Update()
    {
        destructionTimer += Time.deltaTime;
        if (destructionTimer >= destructionTime) Destroy(gameObject);
    }
}
