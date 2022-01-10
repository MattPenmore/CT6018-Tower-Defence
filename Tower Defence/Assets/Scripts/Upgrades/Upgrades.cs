using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    // Towers
    public int turretRangeUpgrade;
    public int turretDamageUpgrade;
    public int turretSpeedUpgrade;

    public int hammerDamageUpgrade;
    public int hammerSpeedUpgrade;

    public int electricRangeUpgrade;
    public int electricDamageUpgrade;
    public int electricSpeedUpgrade;

    public int rocketRangeUpgrade;
    public int rocketDamageUpgrade;
    public int rocketSpeedUpgrade;

    public int machineGunRangeUpgrade;
    public int machineGunDamageUpgrade;
    public int machineGunSpeedUpgrade;

    //Spawners
    public int goblinSpawnUpgrade;
    public int goblinHealthUpgrade;
    public int goblinRewardUpgrade;

    public int skeletonSpawnUpgrade;
    public int skeletonHealthUpgrade;
    public int skeletonRewardUpgrade;

    public int slimeSpawnUpgrade;
    public int slimeHealthUpgrade;
    public int slimeRewardUpgrade;

    public int ogreSpawnUpgrade;
    public int ogreHealthUpgrade;
    public int ogreRewardUpgrade;


    //Towers
    public void TurretIncreaseRange()
    {
        turretRangeUpgrade += 1;
    }

    public void TurretDoubleDamage()
    {
        turretDamageUpgrade *= 2;
    }

    public void TurretDoubleSpeed()
    {
        turretSpeedUpgrade *= 2;
    }

    ////////

    public void HammerDoubleDamage()
    {
        hammerDamageUpgrade *= 2;
    }

    public void HammerDoubleSpeed()
    {
        hammerSpeedUpgrade *= 2;
    }

    ////////

    public void ElectricIncreaseRange()
    {
        electricRangeUpgrade += 1;
    }

    public void ElectricDoubleDamage()
    {
        electricDamageUpgrade *= 2;
    }

    public void ElectricDoubleSpeed()
    {
        electricSpeedUpgrade *= 2;
    }

    ////////

    public void RocketIncreaseRange()
    {
        rocketRangeUpgrade += 1;
    }

    public void RocketDoubleDamage()
    {
        rocketDamageUpgrade *= 2;
    }

    public void RocketDoubleSpeed()
    {
        rocketSpeedUpgrade *= 2;
    }

    ////////

    public void MachineGunIncreaseRange()
    {
        machineGunRangeUpgrade += 1;
    }

    public void MachineGunDoubleDamage()
    {
        machineGunDamageUpgrade *= 2;
    }

    public void MachineGunDoubleSpeed()
    {
        machineGunSpeedUpgrade *= 2;
    }

    //Monsters
    public void GoblinSpawnTimeHalf()
    {
        goblinSpawnUpgrade *= 2;
        goblinRewardUpgrade *= 2;
    }

    public void GoblinHealthDouble()
    {
        goblinHealthUpgrade *= 2;
        goblinRewardUpgrade *= 2;
    }

    /////

    public void SkeletonSpawnTimeHalf()
    {
        skeletonSpawnUpgrade *= 2;
        skeletonRewardUpgrade *= 2;
    }

    public void SkeletonHealthDouble()
    {
        goblinHealthUpgrade *= 2;
        goblinRewardUpgrade *= 2;
    }

    //////////

    public void SlimeSpawnTimeHalf()
    {
        slimeSpawnUpgrade *= 2;
        slimeRewardUpgrade *= 2;
    }

    public void SlimeHealthDouble()
    {
        slimeHealthUpgrade *= 2;
        slimeRewardUpgrade *= 2;
    }

    //////////////

    public void OgreSpawnTimeHalf()
    {
        ogreSpawnUpgrade *= 2;
        ogreRewardUpgrade *= 2;
    }

    public void OgreHealthDouble()
    {
        ogreHealthUpgrade *= 2;
        ogreRewardUpgrade *= 2;
    }
}
