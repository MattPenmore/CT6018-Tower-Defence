using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricController : TowerController
{
    public float shockSpeed;
    public float timeToShock;

    // Update is called once per frame
    void Update()
    {
        //Set distance tower can do damage
        rangeTrigger.radius = (Range + upgrades.electricRangeUpgrade) * singleGridDistance + (singleGridDistance / 2);

        timeToShock -= Time.deltaTime;

        //Check if enemies in range still exist. If they don't remove from list
        EnemyCheck();

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
}
