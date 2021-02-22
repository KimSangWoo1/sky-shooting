using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject target; //목표 지점
    public float cloudSpeed = 10.0f; //구름 속도

    private void OnEnable()
    {
        if (target == null)
        {
            target = GameObject.Find("EndPoint"); //Hieracrchy에서 이름:EndPoint 오브젝트 찾기
        }
    }

    void Update()
    {
        //오브젝트 간에 거리 구하기
        float distance = Vector3.Distance(target.transform.position, transform.position);
        //50f 미만 일경우 풀에 Push 하기
        if (distance < 50f)
        {
            this.gameObject.SetActive(false);
            ObjectPooling.cloudPooling.Push(this.gameObject);
            
        }
        //오브젝트 이동
        transform.Translate(transform.forward * cloudSpeed * Time.deltaTime, Space.Self); 
    }
}
