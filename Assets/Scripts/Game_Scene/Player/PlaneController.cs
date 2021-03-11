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
    public RightPanel_Control rightPanel;
    public JoyStick joystick;

    [Header("속도")]
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float turnSpeed;

    private float fireTime; // 발사 시간
    private float fireReloadTime; //재장전 시간
    private float fireWaitTime;  //발사 후 대기 시간
    private float fireSpeedTime; //발사 속도 시간

    private int fireCount; //총 발사 횟수
    private int fireMaxCount; //발사 최대 횟수

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
        fireWaitTime = 0.3f;
        fireSpeedTime = 5f;
        fireMaxCount = 3;
    }
    void Start()
    {
        BM = BulletManager.Instance;
    }
      void Update()
    {
        //이동
        Move();
        //회전
        Rot();
        //발사
        Fire();
    }

    void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * runSpeed, Space.Self);
        //부스터
        if (rightPanel.buster || Input.GetKey(KeyCode.Space))
        {
            runSpeed = 10f *2;
        }
        else
        {
            runSpeed = 10f;
        }
    }

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
                if (rightPanel.fire || Input.GetKeyDown(KeyCode.RightControl))
                {
                    fireTime = 0f;
                    fireCount++;
                    BM.bullet_Fire(leftMuzzle);
                    BM.bullet_Fire(rightMuzzle);
                }
            }
        }
    }

    /*
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
                if (rightPanel.fire || Input.GetKeyDown(KeyCode.RightControl))
                {
                    fireTime = 0f;
                    fireCount++;
                    BM.bullet_Fire(leftMuzzle);
                    BM.bullet_Fire(rightMuzzle);
                }
            }
        }
    }
    */
}
