using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MessageSender))]
public partial class Interaction : MonoBehaviour
{
    private MessageSender messageSender;
    
    private float damage;

    private void Start()
    {
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
            messageSender.Self_Destruction();
        }
        else if (collision.gameObject.tag == "AI")
        {
            messageSender.Self_Destruction();
        }
    }


}
