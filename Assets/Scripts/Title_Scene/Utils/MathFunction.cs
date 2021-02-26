using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathFunction : MonoBehaviour
{
    //[Range(0, 100f)]
    public float height;
    //[Range(0, 50f)]
    public float limit;
    //[Range(0, 10f)]
    public float speed;

    private float t1;
    private float t2;


    //주로 높낮이 Vector3 Y에 사용되는게 좋음  
    protected float Sin()
    {   
        t1 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Sin(t1);
        return result;
    }
    protected float Cos()
    {
        t2 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Cos(t2);
        return result;
    }
    protected float Sin_Twice()
    {
        t1 += Time.fixedDeltaTime * speed *2f;
        float result = height + limit * Mathf.Sin(t1);
        return result;
    }

    protected float Sin_Abs()
    {
        t1 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Abs(Mathf.Sin(t1)); //반원 호만 그리기    
        return result;
    }
    protected float Sin_Control(float min, float max)
    {
        t1 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Clamp(Mathf.Sin(t1), min, max);
        return result;

    }
    protected float Cos_Control(float min, float max)
    {
        t2 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Clamp(Mathf.Cos(t2),min,max);
        return result;

    }
}
