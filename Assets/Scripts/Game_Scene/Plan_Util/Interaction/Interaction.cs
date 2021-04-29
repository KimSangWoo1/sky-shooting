using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MessageSender))]
public partial class Interaction : MonoBehaviour
{
    private MessageSender messageSender;

    private PlaneBase planeBase;
    private void Start()
    {
        planeBase = GetComponent<PlaneBase>();
        messageSender = GetComponent<MessageSender>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            messageSender.ApplyDamage(other.gameObject.GetComponent<BulletController>().Get_ProfileName());
        }
        else
        {       
            if (other.transform.parent.tag == "Item_Bullet")
            {
                planeBase.FXM.FX_ItemPop(transform);
                messageSender.Apply_AddBullet();
            }
            else if (other.transform.parent.tag == "Item_Muzzle")
            {
                planeBase.FXM.FX_ItemPop(transform);
                messageSender.Apply_AddMuzzle();
            }
            else if (other.transform.parent.tag == "Item_Turbin")
            {
                planeBase.FXM.FX_ItemPop(transform);
                messageSender.Apply_AddTurbin();
            }
            else if (other.transform.parent.tag == "Item_Health")
            {
                planeBase.FXM.FX_ItemPop(transform);
                messageSender.Apply_AddHealth(other.transform.parent.GetComponent<ItemControl>().healthState);
            }
            else if (other.transform.parent.tag == "Item_Dollar")
            {
                planeBase.FXM.FX_MoneyPop(transform);
                messageSender.Apply_AddMoney(other.transform.parent.GetComponent<ItemControl>().dollarState);
            }     
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            messageSender.Self_Destruction();
        }
        else if (collision.gameObject.tag == "AI")
        {
            messageSender.Self_Destruction();
        }else if (collision.gameObject.tag == "Wall")
        {
            messageSender.Self_Destruction();
        }
    }


}
