using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buster_Gage : MonoBehaviour
{
    private Image gage;

    private float value;

    private Color green;
    private Color red;
    private Color orange;

    private bool possible;

    private void Awake()
    {
        value = 1f;
    }
    void Start()
    {
        gage = GetComponent<Image>();
        green = new Color(21f/255f, 233f/255f, 140f/255f);
        red = new Color(245f / 255f, 57f / 255f, 6f/255f);
        orange = new Color(245f / 255f, 182f / 255f, 7f / 255f);
        gage.color = green;
  

    }
    private void Update()
    {
        //부스터 색 변화
        Change_Color();

        //부스터 사용 가능 여부
        if (gage.fillAmount <= 0f)
        {
            possible = false;
        }
        else
        {
            possible = true;
        }
    }
    //부스터 사용 GET Method
    public bool Get_Possible()
    {
        return possible;
    }
    //부스터 색 변화
    private void Change_Color()
    {
        if (gage.fillAmount >= 0.7f)
        {
            gage.color = green;
        }else if(gage.fillAmount >= 0.3f)
        {
            gage.color = orange;
        }
        else
        {
            gage.color = red;
        }
    }
    //부스터 게이지 충전
    public void Charge_Gage()
    {
        // Max 충전 10초 걸림
        value += Time.deltaTime / 10f;
        value = Mathf.Clamp(value, 0f, 1f);
        gage.fillAmount = value;
    }
    //부스터 게이지 방전
    public void DIsCharge_Gage() {
        // Max 사용 5초 
        value -= Time.deltaTime / 5f;
        value = Mathf.Clamp(value, 0f, 1f);
        gage.fillAmount = value;
    }
}
