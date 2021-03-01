using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : Singleton<CloudManager>
{
    public GameObject startPoint; //시작지점
    private static ObjectPooling cloudPooling = new ObjectPooling(); 
    private GameObject clone;
    void Start()
    {
        if (startPoint == null)
        {
            startPoint = Resources.Load("Prefab/Cloud_StartPoint") as GameObject;
        }
        cloudPooling.setState(ObjectPooling.Pooling_State.Cloud); //오브젝트풀링 상태 설정
        cloudPooling.Creation(); //생성
        Cloud_Ready();
    }
    // push & pop
    public void Cloud_Control(GameObject cloud)
    {
        cloud.SetActive(false);
        cloudPooling.Push(cloud);
        Cloud_Ready();
    }
    //pop 및 위치 설정
    private void Cloud_Ready()
    {
        clone = cloudPooling.Pop();
        if (clone != null)
        {
            clone.transform.position = startPoint.transform.position;
            clone.SetActive(true);
        }
    }
}
