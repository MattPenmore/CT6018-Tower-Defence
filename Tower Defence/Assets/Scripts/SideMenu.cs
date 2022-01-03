using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SideMenu : MonoBehaviour
{
    public GameObject towersMenu;
    public GameObject spawnersMenu;

    [SerializeField]
    Toggle[] towerButtons;

    [SerializeField]
    Toggle[] spawnerButtons;

    public void OpenTowersMenu()
    {
        towersMenu.SetActive(true);
        spawnersMenu.SetActive(false);

        foreach(Toggle tog in spawnerButtons)
        {
            tog.isOn = false;
        }
    }

    public void OpenSpawnersMenu()
    {
        towersMenu.SetActive(false);
        spawnersMenu.SetActive(true);

        foreach (Toggle tog in towerButtons)
        {
            tog.isOn = false;
        }
    }
}
