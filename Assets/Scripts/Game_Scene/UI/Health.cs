using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image hp;

    public void ChaneHP(int amount)
    {
        float current_HP = amount / 100f;
        hp.fillAmount = amount;
    }
}
