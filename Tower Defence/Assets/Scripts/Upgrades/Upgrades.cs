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
    }

    public void OgreHealthDouble()
    {
        ogreHealthUpgrade *= 2;
        ogreRewardUpgrade *= 2;
    }

    //Give upgrade variables to save file
    public List<int> GetVariables()
    {
        List<int> list = new List<int>();
        list.Add(turretRangeUpgrade);
        list.Add(turretDamageUpgrade);
        list.Add(turretSpeedUpgrade);

        list.Add(hammerDamageUpgrade);
        list.Add(hammerSpeedUpgrade);

        list.Add(electricRangeUpgrade);
        list.Add(electricDamageUpgrade);
        list.Add(electricSpeedUpgrade);

        list.Add(rocketRangeUpgrade);
        list.Add(rocketDamageUpgrade);
        list.Add(rocketSpeedUpgrade);

        list.Add(machineGunRangeUpgrade);
        list.Add(machineGunDamageUpgrade);
        list.Add(machineGunSpeedUpgrade);

        list.Add(goblinSpawnUpgrade);
        list.Add(goblinHealthUpgrade);
        list.Add(goblinRewardUpgrade);

        list.Add(skeletonSpawnUpgrade);
        list.Add(skeletonHealthUpgrade);
        list.Add(skeletonRewardUpgrade);

        list.Add(slimeSpawnUpgrade);
        list.Add(slimeHealthUpgrade);
        list.Add(slimeRewardUpgrade);

        list.Add(ogreSpawnUpgrade);
        list.Add(ogreHealthUpgrade);
        list.Add(ogreRewardUpgrade);

        return list;
    }

    //Get upgrade variables from save file
    public void SetVariables(List<int> list)
    {
        turretRangeUpgrade = list[0];
        turretDamageUpgrade = list[1];
        turretSpeedUpgrade = list[2];

        hammerDamageUpgrade = list[3];
        hammerSpeedUpgrade = list[4];

        electricRangeUpgrade = list[5];
        electricDamageUpgrade = list[6];
        electricSpeedUpgrade = list[7];

        rocketRangeUpgrade = list[8];
        rocketDamageUpgrade = list[9];
        rocketSpeedUpgrade = list[10];

        machineGunRangeUpgrade = list[11];
        machineGunDamageUpgrade = list[12];
        machineGunSpeedUpgrade = list[13];


        goblinSpawnUpgrade = list[14];
        goblinHealthUpgrade = list[15];
        goblinRewardUpgrade = list[16];

        skeletonSpawnUpgrade = list[17];
        skeletonHealthUpgrade = list[18];
        skeletonRewardUpgrade = list[19];

        slimeSpawnUpgrade = list[20];
        slimeHealthUpgrade = list[21];
        slimeRewardUpgrade = list[22];

        ogreSpawnUpgrade = list[23];
        ogreHealthUpgrade = list[24];
        ogreRewardUpgrade = list[25];
    }
}
