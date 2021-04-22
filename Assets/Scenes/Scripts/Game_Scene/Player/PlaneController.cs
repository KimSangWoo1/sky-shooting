using UnityEngine;
using UnityEngine.UI;
using Message;
[DefaultExecutionOrder(100)]
public class PlaneController : PlaneBase ,IMessageReceiver
{
    [Header("발사")]
    public FireController fireController; //발사 시스템
    [Header("총구 설정")]
    public MuzzleController muzzleController; //총구
    public BusterController busterController; //부스터

    [Header("UI")]
    public JoyStick joystick; //조이스틱
    public Health health;
    public Hit_Blinking hit_Blinking;
    public Image resultUI;
    //입력값
    private float h;
    private float v;
    void Update()
    {
        runPower = Mathf.Clamp(runPower, 10, 30);
        hp = Mathf.Clamp(hp, 0, 100);
        if (hp <= 0f)
        {
            base.FXM.FX_Pop(transform, deadState); // 파괴 연출
            base.Item_Random(); //아이템 생성

            BM.Reset_Score(profile.name);//점수 보드 변경
            gameObject.SetActive(false); //삭제
            resultUI.gameObject.SetActive(true);
            //결과 UI 보여주기
        }

        Move(); //비행기 이동
        Rot(); //비행기 회전
        fireController.Player_FireTrigger(); //발사
        busterController.Player_Buster_Control(); //부스터 관리
    }

    //비행기 이동 & 부스터
    private void Move()
    {   
        /*
        //Mobile 부스터
        if (busterController.Get_BusterClick())
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (runSpeed + runPower), Space.Self);
            engineFX.gameObject.SetActive(false);
            if (!busterFx.isPlaying)
            {
                busterFx.Play();
            }
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
            engineFX.gameObject.SetActive(true);
            busterFx.Pause();
        }
        */
        
         //PC 부스터
         if (Input.GetKeyDown(KeyCode.Space)){
             busterController.buster = true;
             if (!busterFx.isPlaying)
             {
                busterFx.Play();
             }
         }
         else if (Input.GetKey(KeyCode.Space))
         {
             engineFX.gameObject.SetActive(false);
             if(!busterController.buster)
             { 
                 busterFx.Pause();
                 transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
             }
             else
             {
                 transform.Translate(Vector3.forward * Time.deltaTime * (runSpeed + runPower), Space.Self);               
             }

         }
         else if(Input.GetKeyUp(KeyCode.Space))
         {
             engineFX.gameObject.SetActive(true);
             busterFx.Pause();
             busterController.buster = false;
         }
         else
         {
             transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
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

    //메시지 받기1
    public void OnReceiver_InteractMessage(MessageType type, object msg)
    {
        Interaction.InteractMessage message = (Interaction.InteractMessage)msg;

        switch (type)
        {
            case MessageType.HEALTH:
                hp += message.amount;
                print(hp);
                HPCheck();
                health.ChaneHP(hp);
                break;
            case MessageType.DOLLAR:
                //아직
                break;
            case MessageType.BULLET:
                if (message.upgrade)
                {
                    muzzleController.Add_Bullet();
                }
                break;
            case MessageType.MUZZLE:
                if (message.upgrade)
                {
                    muzzleController.Add_Muzzle();
                }
                break;
            case MessageType.TURBIN:
                if (message.upgrade)
                {
                    runSpeed += message.amount;
                }
                break;
        }
    }
        //메시지 받기2
        public void OnReceiver_DamageMessage(MessageType type, object msg)
        {
            Interaction.DamageMessage message = (Interaction.DamageMessage)msg;
            switch (type)
            {
                case MessageType.DAMAGE:
                    hp -= message.damage;  
                    HPCheck(); //hp 체크
                    base.hitFx.Play(); //타격 FX 

                   //점수 보드 변경
                    if (hp <= 0f)
                    {
                        BM.Add_Score(message.name, 100); // 죽인 Player에게 100점
                    }
                    else
                    {
                        BM.Add_Score(message.name, 10); // 맞춘 Player에게 10점
                    }

                    hit_Blinking.Blinking(true); //UI 빨간색 깜박임
                    health.ChaneHP(hp); //UI hp 변경
                    break;
                case MessageType.CLASH:
                    hp -= message.damage;
                    HPCheck();//hp 체크

                    BM.Reset_Score(profile.name);//점수 보드 변경

                    hit_Blinking.Blinking(true); //UI 빨간색 깜박임
                    health.ChaneHP(hp); //UI hp 변경
                break;
            }
        }
}
