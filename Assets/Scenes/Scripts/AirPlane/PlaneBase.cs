using UnityEngine;

public  class PlaneBase :MonoBehaviour
{
    public FX_ItemManager FX_IM;
    public FX_DeadManager FX_DM;

    [Header("비행기 기본 설정")]
    [SerializeField]
    public float runSpeed; //이동속도
    [SerializeField]
    public float turnSpeed; //회전속도
    public float runPower; //부스터 추가 이동속도
    public int hp;

    private Material material;
    public string colorName;

    public ObjectPooling.DeadState deadState;

    //FX
    private ParticleSystem[] FX;
    protected ParticleSystem engineFX; //0 기본 엔진FX
    protected ParticleSystem hitFx;  //1 타격FX
    protected ParticleSystem busterFx; //2 부스터FX
    protected ParticleSystem hurtFx; //3 출혈FX
    protected ParticleSystem deadFx; //4 죽음FX

    protected void Start()
    {
        FX_IM = FX_ItemManager.Instance;
        FX_DM = FX_DeadManager.Instance;

        //죽음FX 어떤 색인지 알아야해서
        material = GetComponent<MeshRenderer>().material;
        colorName = material.name;

        if (colorName.Contains("Red"))
        {
            deadState = ObjectPooling.DeadState.Red;
        }
        else if (colorName.Contains("Grren"))
        {
            deadState = ObjectPooling.DeadState.Green;
        }
        else if (colorName.Contains("Blue"))
        {
            deadState = ObjectPooling.DeadState.Blue;
        }
        else if (colorName.Contains("Orange"))
        {
            deadState = ObjectPooling.DeadState.Orange;
        }

        FX = transform.GetComponentsInChildren<ParticleSystem>();
        engineFX = FX[0];
        hitFx = FX[1];
        busterFx = FX[2];
        hurtFx = FX[3];
        deadFx = FX[4];

        deadFx.gameObject.SetActive(false);

    }
    protected void OnEnable()
    {
        //이동설정
        runSpeed = 10f;
        runPower = 10f;
        turnSpeed = 2f;
        
        hp = 100;
    }

    protected void HpControl()
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

    /*
#if UNITY_ANDROID || UNITY_IOS
    #region Mobile 비행기 부스터
    protected virtual void Mobile_Buster()
    {
        //이동
        transform.Translate(Vector3.down * Time.deltaTime * runSpeed, Space.Self);
        //Mobile 부스터
        if (rightPanel.buster)
        {
            runSpeed = runPower * 2;
        }
        else
        {
            runSpeed = runPower;
        }
    }
    #endregion
#endif

#if UNITY_WINDOW
    #region PC 비행기 부스터
    protected virtual void PC_Buster()
        {
            //PC 부스터
            if (Input.GetKey(KeyCode.Space))
            {
                rightPanel.buster = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rightPanel.buster = false;
            }
        }
    #endregion
#endif
    */
}

