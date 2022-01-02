using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    float maxX;
    [SerializeField]
    float maxZ;
    [SerializeField]
    float minX;
    [SerializeField]
    float minZ;

    [SerializeField]
    float speed;

    

    // Update is called once per frame
    void Update()
    {
        float speedX = 0;
        float speedZ = 0;

        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > minX)
        {
            speedX -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < maxX)
        {
            speedX += 1;
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.z < maxZ)
        {
            speedZ += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.z > minZ)
        {
            speedZ -= 1;
        }

        transform.position = new Vector3(transform.position.x + speedX * speed * Time.deltaTime, transform.position.y, transform.position.z + speedZ * speed * Time.deltaTime);
    }
}
