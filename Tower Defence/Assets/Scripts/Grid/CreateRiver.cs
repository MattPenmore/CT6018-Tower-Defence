using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRiver : MonoBehaviour
{
    [SerializeField]
    HexGrid grid;
    GameObject[] gos;
    GameObject startPoint;
    List<GameObject> z0 = new List<GameObject>();
    List<GameObject> edges = new List<GameObject>();

    [SerializeField]
    List<GameObject> curvePoints = new List<GameObject>();

    [SerializeField]
    int minStart;

    [SerializeField]
    int maxStart;

    [SerializeField]
    int minCurveHeight;

    [SerializeField]
    int maxCurveHeight;

    [SerializeField]
    int minCurveWidth;

    [SerializeField]
    int maxCurveWidth;

    [SerializeField]
    int minNumBridges;

    [SerializeField]
    int maxNumBridges;

    [SerializeField]
    MainBase mainBase;

    GameObject[] Path;


    List<GameObject> riverTiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNewRiver()
    {
        edges.Clear();
        z0.Clear();
        riverTiles.Clear();

        gos = GameObject.FindGameObjectsWithTag("Cell");
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<GridCell>().isEdge)
            {
                edges.Add(go);

                if (go.GetComponent<GridCell>().coordinates.z == 0)
                    z0.Add(go);
            }
        }


        int rand = Random.Range(minStart, maxStart);
        startPoint = z0[rand];
        curvePoints.Add(startPoint);
        AddCurvePoint(startPoint);

        //Prevent river going through base
        foreach (GameObject cell in mainBase.closestCells)
        {
            cell.GetComponent<GridCell>().isObstacle = true;
        }

        for (int i = 0; i < curvePoints.Count - 1; i++)
        {
            Path = new GameObject[500];
            
            Path = gameObject.GetComponent<Pathfind>().FindPath(curvePoints[i], curvePoints[i+1]).ToArray();
            foreach(GameObject cell in Path)
            {
                cell.GetComponent<GridCell>().isRiver = true;
                riverTiles.Add(cell);
            }
        }

        //Make base pathable again
        foreach (GameObject cell in mainBase.closestCells)
        {
            cell.GetComponent<GridCell>().isObstacle = false;
        }

        int numBridges = Random.Range(minNumBridges, maxNumBridges + 1);

        for(int i = 0; i < numBridges; i++)
        {
            CreateBridge();
        }

    }

    void AddCurvePoint(GameObject prevPoint)
    {
        bool madeCurve = false;
        int randUp = Random.Range(minCurveHeight, maxCurveHeight + 1);
        if(prevPoint.GetComponent<GridCell>().coordinates.z + randUp >= grid.height - 1)
        {
            randUp = Mathf.RoundToInt(grid.height - prevPoint.GetComponent<GridCell>().coordinates.z - 1);
        }
        int randWidth = Random.Range(minCurveWidth, maxCurveWidth - 1);
        int randLeftOrRight = Random.Range(0, 2);

        if(randLeftOrRight == 0)
        {
            foreach (GameObject go in gos)
            {
                if(go.GetComponent<GridCell>().coordinates.z == prevPoint.GetComponent<GridCell>().coordinates.z + randUp && go.GetComponent<GridCell>().coordinates.x == prevPoint.GetComponent<GridCell>().coordinates.x - randUp + randWidth)
                {
                    curvePoints.Add(go);
                    if(!go.GetComponent<GridCell>().isEdge)
                    {
                        if(!go.GetComponent<GridCell>().notSelectable && !mainBase.closestCells.Contains(go))
                        {
                            AddCurvePoint(go);
                        }
                        else
                        {
                            curvePoints.Remove(go);
                            AddCurvePoint(prevPoint);
                        }
                    }
                    madeCurve = true;
                    break;
                }
            }
        }
        else
        {
            foreach (GameObject go in gos)
            {
                if (go.GetComponent<GridCell>().coordinates.z == prevPoint.GetComponent<GridCell>().coordinates.z + randUp && go.GetComponent<GridCell>().coordinates.y == prevPoint.GetComponent<GridCell>().coordinates.y - randUp + randWidth)
                {
                    curvePoints.Add(go);
                    if (!go.GetComponent<GridCell>().isEdge)
                    {
                        if (!go.GetComponent<GridCell>().notSelectable && !mainBase.closestCells.Contains(go))
                        {
                            AddCurvePoint(go);
                            
                        }
                        else
                        {
                            curvePoints.Remove(go);
                            AddCurvePoint(prevPoint);
                        }
                    }
                    madeCurve = true;
                    break;
                }
            }
        }
        if(!madeCurve)
        {
            AddCurvePoint(prevPoint);
        }
    }

    void CreateBridge()
    {
        int randBridge = Random.Range(4, riverTiles.Count - 4);
        if(!riverTiles[randBridge].GetComponent<GridCell>().isBridge)
        {
            riverTiles[randBridge].GetComponent<GridCell>().isBridge = true;
        }
        else
        {
            CreateBridge();
        }
    }
}
