using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private float destructionTimer = 0f;
    private float destructionTime = 20f;

    private void Update()
    {
        destructionTimer += Time.deltaTime;
        if (destructionTimer >= destructionTime) Destroy(gameObject);
    }
}
