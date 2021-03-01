using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle_Move : MonoBehaviour
{
    [Range(0, 100f)]
    public float height;
    [Range(0, 50f)]
    public float limit;
    [Range(0, 10f)]
    public float speed;

    public float a;
    public float b;
    public float c;
    public float d;

    public bool check;


    public float x;
    public float y;
    public float _z;
    private float z;

    public float turnSpeed;
    [SerializeField]
    private float t1;
    [SerializeField]
    private float t2;

    private Vector3 startPoint;
    private Quaternion startRotation;
    private void Start()
    {
        startPoint = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Sin();
        //Cos();
        //Tan();
        z += Time.deltaTime * speed* limit;
        //Circle_exercise();
        Half_exercise();
    }
    //주로 높낮이 Vector3 Y에 사용되는게 좋음  
    private float Sin()
    {
        t1 += Time.deltaTime * speed;
        float result_1 = height + limit * Mathf.Clamp(Mathf.Sin(t1), a, b); //반원 그리기
        float result_2 = height + limit * Mathf.Abs(Mathf.Sin(t1)); //반원 호만 그리기
     
        return check ? result_1 : result_2;       

    }
    private float Cos()
    {
        t2 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Clamp(Mathf.Cos(t2), c, d);
        return result;
        //Debug.Log("코사인" + result);
    }

    private void Circle_exercise()
    {  
        this.transform.position += new Vector3(Cos(), y, Sin());
    }

    private void Half_exercise()
    {
        float cos = Cos();
        this.transform.position = new Vector3(cos, -Sin(), - z) + startPoint;
        this.transform.rotation =  Quaternion.Euler(x,y, cos * turnSpeed) * startRotation;
    }
}

