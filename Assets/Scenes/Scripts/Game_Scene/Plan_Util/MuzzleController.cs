using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleController : MonoBehaviour
{
    BulletManager BM; //총알Manager

    [Header("Player 총구 위치")]
    public Transform leftMuzzle;
    public Transform rightMuzzle;
    public Transform centerMuzzle;

    enum Muzzle_Number {one, two, three}
    Muzzle_Number current_Muzzle;

    private int muzzle_Count; //총구 갯수
    private int bullet_Count; //총알 갯수

    private void OnEnable()
    {
        init(); //초기 설정
    }
    void Start()
    {
        BM = BulletManager.Instance;
    }

    void Update()
    {
        // 총구 갯수 최소 최대 1~3
        muzzle_Count = Mathf.Clamp(muzzle_Count, 1, 3);
        // 총알 갯수 최소 최대 1~3
        bullet_Count = Mathf.Clamp(bullet_Count, 1, 3);

        Muzzle_Setting(); //총구 설정
        Muzzle_Update();  //총구 변경
    }

    //총구 설정
    private void Muzzle_Setting()
    {
        switch (muzzle_Count)
        {
            case 1:
                current_Muzzle = Muzzle_Number.one;
                break;
            case 2:
                current_Muzzle = Muzzle_Number.two;
                break;
            case 3:
                current_Muzzle = Muzzle_Number.three;
                break;
            default:
                break;
        }   
    }
    //총구 변경
    private void Muzzle_Update()
    {
        //총구 갯수 별 총구 활성화
        switch (current_Muzzle)
        {
            case Muzzle_Number.one:
                centerMuzzle.gameObject.SetActive(true);
                leftMuzzle.gameObject.SetActive(false);
                rightMuzzle.gameObject.SetActive(false);
                break;
            case Muzzle_Number.two:
                centerMuzzle.gameObject.SetActive(false);
                leftMuzzle.gameObject.SetActive(true);
                rightMuzzle.gameObject.SetActive(true);
                break;
            case Muzzle_Number.three:
                centerMuzzle.gameObject.SetActive(true);
                leftMuzzle.gameObject.SetActive(true);
                rightMuzzle.gameObject.SetActive(true);
                break;
        }
    }

    //발사
    public void Fire()
    {
        //총구 갯수 별 총구에서 총알 발사
        switch (current_Muzzle)
        {
            case Muzzle_Number.one:
                BM.bullet_Fire(centerMuzzle);
                break;
            case Muzzle_Number.two:
                BM.bullet_Fire(leftMuzzle);
                BM.bullet_Fire(rightMuzzle);
                break;
            case Muzzle_Number.three:
                BM.bullet_Fire(centerMuzzle);
                BM.bullet_Fire(leftMuzzle);  
                BM.bullet_Fire(rightMuzzle);
                break;
        }
    }

    //총구 증가
    public void Add_Muzzle()
    {
        muzzle_Count += 1;
    }
    //총알 증가
    public void Add_Bullet()
    {
        bullet_Count += 1;
    }

    //총알 갯수 Get Method
    public int Get_BulletCount()
    {
        return bullet_Count;
    }
    //초기 설정
    public void init()
    {
        muzzle_Count = 1;
        bullet_Count = 1;

        current_Muzzle = Muzzle_Number.one;
    }
}
