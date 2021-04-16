using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_DeadContorl : MonoBehaviour
{
    private FX_DeadManager FX_DM;
    private ParticleSystem particle;
    ObjectPooling.DeadState deadState;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        //싱글톤 생성
        FX_DM = FX_DeadManager.Instance;
        if (this.transform.name.Contains("Red"))
        {
            deadState = ObjectPooling.DeadState.Red;
        }else if (this.transform.name.Contains("Green"))
        {
            deadState = ObjectPooling.DeadState.Green;
        }
        else if (this.transform.name.Contains("Blue"))
        {
            deadState = ObjectPooling.DeadState.Blue;
        }
        else if (this.transform.name.Contains("Orange"))
        {
            deadState = ObjectPooling.DeadState.Orange;
        }
    }
    private void OnEnable()
    {
        particle.Play();
    }
    void Update()
    {
        //끝났을 경우
        if (particle.isStopped)
        {
            FX_DM.FX_Push(this.gameObject, deadState); //Push 및 active 설정
        }
    }
}
