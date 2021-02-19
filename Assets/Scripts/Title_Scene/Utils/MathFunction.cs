using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathFunction : MonoBehaviour
{
    [SerializeField]
    private float t1;
    [SerializeField]

    private float t2;
    //주로 높낮이 Vector3 Y에 사용되는게 좋음  
    private float Sin()
    {
        t1 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Sin(t1);
        return result;
        // Debug.Log("사인" + result);

    }
    private float Cos()
    {
        t2 += Time.deltaTime * speed;
        float result = height + limit * Mathf.Cos(t2);
        return result;
        //Debug.Log("코사인" + result);
    }
}
