using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_DeadContorl : MonoBehaviour
{
    private FX_Manager FXM;
    private ParticleSystem particle;
    [SerializeField]
    ObjectPooling.DeadState deadState;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        //싱글톤 생성
        FXM = FX_Manager.Instance;
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
            FXM.FX_Push(this.gameObject, deadState); //Push 및 active 설정
        }
    }
}
