using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plane_AvoidMove : MathFunction
{
    public GameObject bullet; //총알
    public Transform left_Muzzle; //왼쪽 총구
    public Transform right_Muzzle; //오른쪽 총구
    public bool fireCheck; //총알 발사 객체 체크

    private BulletManager BM;
    private Title_SceneManager TS;

    private float fireTime; // 발사 시간
    private float fireReloadTime; //재장전 시간
    private float fireWaitTime;  //발사 후 대기 시간
    private float fireSpeedTime; //발사 속도 시간

    [SerializeField]
    private float turnSpeed;  //비행기 회전 속도
    [SerializeField]
    private float runSpeed; //비행기 이동 속도
    [SerializeField]
    private float sinSensitivity; //비행기 높이 감도

    private int fireCount; //총 발사 횟수
    private int fireMaxCount; //발사 최대 횟수

    private void Awake()
    {
        //이동
        turnSpeed = 5f;
        runSpeed = 50f;
        sinSensitivity = 1.5f;
        //발사 설정
        fireReloadTime = 2f;
        fireWaitTime = 0.3f;
        fireSpeedTime = 5f;
        fireMaxCount = 3;
    }

    private void OnEnable()
    {

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
        float cos = Cos();
        float sin = Sin_Twice(); //이게 반원 운동에 KeyPoint!!!
        Vector3 move = new Vector3(cos , sin / sinSensitivity,  0f);

        this.transform.Translate(Vector3.forward + move, Space.Self);
        //this.transform.rotation = startRotation * Quaternion.AngleAxis(cos * turnSpeed, Vector3.forward);
        this.transform.Rotate(Vector3.forward, cos *turnSpeed, Space.Self);
        
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
            if (TS.Check_Revenge())
            {
                TS.SetActionState(Title_SceneManager.Action_State.cameraBumpAction);
            }
            else
            {
                TS.SetActionState(Title_SceneManager.Action_State.revengeAction);
            }
        }
    }
}
