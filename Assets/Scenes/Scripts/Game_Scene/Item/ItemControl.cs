using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    ItemManager IM;
    [SerializeField]
    private ObjectPooling.Item_State item_State;
    [SerializeField]
    private ObjectPooling.Item_HealthState health_State;
    [SerializeField]
    private ObjectPooling.Item_DollarState dollar_State;

    void Start()
    {
        IM = ItemManager.Instance;

    }

    private void Update()
    {
       
    }
    public void OnTriggerEnter(Collider other)
    {
        switch (item_State)
        {
            case ObjectPooling.Item_State.Bullet:
            case ObjectPooling.Item_State.Muzzle:
            case ObjectPooling.Item_State.Turbin:
                IM.Item_UpgradePush(this.gameObject, item_State); //Push 및 active 설정
                break;
            case ObjectPooling.Item_State.Health:
                switch (health_State)
                {
                    case ObjectPooling.Item_HealthState.None:
                        break;
                    case ObjectPooling.Item_HealthState.Red:
                    case ObjectPooling.Item_HealthState.Yellow:
                    case ObjectPooling.Item_HealthState.Green:
                        IM.Item_HealthPush(this.gameObject, health_State); //Push 및 active 설정
                        break;
                }
                break;
            case ObjectPooling.Item_State.Dollar:
                switch (dollar_State)
                {
                    case ObjectPooling.Item_DollarState.None:
                        break;
                    case ObjectPooling.Item_DollarState.Red:
                    case ObjectPooling.Item_DollarState.Yellow:
                    case ObjectPooling.Item_DollarState.Green:
                        IM.Item_DollarPush(this.gameObject, dollar_State); //Push 및 active 설정
                        break;
                }
                break;
        }

        this.gameObject.SetActive(false);
    }
}
