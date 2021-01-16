using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{

    private float cloudSpeed = 2.0f;

    void Start()
    {
        
    }

  

    void Update()
    {
        transform.Translate(transform.forward * cloudSpeed * Time.deltaTime, Space.Self);
       
    }
}
