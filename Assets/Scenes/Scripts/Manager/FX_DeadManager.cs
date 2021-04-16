using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_DeadManager : Singleton<FX_DeadManager>
{
    private static ObjectPooling FX_RedDeadPool = new ObjectPooling();// FX_RedDead Pool
    private static ObjectPooling FX_GreenDeadPool = new ObjectPooling();// FX_GreenDead Pool
    private static ObjectPooling FX_BlueDeadPool = new ObjectPooling(); // FX_BlueDead Pool
    private static ObjectPooling FX_OrangeDeadPool = new ObjectPooling(); // FX_OrangeDead Pool

    //총알 오브젝트 셋팅
    void Start()
    {
        FX_RedDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Red);
        FX_RedDeadPool.FX_Creation();

        FX_GreenDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Green);
        FX_GreenDeadPool.FX_Creation();

        FX_BlueDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Blue);
        FX_BlueDeadPool.FX_Creation();

        FX_OrangeDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.Dead, ObjectPooling.DeadState.Orange);
        FX_OrangeDeadPool.FX_Creation();
    }

    //총알 정리
    internal void FX_Push(GameObject FX_Item, ObjectPooling.DeadState _deadState)
    {
        FX_Item.SetActive(false);

        switch (_deadState)
        {
            case ObjectPooling.DeadState.None:
                break;
            case ObjectPooling.DeadState.Red:
                FX_RedDeadPool.FX_Push(FX_Item);
                break;
            case ObjectPooling.DeadState.Green:
                FX_GreenDeadPool.FX_Push(FX_Item);
                break;
            case ObjectPooling.DeadState.Blue:
                FX_BlueDeadPool.FX_Push(FX_Item);
                break;
            case ObjectPooling.DeadState.Orange:
                FX_OrangeDeadPool.FX_Push(FX_Item);
                break;
        }
    }

    //총 발사
    internal void FX_Pop(Transform deadObject, ObjectPooling.DeadState _deadState)
    {
        GameObject FX_Dead =null;
        switch (_deadState)
        {
            case ObjectPooling.DeadState.None:              
                break;
            case ObjectPooling.DeadState.Red:
                if (FX_RedDeadPool.getState() != ObjectPooling.Pooling_State.FX)
                {
                    FX_RedDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.item, ObjectPooling.DeadState.Red);
                }
                FX_Dead = FX_RedDeadPool.FX_Pop();
                break;
            case ObjectPooling.DeadState.Green:
                if (FX_GreenDeadPool.getState() != ObjectPooling.Pooling_State.FX)
                {
                    FX_GreenDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.item, ObjectPooling.DeadState.Red);
                }
                FX_Dead = FX_GreenDeadPool.FX_Pop();
                break;
            case ObjectPooling.DeadState.Blue:
                if (FX_BlueDeadPool.getState() != ObjectPooling.Pooling_State.FX)
                {
                    FX_BlueDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.item, ObjectPooling.DeadState.Red);
                }
                FX_Dead = FX_BlueDeadPool.FX_Pop();
                break;
            case ObjectPooling.DeadState.Orange:
                if (FX_OrangeDeadPool.getState() != ObjectPooling.Pooling_State.FX)
                {
                    FX_OrangeDeadPool.Set_FX_DeadState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.item, ObjectPooling.DeadState.Red);
                }
                FX_Dead = FX_OrangeDeadPool.FX_Pop();
                break;
        }
        if (FX_Dead != null)
        {
            FX_Dead.transform.position = deadObject.position;
            FX_Dead.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            FX_Dead.SetActive(true);
        }
    }
}
