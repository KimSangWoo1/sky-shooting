using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling 
{

    //풀링 오브젝트 목록
    public enum pooling{
        Cloiud
    };
    
    //풀링 대상 오브젝트 resource 참조
    private GameObject cloud;


    //풀링 셋팅
    public GameObject parent;
    public GameObject clone;

    public static ObjectPooling OP = new ObjectPooling();

    public void Creation()
    {
        
    }

    public void Push(GameObject temp)
    {

    }

    public void Pop()
    {

    }
}


