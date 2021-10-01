using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    MeshFilter meshFilter;
    public Vector3 coordinates;
    public MeshRenderer selector;
    public bool isSelector;
    public bool notSelectable;
    public bool isTarget;
    public int distanceToBase;
    public bool isObstacle;

    
    public int f => g + h;
    public int g;
    public int h;
    public List<GameObject> adjacentCells;
    public GameObject previousCell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (notSelectable)
        {
            selector.material.SetColor("_Color", Color.red);
        }
        else
        {
            selector.material.SetColor("_Color", Color.blue);
        }

       if(isSelector)
        {
            selector.enabled = true;
        }
        else
        {
            selector.enabled = false;
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    isSelector = false;
        //}
    }

    public List<GameObject> FindAdjacentCells()
    {
        List<GameObject> adjacentCells = new List<GameObject>();
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Cell");

        foreach (GameObject go in gos)
        {
            Vector3 otherCoordinates = go.GetComponent<GridCell>().coordinates;
            if (Mathf.Abs(otherCoordinates.x - coordinates.x) <=1 && Mathf.Abs(otherCoordinates.y - coordinates.y) <= 1 && Mathf.Abs(otherCoordinates.z - coordinates.z) <=1)
            {
                if (!(Mathf.Abs(otherCoordinates.x - coordinates.x) == 0) || !(Mathf.Abs(otherCoordinates.y - coordinates.y) == 0) && !(Mathf.Abs(otherCoordinates.z - coordinates.z) == 0))
                {
                    adjacentCells.Add(go);
                 
                }
            }
        }
        return adjacentCells;
    }

    public void SetPreviousCell(GameObject makePreviousCell)
    {
        previousCell = makePreviousCell;
    }
}
