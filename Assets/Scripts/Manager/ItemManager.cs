using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ItemManager : Singleton<ItemManager>
{
    // 비행기 업그레이드 아이템 Pool
    private  ObjectPooling item_BulletPooling = new ObjectPooling();
    private  ObjectPooling item_MuzzlePooling = new ObjectPooling();
    private  ObjectPooling item_TurbinPooling = new ObjectPooling();
    // HP 아이템 Pool
    private  ObjectPooling item_RedHealthPooling = new ObjectPooling();
    private  ObjectPooling item_YellowHealthPooling = new ObjectPooling();
    private  ObjectPooling item_GreenHealthPooling = new ObjectPooling();
    // 돈 아이템 Pool
    private  ObjectPooling item_RedDollarPooling = new ObjectPooling();
    private  ObjectPooling item_YellowDollarPooling = new ObjectPooling();
    private  ObjectPooling item_GreenDollarPooling = new ObjectPooling();

    private void Awake()
    {

    }
    void Start()
    {
        // 비행기 업그레이드 아이템 생성
        item_BulletPooling.Set_UpgradeItemState(ObjectPooling.Item_State.Bullet);
        item_BulletPooling.Item_Creation();

        item_MuzzlePooling.Set_UpgradeItemState(ObjectPooling.Item_State.Muzzle);
        item_MuzzlePooling.Item_Creation();

        item_TurbinPooling.Set_UpgradeItemState(ObjectPooling.Item_State.Turbin);
        item_TurbinPooling.Item_Creation();

        //HP 아이템 생성
        item_RedHealthPooling.Set_HealthState(ObjectPooling.Item_State.Health, ObjectPooling.Item_HealthState.Red);
        item_RedHealthPooling.Item_Creation();

        item_YellowHealthPooling.Set_HealthState(ObjectPooling.Item_State.Health, ObjectPooling.Item_HealthState.Yellow);
        item_YellowHealthPooling.Item_Creation();

        item_GreenHealthPooling.Set_HealthState(ObjectPooling.Item_State.Health, ObjectPooling.Item_HealthState.Green);
        item_GreenHealthPooling.Item_Creation();

        // 돈 아이템 생성
        item_RedDollarPooling.Set_DollarState(ObjectPooling.Item_State.Dollar, ObjectPooling.Item_DollarState.Red);
        item_RedDollarPooling.Item_Creation();

        item_YellowDollarPooling.Set_DollarState(ObjectPooling.Item_State.Dollar, ObjectPooling.Item_DollarState.Yellow);
        item_YellowDollarPooling.Item_Creation();

        item_GreenDollarPooling.Set_DollarState(ObjectPooling.Item_State.Dollar, ObjectPooling.Item_DollarState.Green);
        item_GreenDollarPooling.Item_Creation();
    }
}
