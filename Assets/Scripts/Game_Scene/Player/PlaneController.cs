using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    BulletManager BM;
    [Header("총구 위치")]
    public Transform leftMuzzle;
    public Transform rightMuzzle;
    public Transform centerMuzzle;

    [Header("UI")]
    public RightPanel_Control rightPanel; //부스터 ,발사
    public Magazine magazine; //탄창
    public JoyStick joystick; //조이스틱

    private float runSpeed; //이동속도
    private float turnSpeed; //회전속도

    private float fireTime; // 발사 시간
    private float fireReloadTime; //재장전 시간
    private float fireWaitTime;  //발사 후 대기 시간
    private float fireSpeedTime; //발사 속도 시간

    private int fireCount; //총 발사 횟수
    private int fireMaxCount; //발사 최대 횟수

    private bool trigger; //총 발사

    //입력값
    private float h;
    private float v;

    private void Awake()
    {
        //이동설정
        runSpeed = 10f;
        turnSpeed = 15f;

        //발사 설정
        fireReloadTime = 2f;
        fireWaitTime = 0.2f;
        fireSpeedTime = 5f;
        fireMaxCount = 3;
    }
    void Start()
    {
        BM = BulletManager.Instance;
    }
      void Update()
    {
        //비행기 이동
        Move();
        //비행기 회전
        Rot();
        //발사 Trigger 
        Fire_Trigger();
        //발사!!
        if (trigger)
        {
            Shooting();
        }
    }
    //비행기 이동
    void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * runSpeed, Space.Self);
        //Mobile
        if (rightPanel.buster)
        {
            runSpeed = 10f *2;
        }
        else
        {
            runSpeed = 10f;
        }
        //PC
        if (Input.GetKey(KeyCode.Space))
        {
            rightPanel.buster = true;
        }

    }
    //비행기 회전
    void Rot()
    {
        //PC용
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //입력
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            //회전
            Vector3 diret = new Vector3(h, 0f, v);
            if (diret != Vector3.zero)
            {
                diret = diret.normalized;

                Quaternion diretion = Quaternion.LookRotation(diret, Vector3.up);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion * Quaternion.AngleAxis(-90f, Vector3.right), Time.deltaTime * turnSpeed);
            }
        }
        //Mobile 용
        if (joystick.move)
        {
            Vector2 joyDirect = joystick.getDirection();
            joyDirect = joyDirect.normalized;
            float angle = Mathf.Atan2(joyDirect.x, joyDirect.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(-90f, angle, 0f) ;
        }
    }
    // 발사!!
    private void Shooting()
    {
        if (fireCount >= fireMaxCount)
        {            
                trigger = false;
                fireCount = 0;
                fireTime = 0f;
        }
        else
        {
            if (fireTime >= fireWaitTime)
            {
                BM.bullet_Fire(leftMuzzle);  // 총알 발사
                BM.bullet_Fire(rightMuzzle); //총알 발사
                fireCount++;
                fireTime = 0f;
            }
        }         
    }
    //발사 Trigger
    private void Fire_Trigger()
    {
        fireTime += Time.deltaTime * fireSpeedTime;

        if (fireTime >= fireReloadTime)
        {   // Mobile || PC
            if (rightPanel.fire && magazine.get_Fireable() || Input.GetKeyDown(KeyCode.RightShift) && magazine.get_Fireable())
            { 
                trigger = true;
                magazine.Shot(); //탄창 탄알 사용
            }
        }
    }
}
