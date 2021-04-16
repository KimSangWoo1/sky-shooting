using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ItemManager : Singleton<FX_ItemManager>
{
    private static ObjectPooling  FX_ItemPooling = new ObjectPooling();

    //총알 오브젝트 셋팅
    void Start()
    {
        FX_ItemPooling.setState(ObjectPooling.Pooling_State.FX_Item);
        FX_ItemPooling.Creation();
    }

    //총알 정리
    internal void Item_Push(GameObject FX_Item)
    {
        FX_Item.SetActive(false);
        FX_ItemPooling.Push(FX_Item);
    }

    //총 발사
    internal void Item_Pop(Transform deadObject)
    {
        if (FX_ItemPooling.getState() != ObjectPooling.Pooling_State.FX_Item)
        {
            FX_ItemPooling.setState(ObjectPooling.Pooling_State.FX_Item);
        }

        GameObject FX_Item = FX_ItemPooling.Pop();
        FX_Item.transform.position = deadObject.position;
        FX_Item.transform.rotation = deadObject.rotation;
        FX_Item.SetActive(true);
    }
}
