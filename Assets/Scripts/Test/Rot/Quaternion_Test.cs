using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaternion_Test : MonoBehaviour
{
    public bool check;
    public Vector3 euler;
    public Quaternion quter;
    
    void Start()
    {
        euler = this.transform.rotation.eulerAngles;
        quter = this.transform.rotation;
        print(euler);
    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            transform.rotation = quter;
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(euler);
           
        }

    }
}
