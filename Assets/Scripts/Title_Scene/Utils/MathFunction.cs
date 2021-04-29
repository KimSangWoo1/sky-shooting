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
    private float t3;
    private float t4;
    private float t5;
    private float t6;
    private float t7;

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
        t3 += Time.fixedDeltaTime * speed *2f;
        float result = height + limit * Mathf.Sin(t3);
        return result;
    }
    protected float Cos_Twice()
    {
        t4 += Time.fixedDeltaTime * speed * 2f;
        float result = height + limit * Mathf.Cos(t4);
        return result;
    }

    protected float Sin_Abs()
    {
        t5 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Abs(Mathf.Sin(t5)); //반원 호만 그리기    
        return result;
    }
    protected float Sin_Control(float min, float max)
    {
        t6 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Clamp(Mathf.Sin(t6), min, max);
        return result;

    }
    protected float Cos_Control(float min, float max)
    {
        t7 += Time.fixedDeltaTime * speed;
        float result = height + limit * Mathf.Clamp(Mathf.Cos(t7),min,max);
        return result;

    }
}
