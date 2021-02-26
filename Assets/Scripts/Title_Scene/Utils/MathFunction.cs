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
       
        t1 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Sin(t1);
        return result;
        // Debug.Log("사인" + result);

    }
    protected float Cos()
    {
        t2 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Cos(t2);
        return result;
        //Debug.Log("코사인" + result);
    }

    protected float Sin_Control()
    {
        t1 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Abs(Mathf.Sin(t1)); //반원 호만 그리기    
        return result;
    }
    protected float Cos_Control(float min, float max)
    {
        t2 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Clamp(Mathf.Cos(t2),min,max);
        return result;

    }
}
