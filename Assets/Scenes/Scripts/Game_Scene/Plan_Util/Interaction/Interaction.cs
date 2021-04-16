using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MessageSender))]
public partial class Interaction : MonoBehaviour
{
    private FX_ItemManager FX_IM;
    private FX_DeadManager FX_DM;
    private MessageSender messageSender;

    private PlaneBase planeBase;
    private void Start()
    {
        FX_IM = FX_ItemManager.Instance;
        FX_DM = FX_DeadManager.Instance;

        planeBase = GetComponent<PlaneBase>();
        messageSender = GetComponent<MessageSender>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            messageSender.ApplyDamage();
        }
        else
        {
            FX_IM.FX_ItemPop(transform);
            if (other.gameObject.tag == "Item_Bullet")
            {
                messageSender.Apply_AddBullet();
            }
            else if (other.gameObject.tag == "Item_Muzzle")
            {
                messageSender.Apply_AddMuzzle();
            }
            else if (other.gameObject.tag == "Item_Turbin")
            {
                messageSender.Apply_AddTurbin();
            }
            else if (other.gameObject.tag == "Item_Health")
            {
                messageSender.Apply_AddHealth(other.gameObject);
            }
            else if (other.gameObject.tag == "Item_Dollar")
            {
                //추가해야함??
            }
            other.transform.parent.gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FX_DM.FX_Pop(transform, planeBase.deadState);
            messageSender.Self_Destruction();
        }
        else if (collision.gameObject.tag == "AI")
        {
            FX_DM.FX_Pop(transform, planeBase.deadState);
            messageSender.Self_Destruction();
        }else if (collision.gameObject.tag == "Wall")
        {
            FX_DM.FX_Pop(transform, planeBase.deadState);
            messageSender.Self_Destruction();
        }
    }


}
