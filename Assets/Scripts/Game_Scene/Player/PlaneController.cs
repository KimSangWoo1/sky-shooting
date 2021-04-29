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
    public ResultBoardControl resultBoardControl;
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

            profile.UpdateScore(UI_BM.Get_Score(profile.name));//결과 점수 가져오기

            resultBoardControl.Set_ResultBoard(profile); //결과 내용 전송
            

            UI_BM.Reset_Score(profile.name);//플레이 점수 보드 변경
            gameObject.SetActive(false); //삭제
        }

        Move(); //비행기 이동
        Rot(); //비행기 회전
        fireController.Player_FireTrigger(); //발사
        busterController.Player_Buster_Control(); //부스터 관리
    }

    //비행기 이동 & 부스터
    private void Move()
    {
        #if UNITY_ANDROID
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
        #endif

        #if UNITY_EDITOR_WIN
        //PC 부스터
        if (Input.GetKeyDown(KeyCode.Space))
        {
            busterController.buster = true;
            if (!busterFx.isPlaying)
            {
                busterFx.Play();
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            engineFX.gameObject.SetActive(false);
            if (!busterController.buster)
            {
                busterFx.Pause();
                transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
            }
            else
            {
                busterFx.Play();
                transform.Translate(Vector3.forward * Time.deltaTime * (runSpeed + runPower), Space.Self);
            }

        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            engineFX.gameObject.SetActive(true);
            busterFx.Pause();
            busterController.buster = false;
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
        }
        #endif

    }
    //비행기 회전
    private void Rot()
    {
        #if UNITY_EDITOR_WIN
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
        #endif
        #if UNITY_ANDROID
        //Mobile 용
        if (joystick.move)
        {
            Vector2 joyDirect = joystick.getDirection();
            Vector3 direct = new Vector3(joyDirect.x, 0f, joyDirect.y);
            direct = direct.normalized;

            Quaternion diretion = Quaternion.LookRotation(direct, Vector3.up);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion, Time.deltaTime * turnSpeed);
            /* 곧바로 회전
            float angle = Mathf.Atan2(joyDirect.x, joyDirect.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, angle, 0f) ;
            */
        }
        #endif
    }

    //아이템 메시지 받기
    public void OnReceiver_InteractMessage(MessageType type, object msg)
    {
        Interaction.InteractMessage message = (Interaction.InteractMessage)msg;

        switch (type)
        {
            case MessageType.HEALTH:
                hp += message.amount;
                HPCheck();
                health.ChaneHP(hp);
                break;
            case MessageType.DOLLAR:
                profile.UpdateDollar(message.amount);
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
        //점수 메시지 받기
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
                        UI_BM.Add_Score(message.name, 100); // 죽인 Player에게 100점
                    }
                    else
                    {
                        UI_BM.Add_Score(message.name, 10); // 맞춘 Player에게 10점
                    }

                    hit_Blinking.Blinking(true); //UI 빨간색 깜박임
                    health.ChaneHP(hp); //UI hp 변경
                    break;
                case MessageType.CLASH:
                    hp -= message.damage;
                    HPCheck();//hp 체크

                    hit_Blinking.Blinking(true); //UI 빨간색 깜박임
                    health.ChaneHP(hp); //UI hp 변경
                break;
            }
        }
}
