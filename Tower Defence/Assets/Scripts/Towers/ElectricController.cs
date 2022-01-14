using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricController : MonoBehaviour
{
    Upgrades upgrades;

    [SerializeField]
    int damageFinal;


    public int damage = 25;
    public float Range;
    private float singleGridDistance = 17.3333f;
    public SphereCollider rangeTrigger;
    public float shockSpeed;
    public float timeToShock;


    

    public List<GameObject> enemiesInRange;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        upgrades = FindObjectOfType<Upgrades>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set distance tower can do damage
        rangeTrigger.radius = (Range + upgrades.electricRangeUpgrade) * singleGridDistance + (singleGridDistance / 2);

        timeToShock -= Time.deltaTime;

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
        }

        //If enemies are in range, and can shock, deal damage to all enemies in range
        if (enemiesInRange.Count != 0 && timeToShock <= 0)
        {           
            timeToShock = shockSpeed / upgrades.electricSpeedUpgrade;
            foreach (GameObject monster in enemiesInRange)
            {
                monster.GetComponent<MonsterController>().currentHealth -= damage * upgrades.electricDamageUpgrade;
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
}
