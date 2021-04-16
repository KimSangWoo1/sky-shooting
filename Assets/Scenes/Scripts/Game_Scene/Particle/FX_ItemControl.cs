using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_ItemControl : MonoBehaviour
{
    private FX_ItemManager FXM;
    ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {

    }
    void Start()
    {
        //싱글톤 생성
        FXM = FX_ItemManager.Instance;
    }

    void Update()
    {
        //끝났을 경우
        if (particle.isStopped)
        {
            FXM.Item_Push(this.gameObject); //Push 및 active 설정
        }
    }
}
