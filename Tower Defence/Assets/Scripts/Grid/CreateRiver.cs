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

    public void CreateNewRiver()
    {
        //Make sure all variables start empty
        edges.Clear();
        z0.Clear();
        riverTiles.Clear();

        //Get all cells
        gos = GameObject.FindGameObjectsWithTag("Cell");
        foreach (GameObject go in gos)
        {
            //Find which cells are along the edge op the map
            if (go.GetComponent<GridCell>().isEdge)
            {
                edges.Add(go);

                //Find cells that are along the bottom of the map
                if (go.GetComponent<GridCell>().coordinates.z == 0)
                    z0.Add(go);
            }
        }

        //Get random starting position for river. Always starts along the bottom of the map
        int rand = Random.Range(minStart, maxStart);
        startPoint = z0[rand];
        curvePoints.Add(startPoint);

        //Get next position along river where it will turn
        AddCurvePoint(startPoint);

        //Prevent river going through base
        foreach (GameObject cell in mainBase.closestCells)
        {
            cell.GetComponent<GridCell>().isObstacle = true;
        }

        //Pathfind between each turning point in the river, and change all tiles along paths to river tiles
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

        //Get number of bridges path should have, and create them
        int numBridges = Random.Range(minNumBridges, maxNumBridges + 1);

        for(int i = 0; i < numBridges; i++)
        {
            CreateBridge();
        }

    }

    void AddCurvePoint(GameObject prevPoint)
    {
        //Get height, width and direction, to next turning point in river.
        bool madeCurve = false;
        int randUp = Random.Range(minCurveHeight, maxCurveHeight + 1);
        //If position goes off top of map, set position to top of map.
        if(prevPoint.GetComponent<GridCell>().coordinates.z + randUp >= grid.height - 1)
        {
            randUp = Mathf.RoundToInt(grid.height - prevPoint.GetComponent<GridCell>().coordinates.z - 1);
        }
        //Get width and direction, to next turning point in river
        int randWidth = Random.Range(minCurveWidth, maxCurveWidth - 1);
        int randLeftOrRight = Random.Range(0, 2);

        //If turning Left
        if(randLeftOrRight == 0)
        {
            foreach (GameObject go in gos)
            {
                //Check if Cell matches where turning point should be. If it does make it a curve point.
                if(go.GetComponent<GridCell>().coordinates.z == prevPoint.GetComponent<GridCell>().coordinates.z + randUp && go.GetComponent<GridCell>().coordinates.x == prevPoint.GetComponent<GridCell>().coordinates.x - randUp + randWidth)
                {
                    curvePoints.Add(go);
                    //If cell is on the edge of map, end the river, otherwise create another turning point.
                    if(!go.GetComponent<GridCell>().isEdge)
                    {
                        //Make sure posiiton is not too close to main base. If it is remove it and start againform previous position
                        //Otherwise make new turning point from current position
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
                //Check if Cell matches where turning point should be. If it does make it a curve point.
                if (go.GetComponent<GridCell>().coordinates.z == prevPoint.GetComponent<GridCell>().coordinates.z + randUp && go.GetComponent<GridCell>().coordinates.y == prevPoint.GetComponent<GridCell>().coordinates.y - randUp + randWidth)
                {
                    curvePoints.Add(go);
                    //If cell is on the edge of map, end the river, otherwise create another turning point.
                    if (!go.GetComponent<GridCell>().isEdge)
                    {
                        //Make sure posiiton is not too close to main base. If it is remove it and start againform previous position
                        //Otherwise make new turning point from current position
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
        //If making cturning point failed, try again.
        if(!madeCurve)
        {
            AddCurvePoint(prevPoint);
        }
    }

    void CreateBridge()
    {
        //Get position bridge should be and make sure it isn't already a bridge. If it is, try again
        int randBridge = Random.Range(4, riverTiles.Count - 4);

        if(!riverTiles[randBridge].GetComponent<GridCell>().isBridge)
        {
            //Check number of adjacent river tiles
            int numRiverTiles = 0;
            foreach(GameObject cell in riverTiles[randBridge].GetComponent<GridCell>().FindAdjacentCells())
            {
                if (cell.GetComponent<GridCell>().isRiver)
                {
                    numRiverTiles += 1;
                }
            }
            //If there is exactly 2 adjacent river tiles, a bridge can be made, otherwise try again
            if(numRiverTiles == 2)
            {
                riverTiles[randBridge].GetComponent<GridCell>().isBridge = true;
            }
            else
            {
                CreateBridge();
            }
        }
        else
        {
            CreateBridge();
        }
    }
}
