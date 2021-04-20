using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ObjectPooling
{ 
    public enum Item_State{Bullet, Muzzle, Turbin, Health, Dollar}; //오브젝트 풀링 상태

    private Item_State item_State;

    private int item_BulletSize = 10; //아이템 총알 사이즈
    private int item_MuzzleSize = 10; //아이템 머즐 사이즈
    private int item_TurbinSize = 10; //아이템 터빈 사이즈
    private int item_HelathSize = 10; //아이템 HP 사이즈
    private int item_DollarSize = 10; //아이템 돈 사이즈

    //풀링 셋팅
    private GameObject item_BulletParent; //아이템 총알 부모
    private GameObject item_MuzzleParent;  //아이템 머즐 부모
    private GameObject item_TurbinParent;  //아이템 터빈 부모

    // 비행기 업그레이드 아이템 Pool
    private Queue<GameObject> item_BulletPool = new Queue<GameObject>(); // item 총알 Pool
    private Queue<GameObject> item_MuzzlePool = new Queue<GameObject>(); // Item 머즐 Pool
    private Queue<GameObject> item_TurbinPool = new Queue<GameObject>(); // Item 터빈 Pool

    //체력 아이템 Pool
    private Queue<GameObject> item_RedHealthPool = new Queue<GameObject>(); // item 빨강 HP Pool
    private Queue<GameObject> FX_YellowHealthPool = new Queue<GameObject>(); // item 노랑 HP Pool
    private Queue<GameObject> FX_GreenHealthPool = new Queue<GameObject>(); // item 초록 HP Pool
    //돈 아이템 Pool
    private Queue<GameObject> item_RedDollarPool = new Queue<GameObject>(); // item 빨강 돈 Pool
    private Queue<GameObject> FX_YellowDollarPool = new Queue<GameObject>(); // item 노랑 돈 Pool
    private Queue<GameObject> FX_GreenDollarPool = new Queue<GameObject>(); // item 초록 돈 Pool


    //풀링 대상 오브젝트 resource 참조
    public void Set_PlaneItemState(Item_State _state)
    {
        item_State = _state;
        switch (item_State)
        {
            case Item_State.Bullet:
                prefab = Resources.Load("Prefab/Clouds") as GameObject;
                break;
            case Item_State.Muzzle:
                prefab = Resources.Load("Prefab/Plane/Bullets/Bullet") as GameObject;
                break;
            case Item_State.Turbin:
                prefab = Resources.Load("Prefab/Plane/Bullets/Bullet") as GameObject;
                break;
        }
    }

    /*
    //죽음 상태
    public void Set_HealthState(Pooling_State _state, FX_State subState, DeadState tubState)
    {
        state = _state;
        fx_State = subState;
        deadState = tubState;

        switch (fx_State)
        {
            case FX_State.Dead:
                switch (deadState)
                {
                    case DeadState.None:
                        break;
                    case DeadState.Red:
                        prefab = Resources.Load("Particle/FX_Prefab/Dead/FX_RedDead") as GameObject;
                        break;
                    case DeadState.Green:
                        prefab = Resources.Load("Particle/FX_Prefab/Dead/FX_GreenDead") as GameObject;
                        break;
                    case DeadState.Blue:
                        prefab = Resources.Load("Particle/FX_Prefab/Dead/FX_BlueDead") as GameObject;
                        break;
                    case DeadState.Orange:
                        prefab = Resources.Load("Particle/FX_Prefab/Dead/FX_OrangeDead") as GameObject;
                        break;
                }
                break;
        }
    }

    public Pooling_State getState()
    {
        return state;
    }

    #region 주요 오브젝트 (총알,구름) 생성
    //오브젝트 생성
    public void Creation()
    {
        switch (state)
        {
            case Pooling_State.Cloud:
                if (cloudParent == null || !cloudParent.activeInHierarchy)
                {
                    cloudParent = GameObject.Find("구름Pool");
                    if (cloudParent == null)
                    {
                        cloudParent = new GameObject();
                        cloudParent.transform.name = "구름Pool";
                    }
                }

                for (int i = 0; i < cloudSize; i++)
                {
                    if (prefab == null)
                    {
                        Set_State(Pooling_State.Cloud);
                    }
                    clone = GameObject.Instantiate(prefab, cloudParent.transform.position, Quaternion.identity, cloudParent.transform);
                    clone.SetActive(false);
                    cloudPool.Enqueue(clone);
                }
                break;
            case Pooling_State.Bullet:
                if (bulletParent == null || !bulletParent.activeInHierarchy)
                {
                    bulletParent = GameObject.Find("총알Pool");
                    if (bulletParent == null)
                    {
                        bulletParent = new GameObject();
                        bulletParent.transform.name = "총알Pool";
                    }
                }

                for (int i = 0; i < bulletSize; i++)
                {
                    if (prefab == null)
                    {
                        Set_State(Pooling_State.Bullet);
                    }
                    clone = GameObject.Instantiate(prefab, bulletParent.transform.position, Quaternion.Euler(-90f, 0f, 0f), bulletParent.transform);
                    clone.SetActive(false);
                    bulletPool.Enqueue(clone);
                }
                break;
        }
    }
    #endregion

    #region 아이템 생성
    public void Item_Creation()
    {
        switch (state)
        {
            case Pooling_State.Item:
                if (FX_ItemParent == null || !FX_ItemParent.activeInHierarchy)
                {
                    FX_ItemParent = GameObject.Find("FX_ItemPool");
                    if (FX_ItemParent == null)
                    {
                        FX_ItemParent = new GameObject();
                        FX_ItemParent.transform.name = "FX_ItemPool";
                    }
                }

                for (int i = 0; i < FX_ItemSize; i++)
                {
                    if (prefab == null)
                    {
                        Set_State(Pooling_State.Item);
                    }
                    clone = GameObject.Instantiate(prefab, FX_ItemParent.transform.position, Quaternion.Euler(0f, 0f, 0f), FX_ItemParent.transform);
                    clone.SetActive(false);
                    FX_ItemPool.Enqueue(clone);
                }
                break;

        }
    }
    #endregion

    #region PUSH
    //오브젝트 넣기
    public void Push(GameObject temp)
    {
        //Debug.Log("push");
        if (temp.activeSelf)
        {
            temp.SetActive(false);
        }
        switch (state)
        {
            case Pooling_State.Cloud:
                cloudPool.Enqueue(temp);
                break;
            case Pooling_State.Bullet:
                bulletPool.Enqueue(temp);
                break;
        }
    }

    #endregion

    #region POP
    //오브젝트 빼기
    public GameObject Pop()
    {
        GameObject temp;
        switch (state)
        {
            case Pooling_State.Cloud:
                if (cloudPool.Count == 0)
                {
                    Creation();
                }
                temp = cloudPool.Dequeue();
                break;
            case Pooling_State.Bullet:
                if (bulletPool.Count == 0)
                {
                    Creation();
                }
                temp = bulletPool.Dequeue();
                break;
            default:
                temp = null;
                break;
        }
        return temp;
    }
    #endregion
    */
}


