using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, rotationSpeed);
    }
}
