using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FX_Manager : Singleton<FX_Manager>
{
    private static ObjectPooling FX_ItemPooling = new ObjectPooling();
    private static ObjectPooling FX_RedDeadPool = new ObjectPooling();// FX_RedDead Pool
    private static ObjectPooling FX_GreenDeadPool = new ObjectPooling();// FX_GreenDead Pool
    private static ObjectPooling FX_BlueDeadPool = new ObjectPooling(); // FX_BlueDead Pool
    private static ObjectPooling FX_OrangeDeadPool = new ObjectPooling(); // FX_OrangeDead Pool

    void Start()
    {
        //FX Item 생성
        FX_ItemPooling.Set_FX_ItemState(ObjectPooling.FX_State.item);
        FX_ItemPooling.FX_Creation();

        //FX Dead 생성
        FX_RedDeadPool.Set_FX_DeadState(ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Red);
        FX_RedDeadPool.FX_Creation();

        FX_GreenDeadPool.Set_FX_DeadState(ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Green);
        FX_GreenDeadPool.FX_Creation();

        FX_BlueDeadPool.Set_FX_DeadState(ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Blue);
        FX_BlueDeadPool.FX_Creation();

        FX_OrangeDeadPool.Set_FX_DeadState(ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Orange);
        FX_OrangeDeadPool.FX_Creation();
    }

}
