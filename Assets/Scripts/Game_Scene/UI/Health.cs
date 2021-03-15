using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image hp;
    private float current_HP;


    public void Add_Health(GameObject item)
    {
        if (item.transform.parent.name == "Health_Red")
        {
            Add_RedHealth();
        }
        else if (item.transform.parent.name == "Health_Yellow")
        {
            Add_YellowHealth();
        }
        else if (item.transform.parent.name == "Health_Green")
        {
            Add_GreenHealth();
        }
    }

    private void Bullet_Damage()
    {
        hp.fillAmount -= 0.2f;
    }

    private void Add_RedHealth()
    {
        hp.fillAmount += 0.3f;
    }

    private void Add_YellowHealth()
    {
        hp.fillAmount += 0.6f;
    }

    private void Add_GreenHealth()
    {
        hp.fillAmount += 1f;
    }
}
