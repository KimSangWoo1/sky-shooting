using UnityEngine;


public  class PlaneBase :MonoBehaviour
{
    public Profile profile;
    [Header("비행기 기본 설정")]
    public float runSpeed; //이동속도
    [HideInInspector]
    public float turnSpeed; //회전속도
    [HideInInspector]
    public float runPower; //부스터 추가 이동속도
    public int hp;

    private Material material;
    [HideInInspector]
    public ObjectPooling.DeadState deadState;

    //FX
    [HideInInspector]
    public FX_Manager FXM;
    [HideInInspector]
    public ItemManager IM;
    [HideInInspector]
    public BoardManager UI_BM;

    private ParticleSystem[] FX;
    protected ParticleSystem engineFX; //0 기본 엔진FX
    protected ParticleSystem hitFx;  //1 타격FX
    protected ParticleSystem busterFx; //2 부스터FX
    protected ParticleSystem hurtFx; //3 출혈FX                     

    private void Awake()
    { 
        // AI 이름 설정
        if (transform.tag == "AI")
        {       
            profile.UpdateName(GamePlayer.Get_RandomName());
        }
        GamePlayer.ParticipatePlayer(profile);
    }

    protected void Start()
    {
        FXM = FX_Manager.Instance;
        IM = ItemManager.Instance;
        UI_BM = BoardManager.Instance;

        //죽음FX 어떤 색인지 알아야해서
        switch (profile.skinType)
        {
            case 0:
                deadState = ObjectPooling.DeadState.Red;
                break;
            case 1:
                deadState = ObjectPooling.DeadState.Green;
                break;
            case 2:
                deadState = ObjectPooling.DeadState.Blue;
                break;
            case 3:
                deadState = ObjectPooling.DeadState.Orange;
                break;
            default:
                deadState = ObjectPooling.DeadState.Red;
                break;
        }

        material = GetComponent<MeshRenderer>().material;

        FX = transform.GetComponentsInChildren<ParticleSystem>();
        engineFX = FX[0];
        hitFx = FX[1];
        busterFx = FX[2];
        hurtFx = FX[3];

    }
    protected void OnEnable()
    {
        //이동설정
        runSpeed = 10f;
        runPower = 10f;
        turnSpeed = 3f;
        
        hp = 100;
    }

    protected void HPCheck()
    {
        if (hp <= 50f)
        {
            if (!hurtFx.isPlaying)
            {
                hurtFx.Play();
            }
        }
        else
        {
            hurtFx.Stop();
        }
    }
    //죽었을 경우 아이템 랜덤으로 POP
    protected void Item_Random()
    {
        int random = Random.Range(1, 10);
        switch (random)
        {
            case 1:
                IM.Item_Pop(transform, ObjectPooling.Item_State.Bullet);
                break;
            case 2:
                IM.Item_Pop(transform, ObjectPooling.Item_State.Muzzle);
                break;
            case 3:
                IM.Item_Pop(transform, ObjectPooling.Item_State.Turbin);
                break;
            case 4:
                IM.Item_HealthPop(transform, ObjectPooling.Item_HealthState.Red);
                break;
            case 5:
                IM.Item_HealthPop(transform, ObjectPooling.Item_HealthState.Yellow);
                break;
            case 6:
                IM.Item_HealthPop(transform, ObjectPooling.Item_HealthState.Green);
                break;
            case 7:
                IM.Item_DollarPop(transform, ObjectPooling.Item_DollarState.Red);
                break;
            case 8:
                IM.Item_DollarPop(transform, ObjectPooling.Item_DollarState.Yellow);
                break;
            case 9:
                IM.Item_DollarPop(transform, ObjectPooling.Item_DollarState.Green);
                break;
            case 10:
                //아이템 없음
                break;

        }
    }
}

