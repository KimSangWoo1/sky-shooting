using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("총구")]
    public MuzzleController muzzle; //총구

    [Header("UI")]
    public RightPanel_Control rightPanel; //부스터 ,발사
    public Magazine magazine; //탄창
    public JoyStick joystick; //조이스틱
    public Health hp;

    private float runSpeed; //이동속도
    [SerializeField]
    private float turnSpeed; //회전속도

    private float runPower; //추가 이동속도

    private float fireTime; // 발사 시간
    private float fireReloadTime; //재장전 시간
    private float fireWaitTime;  //발사 후 대기 시간
    private float fireSpeedTime; //발사 속도 시간

    private int fireCount; //총 발사 횟수

    private bool trigger; //총 발사

    //입력값
    private float h;
    private float v;

    private void Awake()
    {
        //이동설정
        runSpeed = 10f;
        runPower = 10f;
        turnSpeed = 2f;
        
        //발사 설정
        fireReloadTime = 2f;
        fireWaitTime = 0.2f;
        fireSpeedTime = 5f;
    }
    void Start()
    {
     
    }
      void Update()
    {
        runPower = Mathf.Clamp(runPower, 10, 30);
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

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Item_Bullet")
        {
            muzzle.Add_Bullet();
        }
        else if(other.gameObject.tag == "Item_Muzzle")
        {
            muzzle.Add_Muzzle();
        }else if(other.gameObject.tag == "Item_Turbin")
        {
            runPower += 5f;
            fireWaitTime += 0.1f;
        }else if(other.gameObject.tag == "Item_Health")
        {
            hp.Add_Health(other.gameObject);
        }else if(other.gameObject.tag == "Item_Dollar")
        {

        }

        other.transform.parent.gameObject.SetActive(false);

    }
    //비행기 이동 & 부스터
    private void Move()
    {
        //이동
        transform.Translate(Vector3.down * Time.deltaTime * runSpeed, Space.Self);
        //Mobile 부스터
        if (rightPanel.buster)
        {
            runSpeed = runPower *2;
        }
        else
        {
            runSpeed = runPower;
        }

        //PC 부스터
        if (Input.GetKey(KeyCode.Space))
        {
            rightPanel.buster = true;
        }else if(Input.GetKeyUp(KeyCode.Space))
        {
            rightPanel.buster = false;
        }
    }
    //비행기 회전
    private void Rot()
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
        if (fireCount >= muzzle.Get_BulletCount())
        {
                trigger = false;
                fireCount = 0;
                fireTime = 0f;
        }
        else
        {
            if (fireTime >= fireWaitTime)
            {
                muzzle.Fire(); //총구에서 발사
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
