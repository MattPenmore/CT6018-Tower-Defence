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
    public bool isEdge;

    [SerializeField]
    MeshRenderer cellMaterial;

    public bool isRiver;
    public bool isBridge;

    
    public int f => -g + h;
    public int g;
    public int h;
    public List<GameObject> adjacentCells;
    public GameObject previousCell;


    public bool spawnerPathsChecked = false;
    public bool towerPlacable = true;

    // Start is called before the first frame update
    void Start()
    {
        FindAdjacentCells();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRiver)
        {
            notSelectable = true;
            if(isBridge)
            {
                isObstacle = false;
                cellMaterial.material.color = new Color32(139, 69, 19,255);
            }
            else
            {
                isObstacle = true;
                cellMaterial.material.color = new Color32(0, 0, 139, 255);
            }
        }
        else
        {
            cellMaterial.material.color = Color.white;
        }

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

        if (adjacentCells.Count < 6)
            isEdge = true;
        else
            isEdge = false;
    }

    public List<GameObject> FindAdjacentCells()
    {
        adjacentCells = new List<GameObject>();
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
