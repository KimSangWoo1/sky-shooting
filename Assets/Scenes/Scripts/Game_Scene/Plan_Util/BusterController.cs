using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusterController : MonoBehaviour
{
    [Header("UI관련")]
    public RightPanel_Control rightPanel; //발사

    private bool possible;

    private float value;
    void Start()
    {
        value = 1f;
    }

    void Update()
    {
        //부스터 사용 가능 여부
        if (value <= 0f)
        {
            possible = false;
        }
        else
        {
            possible = true;
        }
    }

    //Player용
    public void Player_Buster_Control()
    {
        //부스터 사용 & 충전 
        if (rightPanel.buster)
        {
            //부스터 게이지 다 사용했는지 검사 
            if (possible)
            {
                DIsCharge_Gage();
            }
            else
            {
                rightPanel.buster = false;
            }
        }
        else
        {
            Charge_Gage();
        }
    }
        
    //부스터 게이지 충전
    public void Charge_Gage()
    {
        // Max 충전 10초 걸림
        value += Time.deltaTime / 10f;
        value = Mathf.Clamp(value, 0, 1f);    
        
    }
    //부스터 게이지 방전
    public void DIsCharge_Gage() {
        // Max 사용 5초 
        value -=  Time.deltaTime / 5f;
        value = Mathf.Clamp(value, 0, 1f);
    }

    //부스터 사용 GET Method
    public bool Get_Possible()
    {
        return possible;
    }

    // Player 부스터 게이지 UI 전용 메소드
    public float Change_UI_Gage()
    {
        return value;
    }
}
