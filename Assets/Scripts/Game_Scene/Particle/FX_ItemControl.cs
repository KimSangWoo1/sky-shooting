using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ItemControl : MonoBehaviour
{
    private FX_Manager FXM;
    ParticleSystem particle;
    [SerializeField]
    ObjectPooling.FX_State fxState;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        //싱글톤 생성
        FXM = FX_Manager.Instance;
    }

    void Update()
    {
        //끝났을 경우
        if (particle.isStopped)
        {
            if (fxState == ObjectPooling.FX_State.item)
            {
                FXM.FX_ItemPush(this.gameObject); //Push 및 active 설정
            }
            else if(fxState == ObjectPooling.FX_State.Money)
            {
                FXM.FX_MoneyPush(this.gameObject);
            }
        }
    }
}
