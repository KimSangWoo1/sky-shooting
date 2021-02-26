using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Move : MonoBehaviour
{
    private CloudManager CM;
    public GameObject target; //목표 지점
    public float cloudSpeed = 10.0f; //구름 속도

    private void OnEnable()
    {
        if (target == null)
        {
            target = GameObject.Find("EndPoint"); //Hieracrchy에서 이름:EndPoint 오브젝트 찾기
        }
    }
    private void Start()
    {
        //싱글톤 적용
        CM = CloudManager.Instance;
    }
    void Update()
    {
        Move();
        Check_Distance();
        Check_Behind();

    }

    //오브젝트 이동
    void Move()
    { 
        transform.Translate(transform.forward * cloudSpeed * Time.deltaTime, Space.Self);
    }

    //오브젝트 간에 거리 구하기
    void Check_Distance()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        //50f 미만 일경우 풀에 Push 하기
        if (distance < 50f)
        {
            CM.Cloud_Control(this.gameObject);
            print("1번");
        }
    }
    //오류 방지를 위한 내적을 사용하여 Object가 타겟 뒤로 가면 Push 시키기
    void Check_Behind()
    {   //타겟.up 방향에 구름이 있으면 dot>0  뒤에 있으면  dot<0
        float dot = Vector3.Dot(target.transform.up, (transform.position - target.transform.position).normalized); //내적 Vector3.Dot(타겟Object,기준Object)
        if (dot < 0)
        {
            CM.Cloud_Control(this.gameObject);
            print("2번");
        }
    }
}
