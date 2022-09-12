using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : TowerController
{
    // Update is called once per frame
    void Update()
    {
        timeToShoot -= Time.deltaTime;
        //Set distance tower can do damage
        rangeTrigger.radius = (Range + upgrades.GetTurretRangeUpgrade()) * singleGridDistance + (singleGridDistance / 2);

        if (enemiesInRange.Count != 0)
        {
            EnemyCheck();
            FindEnemies();

            //If have a target and can see it, rotate to face target, otherwise find new target
            if (enemiesInSight.Contains(target))
            {
                AttackTarget();
            }
            else if (enemiesInSight.Count != 0)
            {
                FindTarget();
                AttackTarget();
            }
            else
            {
                target = null;
            }
        }
    }
}
