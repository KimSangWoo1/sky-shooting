﻿
using UnityEngine;
using Message;
public class PlaneController : PlaneBase ,IMessageReceiver
{
    [Header("발사")]
    public FireController fireController;
    [Header("총구 설정")]
    public MuzzleController muzzle; //총구

    [Header("UI")]
    public RightPanel_Control rightPanel; //부스터 ,발사
    public JoyStick joystick; //조이스틱
    public Health health;

    //입력값
    private float h;
    private float v;
    void Update()
    {
        runPower = Mathf.Clamp(runPower, 10, 30);
        hp = Mathf.Clamp(hp, 0, 100);

        //비행기 이동
        Move();
        //비행기 회전
        Rot();
        //발사
        fireController.Player_FireTrigger();
    }

    //비행기 이동 & 부스터
    private void Move()
    {
        //Mobile 부스터
        if (rightPanel.buster)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (runSpeed + runPower), Space.Self);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
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
                transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion , Time.deltaTime * turnSpeed);
            }
        }
        //Mobile 용
        if (joystick.move)
        {
            Vector2 joyDirect = joystick.getDirection();
            joyDirect = joyDirect.normalized;
            float angle = Mathf.Atan2(joyDirect.x, joyDirect.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, angle, 0f) ;
        }
    }
 
    //메시지 받기
    public void OnReceiverMessage(MessageType type, object msg)
    {
        Interaction.InteractMessage message = (Interaction.InteractMessage)msg;

        switch (type)
        {
            case MessageType.HEALTH:
                hp += message.amount;
                health.ChaneHP(hp);
                break;
            case MessageType.DAMAGE:
                hp -= message.amount;
                health.ChaneHP(hp);
                break;
            case MessageType.BULLET:
                if (message.upgrade)
                {
                    muzzle.Add_Bullet();
                }
                break;
            case MessageType.MUZZLE:
                if (message.upgrade)
                {
                    muzzle.Add_Muzzle();
                }
                break;
            case MessageType.TURBIN:
                if (message.upgrade)
                {
                    runSpeed += message.amount;
                    //fireWaitTime += 0.1f; 
                }
                break;
        }
    }
}