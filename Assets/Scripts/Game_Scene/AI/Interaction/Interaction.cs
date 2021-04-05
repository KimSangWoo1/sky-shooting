using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item_Interaction))]
public partial class Interaction : MonoBehaviour
{
    private Item_Interaction itemInteraction;
    
    private float damage;

    private void Start()
    {
        itemInteraction = GetComponent<Item_Interaction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item_Bullet")
        {
            itemInteraction.Apply_AddBullet();
        }
        else if (other.gameObject.tag == "Item_Muzzle")
        {
            itemInteraction.Apply_AddMuzzle();
        }
        else if (other.gameObject.tag == "Item_Turbin")
        {
            itemInteraction.Apply_AddTurbin();
        }
        else if (other.gameObject.tag == "Item_Health")
        {
             itemInteraction.Apply_AddHealth(other.gameObject);
        }
        else if (other.gameObject.tag == "Item_Dollar")
        {
            //추가해야함??
        }
        other.transform.parent.gameObject.SetActive(false);
    }

    //데미지 적용
    public float ApplyDamage()
    {
        damage = 0.1f;

        return damage;
    }
    //자폭
    public void Self_Destruction()
    {

    }
    //초기화
    public void Reset_Interaction()
    {

    }
}
