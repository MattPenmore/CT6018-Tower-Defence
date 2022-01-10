using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{
    public GameObject closest = null;

    public List<GameObject> closestCells = new List<GameObject>();

    private void Start()
    {
        FindClosestCells();
    }
    public void FindClosestCells()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Cell");
        closest = null;
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
        closest.GetComponent<GridCell>().isTarget = true;
        closestCells.Add(closest);
        Vector3 closestCoordinates = closest.GetComponent<GridCell>().coordinates;
        GameObject[] monsters;
        monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject go in gos)
        {
            Vector3 Coordinates = go.GetComponent<GridCell>().coordinates;
            if (Mathf.Abs(Coordinates.x - closestCoordinates.x) <=2 && Mathf.Abs(Coordinates.y - closestCoordinates.y) <= 2 && Mathf.Abs(Coordinates.z - closestCoordinates.z) <=2)
            {
                go.GetComponent<GridCell>().notSelectable = true;
                closestCells.Add(go);
            }
        }
    }
}
