using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeMenuController : MonoBehaviour
{
    public List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buttons = buttons.OrderBy(x => x.GetComponent<UpgradeButton>().cost).ToList();

        int i = 0;

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
