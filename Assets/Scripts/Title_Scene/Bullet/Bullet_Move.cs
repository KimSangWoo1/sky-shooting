using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Move : MonoBehaviour
{
    private BulletManager BM;
    [SerializeField]
    private float bulletSpeed; //총알 속도
    private float timeSpeed; //생명 시간 속도
    private float lifeTime; //생명 시간
    private float deadTime; //죽는 시간

    private void Awake()
    {
        bulletSpeed = 80f;
        timeSpeed = 2f;
        deadTime = 4f;
    }
    void Start()
    {
        //싱글톤 생성
        BM = BulletManager.Instance;   
    }

    void Update()
    {
        //총 생명 시간 2초  -> 2+2 =4 
        lifeTime += Time.deltaTime *timeSpeed;
        if (lifeTime > deadTime)
        {
            lifeTime = 0f;  //초기화
            BM.bullet_Control(this.gameObject); //Push 및 active 설정
        }
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed, Space.Self);
    }
}
