using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp_Move : MonoBehaviour
{

    public Transform Target;

    public float speed;
    public int num;
    public bool self;
    public bool unity;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //위치 초기화
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = startPosition;
        }
       
        if (self)
        {
            this.transform.position += self_Lerp() * Time.deltaTime * speed;
        }
        if (unity)
        {
            unity_Lerp();
        }
    }

    private Vector3 self_Lerp()
    {
        Vector3 distance = Target.position - transform.position; //방향 및 거리
        return distance / num; //선형보간
    }

    void unity_Lerp()
    {
        this.transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * speed);
    }
}
