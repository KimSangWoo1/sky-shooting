using UnityEngine;
using Message;

[DefaultExecutionOrder(100)]
public class B_Machine : PlaneBase, IMessageReceiver
{
    enum State{Wait, Found, Attack, Avoid, Item, Dead}; //비행기 상태 : (대기, 찾음, 공격, 도망, 아이템, 죽음)
    enum WaitState { GetPosition, GoPosition}; //비행기 대기 상태 : (랜덤좌표 얻기, 랜덤좌표 이동)
    enum AvoidState {GetDirection, RunAway, Return, Blocked}//비행기 도망 상태 : (방향 얻기 ,도망가기, 다시 싸우러가기, 도망가다 막다른 곳 일때)
    State state;
    WaitState waitState;
    AvoidState avoidState;

    public TargetScanner scanner; //스캐너
    public Map map; // 맵
    public MuzzleController muzzle; //총구
    public FireController fireController; //발사

    private Vector3 random_Position; // Map  랜덤 좌표
    private Vector3 fightPosition; //싸웠던 위치
    private Vector3 avoidDirect; //도망칠 방향
    private float closeDistance; // Map 랜덤 좌표 접근 인정 거리
    private float avoidTime; //도망치는 시간

    private int randomNumber; //랜덤 번호
    // AI 형에 따른 설정 가능
    [SerializeField]
    private float avoidWaitTime; //도망가는 시간 설정 기본 2초  //시간이 : 짧을수록 공격형 - 밸런스형 - 방어형  시간이 길수록
    [SerializeField]
    private float busterWaitTime; //부스터
    // 스피드 
 
    private void OnEnable()
    {
        base.OnEnable();
        //상태 초기화
        state = State.Wait;
        waitState = WaitState.GetPosition;
        avoidState = AvoidState.GetDirection;
        fightPosition = Vector3.zero;
    }

