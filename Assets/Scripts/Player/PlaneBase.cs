using UnityEngine;

public  class PlaneBase :MonoBehaviour
{
    [Header("비행기 기본 설정")]
    [SerializeField]
    public float runSpeed; //이동속도
    [SerializeField]
    public float turnSpeed; //회전속도
    public float runPower; //부스터 추가 이동속도

    public int hp;

    public void OnEnable()
    {
        //이동설정
        runSpeed = 10f;
        runPower = 10f;
        turnSpeed = 2f;
        
        hp = 100;
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

