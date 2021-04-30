using UnityEngine;


public  class PlaneBase :MonoBehaviour
{
    private PlaneManager PM;

    public Profile profile;
    [Header("비행기 기본 설정")]
    public float runSpeed; //이동속도
    [HideInInspector]
    public float turnSpeed; //회전속도
    [HideInInspector]
    public float runPower; //부스터 추가 이동속도
    public int hp;

    [HideInInspector]
    public ObjectPooling.DeadState deadState;

    //FX
    [HideInInspector]
    public FX_Manager FXM;
    [HideInInspector]
    public ItemManager IM;
    [HideInInspector]
    public BoardManager UI_BM;

    private GameObject normalFXParent;
    private GameObject bustersFXParent;
    private GameObject hitsFXParent;
    private GameObject hurtsFXParent;

    private ParticleSystem[] normalFX;
    private ParticleSystem[] bustersFX;
    private ParticleSystem[] hitsFX;
    private ParticleSystem[] hurtsFX;

    protected ParticleSystem engineFX; //0 기본 엔진FX
    protected ParticleSystem hitFx;  //1 타격FX
    protected ParticleSystem busterFx; //2 부스터FX
    protected ParticleSystem hurtFx; //3 출혈FX                     

    protected void Awake()
    {
        PM = PlaneManager.Instance;

        // AI 이름 설정
        if (transform.tag == "AI")
        {       
            profile.UpdateName(GamePlayer.Get_RandomName());
        }
        GamePlayer.ParticipatePlayer(profile);

        normalFXParent = transform.Find("FX_Normals").gameObject;
        bustersFXParent = transform.Find("FX_Busters").gameObject;
        hitsFXParent = transform.Find("FX_Hits").gameObject;
        hurtsFXParent = transform.Find("FX_Smokes").gameObject;

        normalFX = normalFXParent.GetComponentsInChildren<ParticleSystem>();
        bustersFX = bustersFXParent.GetComponentsInChildren<ParticleSystem>();
        hitsFX = hitsFXParent.GetComponentsInChildren<ParticleSystem>();
        hurtsFX = hurtsFXParent.GetComponentsInChildren<ParticleSystem>();  

        for(int i=0; i < bustersFX.Length; i++)
        {
            bustersFX[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < hitsFX.Length; i++)
        {
            hitsFX[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < hurtsFX.Length; i++)
        {
            hurtsFX[i].gameObject.SetActive(false);
        }

        engineFX = normalFX[0];
    }

    protected void OnEnable()
    {
        //죽음FX 설정
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
        //부스터FX 설정
        hitFx = hitsFX[profile.skinType];
        hurtFx = hurtsFX[profile.skinType];
        busterFx = bustersFX[profile.busterType];

        hitFx.gameObject.SetActive(true);
        hurtFx.gameObject.SetActive(true);
        busterFx.gameObject.SetActive(true);
        
        //이동설정
        runSpeed = 10f;
        runPower = 10f;
        turnSpeed = 3f;
        
        hp = 100;
    }

    protected void Start()
    {
        FXM = FX_Manager.Instance;
        IM = ItemManager.Instance;
        UI_BM = BoardManager.Instance;

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
    private void OnDisable()
    {
        hitFx.gameObject.SetActive(false);
        hurtFx.gameObject.SetActive(false);
        busterFx.gameObject.SetActive(false);

        if (gameObject.tag.Equals("Player"))
        {
            PM.Plane_Push(gameObject, ObjectPooling.PlaneState.Player);
        }
        else if (gameObject.tag.Equals("AI"))
        {
            PM.Plane_Push(gameObject, ObjectPooling.PlaneState.AI);
        }
    }
}

