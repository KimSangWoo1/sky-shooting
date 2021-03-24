using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TargetScanner 
{
    public Color red;
    public Color blue; 

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //부채꼴로 각도 그려줌
        Vector3 rotateForward = Quaternion.Euler(0, -angle * 0.5f, 0f) * -transform.up;
        Handles.DrawSolidArc(transform.position, Vector3.up, rotateForward, angle, radius);
    }
#endif
}
