using UnityEngine;
using Message;

[DefaultExecutionOrder(100)]
public class B_Machine : PlaneBase, IMessageReceiver
{
    enum State{Wait, Found, Attack, Avoid, Item, Dead}; //비행기 상태
    enum WaitState { GetPosition, GoPosition}; //비행기 대기 상태
    State state;
    WaitState waitState;

    public TargetScanner scanner; //스캐너
    public Map map; // 맵
    public MuzzleController muzzle; //총구

    [Header("랜덤 좌표 접근 인정 거리")]
    [SerializeField]
    private float closeDistance; //랜덤 좌표 인증거리

    private Vector3 random_Position; // Map  랜덤 좌표


    private void OnEnable()
    {
        state = State.Wait;
        waitState = WaitState.GetPosition;
    }

    void Update()
    {
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
                Rot();
                break;
            case State.Attack:
                break;
            case State.Avoid:
                break;
            case State.Item:
                break;
        }
        Move(); //이동
    }
    //이동
    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * runSpeed, Space.Self);
    }

    #region  적방향 회전 & 적 쫒기
    //찾기
    private void Find()
    {
        Transform target = scanner.Detect(transform);
        //회전
        if (target != null)
        {
            Vector3 direct = target.position - transform.position;
            if (direct != Vector3.zero)
            {
                float dot = Vector3.Dot(transform.forward, direct.normalized);
                Vector3 cross = Vector3.Cross(transform.forward, direct.normalized);
                dot = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (cross.y > 0f)
                {
                    print("오른쪽");
                }
                else if (cross.y < 0f)
                {
                    print("왼쪽");
                    dot = -dot;
                }
                Quaternion quater = Quaternion.AngleAxis(dot, Vector3.up);
                Quaternion look = Quaternion.LookRotation(direct.normalized, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, quater, Time.fixedDeltaTime * turnSpeed);
            }
        }
    }

    //레이더 안에 들어온 타겟 바라보기
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
                if (random_Position != Vector3.zero ||random_Position !=null)
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
        if(distance < closeDistance)
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
            scanner.EditorScanner(this.transform);
    }
#endif
}
