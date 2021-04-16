using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ItemManager : Singleton<FX_ItemManager>
{
    private static ObjectPooling  FX_ItemPooling = new ObjectPooling();

    //총알 오브젝트 셋팅
    void Start()
    {
        FX_ItemPooling.Set_FX_ItemState(ObjectPooling.Pooling_State.FX, ObjectPooling.FX_State.item);
        FX_ItemPooling.FX_Creation();
    }

    //총알 정리
    internal void FX_ItemPush(GameObject FX_Item)
    {
        FX_Item.SetActive(false);
        FX_ItemPooling.FX_Push(FX_Item);
    }

    //총 발사
    internal void FX_ItemPop(Transform EatObject)
    {
        if (FX_ItemPooling.getState() != ObjectPooling.Pooling_State.FX)
        {
            FX_ItemPooling.Set_FX_ItemState(ObjectPooling.Pooling_State.FX,ObjectPooling.FX_State.item);
        }

        GameObject FX_Item = FX_ItemPooling.FX_Pop();
        FX_Item.transform.position = EatObject.position;
        FX_Item.transform.rotation = EatObject.rotation;
        FX_Item.SetActive(true);
    }
}
