using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Profile profile;

    private BulletManager BM;
    [SerializeField]
    private float bulletSpeed; //총알 속도
    private float timeSpeed; //생명 시간 속도
    private float lifeTime; //생명 시간
    private float deadTime; //죽는 시간

    private void OnEnable()
    {
        lifeTime = 0f;
    }
    private void Awake()
    {
        bulletSpeed = 80f;
        timeSpeed = 2f;
        deadTime =  1.5f;
    }
    void Start()
    {
        //싱글톤 생성
        BM = BulletManager.Instance;   
    }

    void Update()
    {
        //총 생명 시간 1초   
        lifeTime += Time.deltaTime *timeSpeed;
        if (lifeTime > deadTime)
        {
            lifeTime = 0f;  //초기화
            BM.bullet_Control(this.gameObject); //Push 및 active 설정
        }
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BM.bullet_Control(this.gameObject); //Push 및 active 설정
        }
        else if (other.gameObject.tag == "AI")
        {
            BM.bullet_Control(this.gameObject); //Push 및 active 설정
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            BM.bullet_Control(this.gameObject); //Push 및 active 설정
        }
    }
    
    public void Set_PlayerProfile(Profile _profile)
    {
        profile = _profile;
    }

    public string Get_ProfileName()
    {
        return profile.name;
    }

}
