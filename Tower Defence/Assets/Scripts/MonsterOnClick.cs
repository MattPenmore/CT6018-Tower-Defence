using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MonsterOnClick : MonoBehaviour
{
    public GameObject Monster;
    GameObject[] gos;
    public Camera cam;

    public Vector3 monsterOffset;

    [SerializeField]
    Toggle[] towerButtons;

    [SerializeField]
    Toggle[] spawnerButtons;

    // Start is called before the first frame update
    void Start()
    {
        gos = GameObject.FindGameObjectsWithTag("Cell");
    }

    // Update is called once per frame
    void Update()
    {
        bool canSpawn = true;

        //If any of tower or spawner buttons are slected, can not spawn a monster
        foreach (Toggle tog in towerButtons)
        {
            if(tog.isOn)
            {
                canSpawn = false;
            }
        }

        foreach (Toggle tog in spawnerButtons)
        {
            if (tog.isOn)
            {
                canSpawn = false;
            }
        }

        //If press left mouse button and can spawn a monster
        if (Input.GetMouseButtonDown(0) && canSpawn)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                foreach (GameObject go in gos)
                {
                    //if mouse is over cell, spawn a monster at that position
                    if (GameObject.ReferenceEquals(objectHit.gameObject, go) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        if(!go.GetComponent<GridCell>().isObstacle)
                        {
                            GameObject monster = Instantiate(Monster, go.transform.position + monsterOffset, Monster.transform.rotation);
                            monster.transform.parent = null;
                        }
                    }
                }
            }
        }
    }
}
