using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeMenuController : MonoBehaviour
{
    public List<GameObject> buttons;

    // Update is called once per frame
    void Update()
    {
        //Order upgrade buttons by cost
        buttons = buttons.OrderBy(x => x.GetComponent<UpgradeButton>().cost).ToList();

        int i = 0;

        //Set order of buttons as child of parent by their cost
        foreach(GameObject button in buttons)
        {
            button.transform.SetSiblingIndex(i);
            if(button.GetComponent<UpgradeButton>().isAvailable && !button.GetComponent<UpgradeButton>().hasBeenPressed)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }

            i++;
        }
    }
}
