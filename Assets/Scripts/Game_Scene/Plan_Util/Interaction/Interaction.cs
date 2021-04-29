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
            messageSender.ApplyDamage(other.gameObject.GetComponent<Bullet_Move>().Get_ProfileName());
        }
        else
        {
            planeBase.FXM.FX_ItemPop(transform);
            if (other.transform.parent.tag == "Item_Bullet")
            {
                messageSender.Apply_AddBullet();
            }
            else if (other.transform.parent.tag == "Item_Muzzle")
            {
                messageSender.Apply_AddMuzzle();
            }
            else if (other.transform.parent.tag == "Item_Turbin")
            {
                messageSender.Apply_AddTurbin();
            }
            else if (other.transform.parent.tag == "Item_Health")
            {
                messageSender.Apply_AddHealth(other.transform.parent.GetComponent<ItemControl>().healthState);
            }
            else if (other.transform.parent.tag == "Item_Dollar")
            {
                //추가해야함??
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
