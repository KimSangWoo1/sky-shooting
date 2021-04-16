
using UnityEngine;
using Message;
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
        //부스터 관리
        busterController.Player_Buster_Control();
    }

    //비행기 이동 & 부스터
    private void Move()
    {   
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

        /*
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
         */
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
                HpControl();
                health.ChaneHP(hp);
                break;
            case MessageType.DAMAGE:
                hp -= message.amount;
                hitFx.Play();
                HpControl();
                hit_Blinking.Blinking(true);
                health.ChaneHP(hp);
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
            case MessageType.CLASH:
                ParticleSystem dead = Instantiate(deadFx, transform.position, Quaternion.Euler(-90f, 0f, 0f));
                dead.gameObject.SetActive(true);

                this.gameObject.SetActive(false);
                break;
        }
    }
}
