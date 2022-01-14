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
        //Get all cells
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Cell");
        closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        //Get distance of cells from base, find cell that is closest
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

        closestCells.Add(closest);
        //Get coordinates of closest cells
        Vector3 closestCoordinates = closest.GetComponent<GridCell>().coordinates;

        //Get cells within 2 distance from base and make them unselectable for placing towers and spawners
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
