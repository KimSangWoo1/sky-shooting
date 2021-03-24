using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Machine : MonoBehaviour
{
    public TargetScanner scanner;
    int maxColliders = 10;

    void Update()
    {
       Transform target = scanner.Detect(transform);
       if(target != null)
        {
            transform.rotation = Quaternion.LookRotation(target.position,Vector3.up) * Quaternion.AngleAxis(-90f, Vector3.right);
        }
    }

    //레이더 적군 찾기


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        scanner.EditorScanner(transform);
    }
#endif
    
}
