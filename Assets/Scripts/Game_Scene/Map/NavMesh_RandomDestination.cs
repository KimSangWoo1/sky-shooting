using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMesh_RandomDestination : MonoBehaviour
{
    public float range = 10.0f; //반경
    public float life = 10.0f; //ray 표시 시간

    public Transform target;
    public bool RandomPoint( out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = target.transform.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                Debug.DrawRay(result, Vector3.up, Color.red, life);
                return true;
            }
            else
            {
                print("NN1");
            }
        }
        result = Vector3.zero;
        return false;
    }

    void Update()
    {
        Vector3 point;
        if (RandomPoint(out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, life);
            print("YY");
        }
        else
        {
            print("NN2");   
        }

    }
}
