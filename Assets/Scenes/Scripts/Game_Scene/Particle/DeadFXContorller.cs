using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFXContorller : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particle.Play();
    }
    void Update()
    {
        Destroy(this.gameObject, particle.startLifetime);
    }
}
