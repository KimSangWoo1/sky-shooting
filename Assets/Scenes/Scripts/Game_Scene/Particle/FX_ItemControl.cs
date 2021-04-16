using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ItemControl : MonoBehaviour
{
    private FX_ItemManager FX_IM;
    ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        //싱글톤 생성
        FX_IM = FX_ItemManager.Instance;
    }

    void Update()
    {
        //끝났을 경우
        if (particle.isStopped)
        {
            FX_IM.FX_ItemPush(this.gameObject); //Push 및 active 설정
        }
    }
}
