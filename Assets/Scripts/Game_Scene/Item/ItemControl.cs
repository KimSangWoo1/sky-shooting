using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    ItemManager IM;

    public ObjectPooling.Item_State itemState;
    public ObjectPooling.Item_HealthState healthState;
    public ObjectPooling.Item_DollarState dollarState;

    void Start()
    {
        IM = ItemManager.Instance;

    }

    private void Update()
    {
       
    }
    public void OnTriggerEnter(Collider other)
    {
        switch (itemState)
        {
            case ObjectPooling.Item_State.Bullet:
            case ObjectPooling.Item_State.Muzzle:
            case ObjectPooling.Item_State.Turbin:
                IM.Item_UpgradePush(this.gameObject, itemState); //Push 및 active 설정
                break;
            case ObjectPooling.Item_State.Health:
                switch (healthState)
                {
                    case ObjectPooling.Item_HealthState.None:
                        break;
                    case ObjectPooling.Item_HealthState.Red:
                    case ObjectPooling.Item_HealthState.Yellow:
                    case ObjectPooling.Item_HealthState.Green:
                        IM.Item_HealthPush(this.gameObject, healthState); //Push 및 active 설정
                        break;
                }
                break;
            case ObjectPooling.Item_State.Dollar:
                switch (dollarState)
                {
                    case ObjectPooling.Item_DollarState.None:
                        break;
                    case ObjectPooling.Item_DollarState.Red:
                    case ObjectPooling.Item_DollarState.Yellow:
                    case ObjectPooling.Item_DollarState.Green:
                        IM.Item_DollarPush(this.gameObject, dollarState); //Push 및 active 설정
                        break;
                }
                break;
        }

        this.gameObject.SetActive(false);
    }
}
