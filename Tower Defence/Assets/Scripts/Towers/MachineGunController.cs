using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : MonoBehaviour
{
    [SerializeField]
    List<string> towerNames;

    [SerializeField]
    List<string> spawnerNames;

    Upgrades upgrades;
    public int damage = 25;
    public float Range;
    private float singleGridDistance = 17.3333f;
    public SphereCollider rangeTrigger;
    public float shootSpeed;
    public float timeToShoot;
    public float rotationSpeed;
    public GameObject turretRotator;
    public List<GameObject> enemiesInRange;
    public List<GameObject> enemiesInSight;

    public GameObject target;

    public GameObject shootAnim;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        enemiesInSight = new List<GameObject>();

        upgrades = FindObjectOfType<Upgrades>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToShoot -= Time.deltaTime;
        //Set distance tower can do damage
        rangeTrigger.radius = (Range + upgrades.machineGunRangeUpgrade) * singleGridDistance + (singleGridDistance / 2);

        //Check if enemies in range still exist. If they don't remove from list
        if (enemiesInRange.Count != 0)
        {
            for (int i = enemiesInRange.Count - 1; i >= 0; i--)
            {
                if (enemiesInRange[i] == null)
                {
                    enemiesInRange.RemoveAt(i);
                }
            }
            enemiesInSight.Clear();

            //Find enemies in both line of sight and range of the tower;
            foreach (GameObject enemy in enemiesInRange)
            {
                bool isObstacle = false;
                RaycastHit[] hits = Physics.RaycastAll(turretRotator.transform.position, enemy.transform.position - turretRotator.transform.position, Vector3.Distance(turretRotator.transform.position, enemy.transform.position));
                foreach (RaycastHit hit in hits)
                {
                    if (spawnerNames.Contains(hit.transform.tag) || towerNames.Contains(hit.transform.tag))
                    {
                        isObstacle = true;
                        break;
                    }
                }

                if (!isObstacle)
                {
                    enemiesInSight.Add(enemy);
                }
            }

            //If have a target and can see it, rotate to face target, otherwise find new target
            if (enemiesInSight.Contains(target))
            {
                Vector3 finalRotation = Quaternion.LookRotation(target.transform.position - turretRotator.transform.position).eulerAngles;

                Vector3 currentRotation = turretRotator.transform.eulerAngles;

                if (currentRotation.y - finalRotation.y > 180)
                {
                    currentRotation.y -= 360;
                }
                if (finalRotation.y - currentRotation.y > 180)
                {
                    currentRotation.y += 360;
                }

                currentRotation.y = Mathf.Lerp(currentRotation.y, finalRotation.y, Time.deltaTime * rotationSpeed);
                turretRotator.transform.eulerAngles = currentRotation;

                //Shoot target if able
                Shoot();
            }
            else if (enemiesInSight.Count != 0)
            {
                GameObject closest = null;
                float distance = Mathf.Infinity;

                foreach (GameObject enem in enemiesInSight)
                {
                    //Get distance of monster from turret
                    Vector3 diff = enem.transform.position - transform.position;
                    float curDistance = diff.sqrMagnitude;
                    //If closest, set as closest
                    if (curDistance < distance)
                    {
                        closest = enem;
                        distance = curDistance;
                    }
                }
                //Make closest monster target.
                target = closest;

                //rotate to face target
                Vector3 finalRotation = Quaternion.LookRotation(closest.transform.position - turretRotator.transform.position).eulerAngles;

                Vector3 currentRotation = turretRotator.transform.eulerAngles;
                currentRotation.y = Mathf.Lerp(currentRotation.y, finalRotation.y, Time.deltaTime * rotationSpeed);
                turretRotator.transform.eulerAngles = currentRotation;

                //Shoot target if able
                Shoot();
            }
            else
            {
                target = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void Shoot()
    {
        //If angle tower is facing, is close enough to facing target, Deal damage to target.
        float angle = 15;
        if (Vector3.Angle(turretRotator.transform.forward, target.transform.position - turretRotator.transform.position) < angle && timeToShoot <= 0)
        {

            Debug.DrawRay(turretRotator.transform.position, target.transform.position - turretRotator.transform.position,
            Color.red);
            target.GetComponent<MonsterController>().currentHealth -= damage * upgrades.machineGunDamageUpgrade;
            shootAnim.SetActive(true);
            timeToShoot = shootSpeed / upgrades.machineGunSpeedUpgrade;
        }
    }
}
