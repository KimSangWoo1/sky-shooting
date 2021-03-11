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

    private int bulletCount; //총알 잔여량

    //체크용
    private bool one;
    private bool two;
    private bool three;

    private void Awake()
    {
        one = true; // T :사용 , F :미사용
        two = true;
        three = true;

        bulletCount = 3; //총알 잔여량
    }
    void Start()
    {
        alpha_Half = new Color(255f, 255f, 255f, 0.5f);
        alpha_Full = new Color(255f, 255f, 255f, 1f);
    }

    void Update()
    {
        Bullet_Remain();
    }

    private void Bullet_Remain()
    {
        switch (bulletCount)
        {
            case 3:
                bullet_One.color = alpha_Full;
                bullet_Two.color = alpha_Full;
                bullet_Three.color = alpha_Full;
                break;
            case 2:
                one = false;

                bullet_One.color = alpha_Half;
                bullet_Two.color = alpha_Full;
                bullet_Three.color = alpha_Full;
                break;
            case 1:
                two = false;
                bullet_One.color = alpha_Half;
                bullet_Two.color = alpha_Half;
                bullet_Three.color = alpha_Full;
                break;
            case 0:
                three = false;
                bullet_One.color = alpha_Half;
                bullet_Two.color = alpha_Half;
                bullet_Three.color = alpha_Half;
                break;
            default:
                break;
        }
    }

    public void Shot()
    {
        bulletCount -= 1;
    }


}


