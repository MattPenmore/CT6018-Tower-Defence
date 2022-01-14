using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterController : MonoBehaviour
{
    public string monsterName;
    [SerializeField]
    float maxLifetime = 500;

    [SerializeField]
    float currentLifeTime;

    public int maxPathLength = 500;
    public GameObject[] Path;
    GameObject pathfind;
    GameObject previousPathingCell;
    public GameObject currentCell;
    GameObject previousCell;
    GameObject mainBase;
    public GameObject endPoint;
    public float moveSpeed;
    Rigidbody rb;
    //
    public Vector3 offset;

    public int maxHealth = 100;
    public int currentHealth;
    public int scoreValue;
    public bool needToPathfind = true;

    float timeToPathFind = 5;
    float timeUntilPathfind = 5;
    
    Upgrades upgrades;

    // Start is called before the first frame update
    void Start()
    {
        //find players base
        mainBase = GameObject.FindGameObjectWithTag("Base");
        currentLifeTime = 0;
        pathfind = gameObject;
        endPoint = mainBase.GetComponent<MainBase>().closest;
        upgrades = FindObjectOfType<Upgrades>();
        Path = new GameObject[maxPathLength];
        rb = GetComponent<Rigidbody>();

        //Set health and score values dependant on type of monster and upgrades
        if(monsterName == "goblin")
        {
            maxHealth *= upgrades.goblinHealthUpgrade;
            scoreValue *= upgrades.goblinRewardUpgrade;
        }
        if (monsterName == "skeleton")
        {
            maxHealth *= upgrades.skeletonHealthUpgrade;
            scoreValue *= upgrades.skeletonRewardUpgrade;
        }
        if (monsterName == "slime")
        {
            maxHealth *= upgrades.slimeHealthUpgrade;
            scoreValue *= upgrades.slimeRewardUpgrade;
        }
        if (monsterName == "ogre")
        {
            maxHealth *= upgrades.ogreHealthUpgrade;
            scoreValue *= upgrades.ogreRewardUpgrade;
        }
        currentHealth = maxHealth;
        //find path to players base
        PathFind();

    }

    // Update is called once per frame
    void Update()
    {
        //If monster has lived too long destroy it to prevent lag
        if(currentLifeTime >= maxLifetime)
        {
            Destroy(gameObject);
        }

        //if monster has no health, give player gold and destroy it.
        if(currentHealth <= 0)
        {
            mainBase.GetComponent<Score>().score += scoreValue; 
            Destroy(gameObject);
        }

        if (needToPathfind)
        {
            PathFind();
            needToPathfind = false;
        }

        //If already have path to player base that doesn't need changing, continue along it, otherwise wait until time to pathfind
        FindCurrentCell();
        if (previousPathingCell != null && Path.Contains(currentCell) && !needToPathfind)
        {
            //Get current position along path
            int index = System.Array.IndexOf(Path, currentCell);
            //If reached base, take gold from player and destroy monster
            if (Path.Length == index + 1 || Path.Length == index + 2)
            {
                mainBase.GetComponent<Score>().score -= scoreValue;
                Destroy(gameObject);
            }

            //If not reached base
            if(Path.Length > index + 2)
            {
                //Get distance from center of cell, if not close enough to it, move towards it, otherwise move towards next cell on path
                offset = Path[index].transform.position - transform.position + new Vector3(0, transform.position.y - Path[index].transform.position.y, 0);
                if (offset.magnitude > 1f && previousCell != currentCell)
                {
                    Vector3 Target = Path[index].transform.position;
                    MoveTowards(Target);
                }
                else
                {
                    Vector3 Target = Path[index + 1].transform.position;
                    MoveTowards(Target);
                    previousCell = currentCell;
                }
            }
        }
        else
        {
            timeUntilPathfind -= Time.deltaTime;
            if(timeUntilPathfind <= 0)
            {
                //Prevent all occuring at same time
                float rand = Random.Range(-1f, 1f);

                timeUntilPathfind = timeToPathFind + rand;
                needToPathfind = true;
            }
        }
        
    }

    void FindCurrentCell()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.gameObject.tag == "Cell")
            {
                currentCell = objectHit.gameObject;
            }
        }
    }

    void MoveTowards(Vector3 Target)
    {
        offset = Target - transform.position + new Vector3(0,transform.position.y - Target.y ,0);
        if (offset.magnitude > 0.1f)
        {
            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            offset = offset.normalized * moveSpeed;
            //normalize it and account for movement speed.
            rb.velocity = offset;
            //actually move the character.

        }
    }

    public void PathFind()
    {
        FindCurrentCell();
        //If have previous path, check if contains cell, monster is on
        if (previousPathingCell != null && Path.Contains(currentCell))
        {
            //Check if any of cells on path have become obstacles. If they have new path needed, otherwise continue using previous path.
            bool usePreviousPath = true;
            foreach (GameObject cell in Path)
            {
                if (cell.GetComponent<GridCell>().isObstacle)
                {
                    usePreviousPath = false;
                }
            }
            if (usePreviousPath)
            {
                int index = System.Array.IndexOf(Path, currentCell);
                //If reached base, take gold from player and destroy monster
                if (Path.Length == index + 1 || Path.Length == index + 2)
                {
                    mainBase.GetComponent<Score>().score -= scoreValue;
                    Destroy(gameObject);
                }
                //If not reached base
                //Get distance from center of cell, if not close enough to it, move towards it, otherwise move towards next cell on path
                offset = Path[index].transform.position - transform.position + new Vector3(0, transform.position.y - Path[index].transform.position.y, 0);
                if (offset.magnitude > 1f && previousCell != currentCell)
                {
                    Vector3 Target = Path[index].transform.position;
                    MoveTowards(Target);
                }
                else
                {
                    Vector3 Target = Path[index + 1].transform.position;
                    MoveTowards(Target);
                    previousCell = currentCell;
                }
            }
            else
            {
                //Create new path from monster to base
                Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint).ToArray();
                int index = System.Array.IndexOf(Path, currentCell);
                //If no path can be found, monster will just move towards players base, ignoring all obstacles
                if (Path.Length == 0)
                {
                    MoveTowards(endPoint.transform.position);
                }
                else
                {
                    if (Path.Length == 1)
                    {
                        Destroy(gameObject);
                    }

                    offset = Path[index].transform.position - transform.position + new Vector3(0, transform.position.y - Path[index].transform.position.y, 0);
                    if (offset.magnitude > 1f)
                    {
                        Vector3 Target = Path[index].transform.position;
                        MoveTowards(Target);
                    }
                    else
                    {
                        Vector3 Target = Path[index + 1].transform.position;
                        MoveTowards(Target);
                        previousCell = currentCell;
                        previousPathingCell = currentCell;
                    }
                }
            }
        }
        else if (currentCell != null)
        {
            Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint).ToArray();
            FindCurrentCell();
            if(!Path.Contains(currentCell))
            {
                Debug.Log("Break");
            }
            int index = System.Array.IndexOf(Path, currentCell);
            if (Path.Length == 0)
            {
                MoveTowards(endPoint.transform.position);
            }
            else
            {
                //If reached base, take gold from player and destroy monster
                if (Path.Length == index + 1 || Path.Length == index + 2)
                {
                    mainBase.GetComponent<Score>().score -= scoreValue;
                    Destroy(gameObject);
                }
                //If not reached base
                //Get distance from center of cell, if not close enough to it, move towards it, otherwise move towards next cell on path
                offset = Path[index].transform.position - transform.position + new Vector3(0, transform.position.y - Path[index].transform.position.y, 0);
                if (offset.magnitude > 1f)
                {
                    Vector3 Target = Path[index].transform.position;
                    MoveTowards(Target);
                }
                else
                {
                    Vector3 Target = Path[index + 1].transform.position;
                    MoveTowards(Target);
                    previousCell = currentCell;
                    previousPathingCell = currentCell;
                }
            }
        }
    }
}
