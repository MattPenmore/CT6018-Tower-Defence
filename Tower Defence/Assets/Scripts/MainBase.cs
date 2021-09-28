using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{

    private void Start()
    {
        FindClosestCells();
    }
    public void FindClosestCells()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Cell");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        Vector3 closestCoordinates = closest.GetComponent<GridCell>().cooridinates;

        foreach (GameObject go in gos)
        {
            Vector3 Coordinates = go.GetComponent<GridCell>().cooridinates;
            if (Mathf.Abs(Coordinates.x - closestCoordinates.x) <=2 && Mathf.Abs(Coordinates.y - closestCoordinates.y) <= 2 && Mathf.Abs(Coordinates.z - closestCoordinates.z) <=2)
            {
                go.GetComponent<GridCell>().notSelectable = true;
                go.GetComponent<GridCell>().isTarget = true;
            }
        }
    }
}