    void Start()
    {
        closeDistance = 10f;
        avoidWaitTime = 2f;
    }
    void Update()
    {
        print(state+" "+ fireController.Get_BulletCount());
        switch (state)
        {
            case State.Wait:
                Transform target = scanner.Detect(transform);
                if (target != null)
                {
                    state = State.Found; //발견
                }
                else
                {
                    WaitAction(); //미발견
                }
                break;
            case State.Found:
                Rot(); //타겟 방향 회전
                // 쏘면 맞는지 확인
                if (scanner.AttackDetect(transform))
                {
                    state = State.Attack;
                }
      
                
                break;
            case State.Attack:
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
                break;
            case State.Avoid:
                AvoidAction();
                break;
            case State.Item:
                break;
            case State.Dead:
                // 파괴 연출
                
                //삭제
                this.gameObject.SetActive(false);
                break;
        }
        Move(); //이동
        /*
        if(fireController.Get_BulletCount() < 1)
        {
            print("탄창 없음 RUN");
            state = State.Avoid;
            avoidState = AvoidState.GetDirection;
        }
        */
    }
    //이동
    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
    }

    #region Wait State (돌아다니기)
    private void WaitAction()
    {
        switch (waitState)
        {
            case WaitState.GetPosition:
                random_Position = map.Random_Position(); //랜덤 좌표 받기
                waitState = WaitState.GoPosition; //상태 변경
                break;
            case WaitState.GoPosition:
                if (random_Position != Vector3.zero || random_Position != null)
                {
                    Check_DistancePosition(random_Position); //거리 체크하기
                }
                else
                {
                    waitState = WaitState.GetPosition; //상 태변경
                }
                break;
        }
    }
    //새로운 랜덤 좌표 받아오기
    private void Check_DistancePosition(Vector3 random_Position)
    {
        float distance = Vector3.Distance(transform.position, random_Position);
        if (distance < closeDistance)
        {
            waitState = WaitState.GetPosition; //어느정도 거리 접근하면 상태변경
        }
        else
        {
            Random_Rot(random_Position); // 좌표로 방향 회전
        }
    }
    // 새로운 랜덤 좌표 방향으로 돌기
    private void Random_Rot(Vector3 point)
    {
        //회전
        if (point != null && point != Vector3.zero)
        {
            Vector3 diret = point - transform.position;
            if (diret != Vector3.zero)
            {
                diret = diret.normalized;
                Quaternion diretion = Quaternion.LookRotation(diret, Vector3.up);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion, Time.deltaTime * turnSpeed);
            }
        }
    }
    #endregion

    #region Found State (적방향 회전 & 적 쫒기)
    //레이더 안에 들어온 타겟 방향 설정
    private void Rot()
    {
        Transform target = scanner.Detect(transform);
        //회전
        if (target != null)
        {
            Vector3 diret = target.position - transform.position;
            if (diret != Vector3.zero)
            {
                diret = diret.normalized;

                Quaternion diretion = Quaternion.LookRotation(diret, Vector3.up);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion, Time.deltaTime * turnSpeed);
            }
        }
        else
        {  //놓칠 경우 상태 변경
            state = State.Wait;
        }
    }
    #endregion

    #region Attack State (적 쏘기)

    #endregion

    #region Avoid State (도망가기)

    private void AvoidAction()
    {
        switch (avoidState)
        {
            //0. 도망칠 방향 정하기
            case AvoidState.GetDirection:
                randomNumber = Random.Range(1, 3);
                RandomDirection(randomNumber); 
                avoidState = AvoidState.RunAway;
                break;
            //1. 도망 
            case AvoidState.RunAway:
                RandomAway();
                break;
            //2. 도망 갔다가 싸운 지점 복귀
            case AvoidState.Return:
                //복귀 시 타겟 발견하면 쫒아감
                Transform target = scanner.Detect(transform);
                //발견해도 && 탄창이 한개라도 있어야 함
                if (target != null && fireController.Get_BulletCount() < 1)
                {
                    state = State.Found; 
                }
                // 싸웠던 장소 복귀
                else
                {
                    Return_FightPosition(fightPosition);
                }
                break;
            case AvoidState.Blocked:
                break;
        }
    }
    //1. 도망  (좌회전 , 우회전 ,반대방향)  예외 사항 벽이 있을 경우
    private void RandomAway()
    {
        //정해진 시간동안 도망가고 && 탄창이 한개라도 있을 때
        if(avoidTime <= avoidWaitTime && fireController.Get_BulletCount() < 1)
        {
            avoidTime += Time.deltaTime;

            
            //랜덤 위치
            Vector3 random = RandomPosition();
            random = new Vector3(random.x, 0f, random.z);
            //반대방향 + 랜덤 위치
            Vector3 diret = (avoidDirect + random) * 30f;
            /*
            Vector3 random = map.Spot_RendomPosition(transform.position);
            Vector3 diret = random - transform.position;
            */
           Debug.DrawRay(diret, Vector3.up, Color.red, 5f);
            //회전
            Quaternion diretion = Quaternion.LookRotation(diret, Vector3.up);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion, Time.deltaTime * turnSpeed);
        }
        else
        {
            avoidTime = 0f;
            avoidState = AvoidState.Return;
        }
    }

    //랜덤 위치
    private Vector3 RandomPosition()
    {
        Vector3 position = Random.insideUnitSphere;
        return position;
    }
    //랜덤 방향
    private void RandomDirection(int num)
    {
        switch (num)
        {
            case 1:
                avoidDirect = -transform.right; //좌회전
                break;
            case 2:
                avoidDirect = transform.right; //우회전
                break;
            case 3:
                avoidDirect = -transform.forward; //반대 방향
                break;
            default:
                avoidDirect = -transform.forward;
                break;
        }
    }
    //2. 도망 갔다가 싸운 지점 복귀
    private void Return_FightPosition(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        //3. 복귀시 타겟 없으면 Wait 상태로
        if (distance < closeDistance){
            state = State.Wait;
            avoidState = AvoidState.GetDirection;// 초기화
        }
        // 싸운 지점 복귀중
        else {         
            Vector3 diret = position - transform.position;
            diret = diret.normalized;
            //회전
            Quaternion diretion = Quaternion.LookRotation(diret, Vector3.up);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, diretion, Time.deltaTime * turnSpeed);
        }
    }
    
    //4. 도망 지점이 장애물, 벽이 있을때
    private void BlockedObstacle()
    {
       //장애물이 있는지 없는지 확인

       //없으면 Go

       //있으면 Wait 으로 도망가기
    }
    #endregion

    public void OnReceiverMessage(MessageType type, object msg)
    {
        Interaction.InteractMessage message = (Interaction.InteractMessage)msg;

        switch (type)
        {
            case MessageType.HEALTH:
                hp += message.amount;
                break;
            case MessageType.DAMAGE:
                hp -= message.amount;
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
                //fireWaitTime += 0.1f;
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
