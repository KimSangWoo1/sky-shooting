using UnityEngine;
using Message;

[DefaultExecutionOrder(100)]
public class B_Machine : PlaneBase, IMessageReceiver
{
    enum State { Wait, Found, Attack, Avoid, Dead }; //비행기 상태 : (대기, 찾음, 공격, 도망, 죽음)
    enum FoundState { Enemy, Item}; // 비행기 센서 상태 : (타겟 발견, 아이템 발견)
    enum WaitState { GetPosition, GoPosition }; //비행기 대기 상태 : (랜덤좌표 얻기, 랜덤좌표 이동)
    enum AvoidState { GetDirection, RunAway, Return, Emergency }//비행기 도망 상태 : (방향 얻기 ,도망가기, 다시 싸우러가기, 위급 상황)
    State state;
    FoundState foundState;
    WaitState waitState;
    AvoidState avoidState;

    public TargetScanner scanner; //스캐너
    public Map map; // 맵
    public MuzzleController muzzle; //총구
    public FireController fireController; //발사
    public BusterController busterController; //부스터

    private Transform target; //적군
    private Transform item; //아이템

    private Vector3 randomPosition; // Map  랜덤 좌표
    private Vector3 fightPosition; //싸웠던 위치
    private Vector3 avoidPosition; //도망칠 방향
    private float closeDistance; // Map 랜덤 좌표 접근 인정 거리
    private float avoidTime; //도망치는 시간

    // AI 형에 따른 설정 가능
    [SerializeField]
    private float avoidWaitTime; //도망가는 시간 설정 기본 2초  //시간이 : 짧을수록 공격형 - 밸런스형 - 방어형  시간이 길수록
    [SerializeField]
    private float busterWaitTime; //부스터
    [SerializeField]
    private float sensingSensitivity; //근접 거리 감지 감도  //시간이 : 짧을수록 공격형 - 밸런스형 - 방어형  시간이 길수록   최소 : 5f ~ 최대 : 7f

    private Vector3 direct; // 방향 계산용
    private Quaternion direction; //각도 계산용
    private int emergencyMode; //1 : 비행기 충돌 피하기 2 : 피격 피하기 후 공격하러 가기

    private void OnEnable()
    {
        base.OnEnable();
        //상태 초기화
        state = State.Wait;
        waitState = WaitState.GetPosition;
        avoidState = AvoidState.GetDirection;
        foundState = FoundState.Enemy;

        randomPosition = Vector3.zero;
        fightPosition = Vector3.zero;
        avoidPosition = Vector3.zero;
    }

