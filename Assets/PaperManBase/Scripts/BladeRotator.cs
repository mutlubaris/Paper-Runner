﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 1000);
    }
}
