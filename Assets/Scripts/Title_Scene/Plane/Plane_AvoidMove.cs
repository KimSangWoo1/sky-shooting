using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_AvoidMove : MathFunction
{
    public GameObject bullet; //총알
    public Transform left_Muzzle; //왼쪽 총구
    public Transform right_Muzzle; //오른쪽 총구
    public bool fireCheck; //총알 발사 객체 체크

    private BulletManager BM;
    private Title_SceneManager TS;

    private Vector3 startPosition;  //초기 위치
    private Quaternion startRotation; //초기 각도

    private float fireTime; // 발사 시간
    private float fireReloadTime; //재장전 시간
    private float fireWaitTime;  //발사 후 대기 시간
    private float fireSpeedTime; //발사 속도 시간

    [SerializeField]
    private float turnSpeed;  //비행기 회전 속도
    [SerializeField]
    private float runSpeed; //비행기 이동 속도
   
    private int fireCount; //총 발사 횟수
    private int fireMaxCount; //발사 최대 횟수

    
    private void Awake()
    {
        //이동
        turnSpeed = 5f;
        runSpeed = 30f;
        //발사 설정
        fireReloadTime = 2f;
        fireWaitTime = 0.3f;
        fireSpeedTime = 5f;
        fireMaxCount = 3;
    }

    private void OnEnable()
    {
        //초기 값
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    void Start()
    {
        BM = BulletManager.Instance;
        TS = Title_SceneManager.Instance;
    }

    void Update()
    {
        if (fireCheck)
        {
            Fire();
        }
        Half_exercise();     
    }

    //반원 운동
    private void Half_exercise()
    {
        float cos = Cos_Control(-1,1);
         this.transform.Translate((-Vector3.up+ new Vector3(cos, -Sin_Control(), 0f).normalized)* Time.deltaTime * runSpeed ,Space.Self);
        this.transform.rotation = Quaternion.Euler(0f, 0f, cos * turnSpeed) * startRotation;
    }
    //발사
    private void Fire()
    {   
        fireTime += Time.deltaTime * fireSpeedTime;
        if (fireCount >= fireMaxCount)
        {
            if (fireTime >= fireReloadTime)
            {
                fireCount = 0;
                fireTime = 0;
            }
        }
        else
        { 
            if (fireTime >= fireWaitTime)
            {
                fireTime = 0f;
                fireCount++;
                BM.bullet_Fire(left_Muzzle);
                BM.bullet_Fire(right_Muzzle);             
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            TS.SetActionState(Title_SceneManager.Action_State.revengeAction);
        }
    }
}