    void Start()
    {
        base.Start();
        closeDistance = 10f;
        avoidWaitTime = 2f;

        sensingSensitivity = 7f;
    }
    void Update()
    {
        if (hp <= 0)
        {
            //죽음
            state = State.Dead;
            //초기화
            waitState = WaitState.GetPosition;
            foundState = FoundState.Enemy;
            avoidState = AvoidState.GetDirection;
        }

        Move(); //이동
        Machine_State(); //상태 관리
        busterController.AI_Buster_Control(); //부스터
    }
    #region 이동 & 부스터
    //이동
    private void Move()
    {
        if (!busterController.buster)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (runSpeed + runPower), Space.Self);
        }
        //부스터 적용은 Wait과 Avoid 때만
        if (state == State.Wait)
        {
            BusterControl(0.6f);
        }
        else if(state == State.Avoid)
        {
            BusterControl(0.3f);
        }        
    }
    private void BusterControl(float amount)
    {
        if (busterController.Get_BusterGage() >= amount || busterController.buster)
        {
            busterController.buster = true;
            engineFX.gameObject.SetActive(false);
            if (!busterFx.isPlaying)
            {
                busterFx.Play();
            }        
        }
        else
        {
            busterController.buster = false;
            engineFX.gameObject.SetActive(true);
            busterFx.Pause();
        }
    }
    #endregion
    #region AI 상태 설정
    private void Machine_State()
    {
        //자폭 위험 방지
        EmerGencyPrevention();
       // print(state+" "+ fireController.Get_BulletCount());
        switch (state)
        {
            case State.Wait:
                target = scanner.Detect(transform, TargetScanner.DetectState.Enemy);
                item = scanner.Detect(transform, TargetScanner.DetectState.Item);
                if (target != null || item !=null)
                {
                    state = State.Found; //발견
                }
                else
                {
                    WaitAction(); //미발견
                }
                break;
            case State.Found:
                FoundAction();
                CheckObstacle(); //장애물이 앞에 있을 경우
                break;
            case State.Attack:
                AttackAction();
                break;
            case State.Avoid:
                AvoidAction();
                break;
            case State.Dead:
                base.FXM.FX_Pop(transform, deadState);  // 파괴 연출
                base.Item_Random(); //아이템 생성

                UI_BM.Reset_Score(profile.name);//점수 보드 변경
                gameObject.SetActive(false);//삭제
                break;
        }
    }
    // 비행기 출돌 피하기
    private void EmerGencyPrevention()
    {
        if (state==State.Found || state == State.Attack || (state ==State.Avoid && avoidState != AvoidState.Emergency ) || (avoidState == AvoidState.Emergency && avoidTime>=1f))
        {
            if(target != null)
            {           
                direct = target.position - transform.position;
                float targetDistance = direct.sqrMagnitude;
                float safeDistance = closeDistance * closeDistance * sensingSensitivity;

                if (scanner.AttackDetect(transform, target))
                {
                    //피하기 전 총알 쏘기
                    if (fireController.IsRemainMagazine())
                    {
                        fireController.AI_FireTrigger();
                    }
                }
                //거리 점검
                if (targetDistance <= safeDistance )
                {

                    fightPosition = this.transform.position;
                    emergencyMode = 1; // 1. 비행기 출돌 피하기 모드
                    avoidTime = 0f;
                    avoidPosition = map.ClashAvoid_RandomPosition(transform);
                    state = State.Avoid;
                    avoidState = AvoidState.Emergency;                 
                }
            }
        }
    }

    #endregion
    #region Wait State (돌아다니기)
    private void WaitAction()
    {
        switch (waitState)
        {
            case WaitState.GetPosition:
                randomPosition = map.Random_Position(); //랜덤 좌표 받기
                waitState = WaitState.GoPosition; //상태 변경
                break;
            case WaitState.GoPosition:
                if (randomPosition != Vector3.zero || randomPosition != null)
                {
                    Check_DistancePosition(randomPosition); //거리 체크하기
                }
                else
                {
                    waitState = WaitState.GetPosition; //상 태변경
                }
                break;
        }
    }
    //새로운 랜덤 좌표 받아오기
    private void Check_DistancePosition(Vector3 _randomPosition)
    {
        float distance = Vector3.Distance(transform.position, _randomPosition);
        if (distance < closeDistance)
        {
            waitState = WaitState.GetPosition; //어느정도 거리 접근하면 상태변경
        }
        else
        {
            Random_Rot(_randomPosition); // 좌표로 방향 회전
        }
    }
    // 새로운 랜덤 좌표 방향으로 돌기
    private void Random_Rot(Vector3 point)
    {
        //회전
        if (point != null && point != Vector3.zero)
        {
            direct = point - transform.position;
            if (direct != Vector3.zero)
            {
                direct = direct.normalized;
                direction = Quaternion.LookRotation(direct, Vector3.up);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, Time.deltaTime * turnSpeed);
            }
        }
    }
    #endregion

    #region Found State (적방향 회전 & 적 쫒기)
    private void FoundAction()
    {
        target = scanner.Detect(transform, TargetScanner.DetectState.Enemy);
        item = scanner.Detect(transform, TargetScanner.DetectState.Enemy);

        switch (foundState)
        {
            case FoundState.Enemy:
                Rot(target); //타겟 방향 회전
                // 쏘면 맞는지 확인
                if (target != null)
                {
                    if (scanner.AttackDetect(transform, target))
                    {
                        state = State.Attack;
                    }
                }
                else
                {
                    if (item != null)
                    {
                        foundState = FoundState.Item;
                    }
                }
                break;
            case FoundState.Item:
                Rot(item);
                if (target != null)
                {
                    foundState = FoundState.Enemy;
                }
                    break;
            default:
                break;
        }
    }

    //레이더 안에 들어온 타겟 방향 설정
    private void Rot(Transform _target)
    {
        //회전
        if (_target != null)
        {
            direct = _target.position - transform.position;
            if (direct != Vector3.zero)
            {
                direct = direct.normalized;

                direction = Quaternion.LookRotation(direct, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, direction, Time.deltaTime * turnSpeed); //*2f
            }
        }
        else
        {  //놓칠 경우 상태 변경
            state = State.Wait;
        }
    }
    //앞에 장애물 있을 경우 놓친걸로 판단
    private void CheckObstacle()
    {
        if (Physics.Raycast(transform.position, transform.forward, scanner.radius, scanner.blockLayerMask, QueryTriggerInteraction.Collide))
        {
            state = State.Wait;
        }
    }
    #endregion

    #region Attack State (적 쏘기)
    private void AttackAction()
    {
        //쏠 수 있는 탄알이 있는지
        if (fireController.IsRemainMagazine())
        {
            fireController.AI_FireTrigger();
        }
        //없으면 도망
        else
        {
            fightPosition = transform.position; //싸웠던 위치 기억
            state = State.Avoid;
        }
    }
    #endregion

    #region Avoid State (도망가기)

    private void AvoidAction()
    {
        target = scanner.Detect(transform, TargetScanner.DetectState.Enemy);

        switch (avoidState)
        {
            //0. 도망칠 방향 정하기
            case AvoidState.GetDirection:
                avoidPosition = map.Spot_RendomPosition(transform);
                avoidState = AvoidState.RunAway;
                break;
            //1. 도망 
            case AvoidState.RunAway:
                RunAway(avoidPosition);
                break;
            //2. 도망 갔다가 싸운 지점 복귀
            case AvoidState.Return:
                //복귀 시 타겟 발견하면 쫒아감
                if (target != null )
                {
                    state = State.Found;
                }
                // 싸웠던 장소 복귀
                else
                {
                    Return_FightPosition(fightPosition);
                }
                break;
            //3. 위급 상황
            case AvoidState.Emergency:
                EmergencyAway(avoidPosition, emergencyMode);
                break;
        }
    }
    //1. 도망 (설정 : 도망 칠 거리, 도망칠 방향 범위, 장애물 검사, Map 경계 검사)
    private void RunAway(Vector3 _avoidPosition)
    {
        //가까우면 복귀
        if (Vector3.Distance(transform.position, _avoidPosition) < closeDistance)
        {
            avoidTime = 0f;
            avoidState = AvoidState.Return;
        }
        else
        {
            //정해진 시간동안 도망감
            if (avoidTime <= avoidWaitTime)
            {
                avoidTime += Time.deltaTime;
                direct = _avoidPosition - transform.position;
                direct = direct.normalized;
                //회전
                direction = Quaternion.LookRotation(direct, Vector3.up);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, Time.deltaTime * turnSpeed);
            }
            else
            {
                // 탄창이 없으면 도망가도록
                if (fireController.Get_BulletCount() == 0)
                {
                    avoidTime = 0f;
                }
                else
                {
                    avoidTime = 0f;
                    avoidState = AvoidState.Return;
                }
            }
        }
    }

    //2. 도망 갔다가 싸운 지점 복귀
    private void Return_FightPosition(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        //3. 복귀시 타겟 없으면 Wait 상태로
        if (distance < closeDistance)
        {
            state = State.Wait;
            avoidState = AvoidState.GetDirection;// 초기화
        }
        // 싸운 지점 복귀중
        else
        {
            direct = position - transform.position;
            direct = direct.normalized;
            //회전
            direction = Quaternion.LookRotation(direct, Vector3.up);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, Time.deltaTime * turnSpeed);
        }
    }

    private void EmergencyAway(Vector3 _avoidPosition , int mode)
    {
        Debug.DrawRay(_avoidPosition, Vector3.up, Color.black, 5f);
        //가까우면 복귀
        if (Vector3.Distance(transform.position, _avoidPosition) < closeDistance)
        {
            avoidTime = 0f;
            if(mode == 1)
            {
                state = State.Wait;
            }
            else
            {
                avoidState = AvoidState.Return;
            }
            state = State.Wait;
        }
        else
        {
            //정해진 시간동안 도망감
            if (avoidTime <= avoidWaitTime)
            {
                avoidTime += Time.deltaTime;
                direct = _avoidPosition - transform.position;
                direct = direct.normalized;
                //회전
                direction = Quaternion.LookRotation(direct, Vector3.up);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, Time.deltaTime * turnSpeed);
            }
            else
            {
                // 탄창이 없으면 도망가도록
                if (fireController.Get_BulletCount() == 0)
                {
                    avoidTime = 0f;
                }
                else
                {
                    avoidTime = 0f;
                    state = State.Wait;
                }
            }
        }
    }
    #endregion
    public void OnReceiver_InteractMessage(MessageType type, object msg)
    {
        Interaction.InteractMessage message = (Interaction.InteractMessage)msg;

        switch (type)
        {
            case MessageType.HEALTH:
                hp += message.amount;
                HPCheck();
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
                runSpeed += message.amount;
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
                //HP 변경
                hp -= message.damage;
                HPCheck(); //HP Check
                base.hitFx.Play(); // Hp FX

                //점수 보드 변경
                if (hp <= 0f)
                {
                    UI_BM.Add_Score(message.name, 100); // 죽인 Player에게 100점
                }
                else
                {
                    UI_BM.Add_Score(message.name, 10); // 맞춘 Player에게 10점
                }

                //상태변경
                emergencyMode = 2; // 피격 피하기 후 공격하러 가기
                avoidState = AvoidState.Emergency; 
                state = State.Avoid; 
                break;
            case MessageType.CLASH:
                hp -= message.damage;
                HPCheck();

                UI_BM.Reset_Score(profile.name);//점수 보드 변경

                state = State.Dead;
                break;
        }
    }

    //레이더 Editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        scanner.Editor_TargetScanner(this.transform);
        scanner.Editor_AttackScanner(this.transform);
    }
#endif
}
