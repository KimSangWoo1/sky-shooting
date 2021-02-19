using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Move : MonoBehaviour
{

    [Range(0, 100f)]
    public float height;
    [Range(0, 50f)]
    public float limit;
    [Range(0, 10f)]
    public float speed;
    [Range(0, 360f)]
    public float turnSpeed;

    public float x;
    public float y;
    public float z;

    [SerializeField]
    private float t1;
    [SerializeField]
    private float t2;

    public Transform target;

    private Vector3 startPosition;
    private void Start()
    {
        startPosition = this.startPosition;
    }
    void Update()
    {
        Circle_exercise();

        //Circle_exercise2();
    }

    private void Circle_exercise()
    {
        
        if (target != null)
        {
            Vector3 diret = target.position - transform.position;
            diret = diret.normalized;
            this.transform.rotation = Quaternion.LookRotation(diret) * Quaternion.AngleAxis(90f,Vector3.left);

            this.transform.position = Vector3.Lerp(this.transform.position, target.position, Time.deltaTime * speed);
        }
        else
        {
            this.transform.position = new Vector3(Cos(), y, Sin()) + startPosition;
        }
    }

    private void Circle_exercise2()
    {
        if (target != null)
        {
        
            this.transform.rotation = Quaternion.LookRotation(target.position -transform.position,transform.up);
         
            this.transform.position = Vector3.Lerp(this.transform.position, target.position,Time.deltaTime *speed);
        }
        else
        {
            this.transform.position = new Vector3(x, Sin(), Cos()) + startPosition;
            

        }

    }

}
