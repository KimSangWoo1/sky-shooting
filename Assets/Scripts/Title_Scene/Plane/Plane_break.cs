using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_break : MathFunction
{
    public Transform target;

    void Start()
    {
        
    }


    void Update()
    {
        Vector3 diret = target.position - transform.position;
        diret = diret.normalized;
        this.transform.rotation = Quaternion.LookRotation(diret) * Quaternion.AngleAxis(90f, Vector3.left);

        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position,
            speed);
    }
}
