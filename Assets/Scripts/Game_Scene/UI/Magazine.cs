using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour
{
    [Header("탄창 총알s")]
    public Image bullet_One;
    public Image bullet_Two;
    public Image bullet_Three;

    private Color alpha_Half; //알파 반값
    private Color alpha_Full; //알파 풀값

    private float chareTime; //탄알 충전 시간
    private int bulletCount; //총알 잔여량

    private bool fireable;

    private void Awake()
    {
        //발사 설정
        bulletCount = 3; 
    }
    void Start()
    {
        alpha_Half = new Color(255f, 255f, 255f, 0.5f);
        alpha_Full = new Color(255f, 255f, 255f, 1f);
    }

    void Update()
    {
        //탄알 1개 충전 시간 3초
        chareTime += Time.deltaTime /3f;

        chareTime = Mathf.Clamp(chareTime, 0f, 1f);
        bulletCount = Mathf.Clamp(bulletCount, 0, 3);

        Charge_Bullet(); //탄알 장전
        Bullet_Remain(); //탄창 처리
    }
    //탄알 UI 알파 조절
    private void Bullet_Remain()
    {    
        switch (bulletCount)
        {
            case 3:
                chareTime = 0f;
                fireable = true;

                bullet_One.color = alpha_Full;
                bullet_Two.color = alpha_Full;
                bullet_Three.color = alpha_Full;
                break;
            case 2:
                fireable = true;

                bullet_One.color = alpha_Half;
                bullet_Two.color = alpha_Full;
                bullet_Three.color = alpha_Full;
                break;
            case 1:
                fireable = true;

                bullet_One.color = alpha_Half;
                bullet_Two.color = alpha_Half;
                bullet_Three.color = alpha_Full;
                break;
            case 0:
                fireable = false;

                bullet_One.color = alpha_Half;
                bullet_Two.color = alpha_Half;
                bullet_Three.color = alpha_Half;
                break;
            default:
                fireable = false;
                break;
        }
    }
    //발사
    public void Shot()
    {
        bulletCount -= 1;
        Dischare_Bullet(); //탄알 사용 후 처리
    }
    //장전 시스템
    private void Charge_Bullet()
    {
        switch (bulletCount)
        {
            case 3:
                break;
            case 2:
                Ready_Bullet(bullet_One);
                break;
            case 1:
                Ready_Bullet(bullet_Two);
                break;
            case 0:
                Ready_Bullet(bullet_Three);
                break;
            default:
                break;
        }
    }
    //탄알 사용
    private void Dischare_Bullet()
    {
        switch (bulletCount)
        {
            case 3:              
                break;
            case 2:
                bullet_One.fillAmount = 0f;
                break;
            case 1:
                bullet_One.fillAmount = 0f;
                bullet_Two.fillAmount = 0f;
                break;
            case 0:
                bullet_One.fillAmount = 0f;
                bullet_Two.fillAmount = 0f;
                bullet_Three.fillAmount = 0f;
                break;
            default:
                break;
        }
    }
    //탄창 준비하기
    private void Ready_Bullet(Image bullet)
    {
        if (bullet.fillAmount < 1f)
        {
            bullet.fillAmount = chareTime;
        }
        else
        {
            bulletCount += 1;
            chareTime = 0f;
        }
    }
    //사격 사용 가능 여부 GET Method
    public bool get_Fireable()
    {
        return fireable;
    }
}


