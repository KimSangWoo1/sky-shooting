using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image hp;

    public void ChaneHP(float amount)
    {
        hp.fillAmount = amount;
    }
}
