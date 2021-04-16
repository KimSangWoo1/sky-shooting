﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ObjectPooling : MonoBehaviour
{
    public enum FX_State { item, Dead };
    public enum DeadState {None, Red, Green, Blue, Orange };

    private FX_State fx_State;
    private DeadState deadState;

    private int FX_ItemSize = 10; //아이템 첫 사이즈
    private int FX_DeadSize = 5;  //죽음 첫 사이즈

    private GameObject FX_ItemParent;  //FX 아이템 부모
    private GameObject FX_DeadParent;  //FX 죽음 부모

    // 아이템 Pool
    private Queue<GameObject> FX_ItemPool = new Queue<GameObject>(); // FX_Item Pool

    //죽음 Pool
    private Queue<GameObject> FX_RedDeadPool = new Queue<GameObject>(); // FX_RedDead Pool
    private Queue<GameObject> FX_GreenDeadPool = new Queue<GameObject>(); // FX_GreenDead Pool
    private Queue<GameObject> FX_BlueDeadPool = new Queue<GameObject>(); // FX_BlueDead Pool
    private Queue<GameObject> FX_OrangeDeadPool = new Queue<GameObject>(); // FX_OrangeDead Pool

    #region 상태조정
    //아이템 상태
    public void Set_FX_ItemState(Pooling_State _state, FX_State subState)
    {
        state = _state;
        fx_State = subState;
        switch (fx_State)
        {
            case FX_State.item:
                prefab = Resources.Load("Particle/FX_Prefab/FX_Rainbow") as GameObject;
                break;
        }
    }
    //죽음 상태
    public void Set_FX_DeadState(Pooling_State _state, FX_State subState, DeadState tubState)
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
    #endregion
    #region FX 생성
    //오브젝트 생성
    public void FX_Creation()
    {
        switch (state)
        {
            case Pooling_State.FX:
                switch (fx_State)
                {
                    case FX_State.item:
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
                                Set_State(Pooling_State.FX);
                            }
                            clone = GameObject.Instantiate(prefab, FX_ItemParent.transform.position, Quaternion.Euler(0f, 0f, 0f), FX_ItemParent.transform);
                            clone.SetActive(false);
                            FX_ItemPool.Enqueue(clone);
                        }
                        break;
                    case FX_State.Dead:
                        if (FX_DeadParent == null || !FX_DeadParent.activeInHierarchy)
                        {
                            FX_DeadParent = GameObject.Find("FX_DeadPool");
                            if (FX_DeadParent == null)
                            {
                                FX_DeadParent = new GameObject();
                                FX_DeadParent.transform.name = "FX_DeadPool";
                            }
                        }

                        for (int i = 0; i < FX_DeadSize; i++)
                        {
                            if (prefab == null)
                            {
                                Set_State(Pooling_State.FX);
                            }
                            clone = GameObject.Instantiate(prefab, FX_DeadParent.transform.position, Quaternion.Euler(0f, 0f, 0f), FX_DeadParent.transform);
                            clone.SetActive(false);
                            switch (deadState)
                            {
                                case DeadState.None:
                                    break;
                                case DeadState.Red:
                                    FX_RedDeadPool.Enqueue(clone);
                                    break;
                                case DeadState.Green:
                                    FX_GreenDeadPool.Enqueue(clone);
                                    break;
                                case DeadState.Blue:
                                    FX_BlueDeadPool.Enqueue(clone);
                                    break;
                                case DeadState.Orange:
                                    FX_OrangeDeadPool.Enqueue(clone);
                                    break;
                            }
                        }
                        break;
                }
                break;

            default:
                break;
        }
    }
    #endregion

    #region PUSH
    // FX Push
    public void FX_Push(GameObject temp)
    {
        //Debug.Log("push");
        if (temp.activeSelf)
        {
            temp.SetActive(false);
        }
        switch (state)
        {
            case Pooling_State.FX:
                switch (fx_State)
                {
                    case FX_State.item:
                        FX_ItemPool.Enqueue(temp);
                        break;
                    case FX_State.Dead:
                        switch (deadState)
                        {
                            case DeadState.None:
                                break;
                            case DeadState.Red:
                                FX_RedDeadPool.Enqueue(clone);
                                break;
                            case DeadState.Green:
                                FX_GreenDeadPool.Enqueue(clone);
                                break;
                            case DeadState.Blue:
                                FX_BlueDeadPool.Enqueue(clone);
                                break;
                            case DeadState.Orange:
                                FX_OrangeDeadPool.Enqueue(clone);
                                break;
                        }
                        break;
                }
                break;
        }
    }
    #endregion

    #region POP
    public GameObject FX_Pop()
    {
        GameObject temp;
        switch (state)
        {
            case Pooling_State.FX:
                switch (fx_State)
                {
                    case FX_State.item:
                        if (FX_ItemPool.Count == 0)
                        {
                            FX_Creation();
                        }
                        temp = FX_ItemPool.Dequeue();

                        break;
                    case FX_State.Dead:
                        switch (deadState)
                        {
                            case DeadState.None:
                                temp = null;
                                break;
                            case DeadState.Red:
                                if (FX_RedDeadPool.Count == 0)
                                {
                                    FX_Creation();
                                }
                                temp = FX_RedDeadPool.Dequeue();
                                break;
                            case DeadState.Green:
                                if (FX_GreenDeadPool.Count == 0)
                                {
                                    FX_Creation();
                                }
                                temp = FX_GreenDeadPool.Dequeue();
                                break;
                            case DeadState.Blue:
                                if (FX_BlueDeadPool.Count == 0)
                                {
                                    FX_Creation();
                                }
                                temp = FX_BlueDeadPool.Dequeue();
                                break;
                            case DeadState.Orange:
                                if (FX_OrangeDeadPool.Count == 0)
                                {
                                    FX_Creation();
                                }
                                temp = FX_OrangeDeadPool.Dequeue();
                                break;
                            default:
                                temp = null;
                                break;
                        }
                        break;
                    default:
                        temp = null;
                        break;
                }
                break;
            default:
                temp = null;
                break;
        }
        return temp;
    }
    #endregion
}
