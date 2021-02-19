﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling 
{
    //풀링 오브젝트 목록
    public enum pooling{
        Cloud
    };

    public static ObjectPooling cloudPooling = new ObjectPooling();

    public int cloudSize = 3;
 
    //풀링 대상 오브젝트 resource 참조
    private GameObject clouds =Resources.Load("Prefab/Clouds") as GameObject;

    //풀링 셋팅
    private GameObject parent;
    private GameObject clone;
    
    //실제 풀 리스트들
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Creation()
    {
        if (parent==null || !parent.activeInHierarchy)
        {
            parent = new GameObject();
            parent.transform.name = "구름Pool";
            parent.transform.position = new Vector3(20f, 30f, -680f);
        }

        for (int i = 0; i < cloudSize; i++)
        {
            clone = GameObject.Instantiate(clouds, Vector3.zero, Quaternion.identity, parent.transform);
            clone.SetActive(false);
            pool.Enqueue(clone);
        }
    }

    public  void Push(GameObject temp)
    {
        if (temp.activeSelf)
        {
            temp.SetActive(false);
        }
        pool.Enqueue(temp);
        Pop();
    }

    public  void Pop()
    {
        if (pool.Count == 0)
        {
            Creation();
        }
        GameObject temp = pool.Dequeue();
        temp.transform.position = parent.transform.position;
        temp.SetActive(true);
    }
}

