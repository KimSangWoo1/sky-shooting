using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public float limitRange; // 범위 정하기 
    public float permit_RangeMin; // 허용 범위 최소
    public float permit_RangeMax; // 허용 범위 최대 
    [SerializeField]
    private float ray_Duration; // 레이져 시간

    public float add;
    void Start()
    {
        limitRange = 1;
        permit_RangeMin = 2;
        permit_RangeMax = 10;
        ray_Duration = 30f;
    }

    void Update()
    {
        Spot_RendomPosition(transform.position);
    }

    public Vector3 Spot_RendomPosition(Vector3 point)
    {
        Vector2 d = Random.insideUnitCircle;
        float x = d.x * 10f;
        float z = d.y  * 10f;

        //사각 구멍 원형1
        /*
        if (x >= -2 && x <=2)
        {
            if (z > 0)
            {
                z = Mathf.Clamp(z, 2, 10);
            }
            else
            {
                z = Mathf.Clamp(z, -10, -2);
            }
        }
        else
        {
            z = Mathf.Clamp(z, -10, 10);
        }
        */
        /*사각 구멍 원형2       
        if(z >= -2 && z <= 2)
        {
            if(x > 0)
            {
                x = Mathf.Clamp(x, 2, 10);
            }
            
            else
            {
                x = Mathf.Clamp(x, -10, -2);
            }          
        }
        */
        //원형 구멍       
        if (z > -permit_RangeMin && z < permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    //x = x + permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    //x = x - permit_RangeMin;
                }
            }
        }
        //
        /*몬스터볼?
        if(z>2 || z < -2)
        {
            x = Mathf.Clamp(x, -10, 10);
        }
        else
        {
            x = Mathf.Clamp(x, -2, 2);
        }
        */

        /*십자
        if (z < 2 && z >-2 )
        {
            x = Mathf.Clamp(x, -10, 10);
        }
        else
        {
            x = Mathf.Clamp(x, -2, 2);
        }
        */

        /*원형 역십자 
        if (x > 0)
        {
            x = Mathf.Clamp(x, 2, 10);
        }else  {
            x = Mathf.Clamp(x, -10, -2);
        }
        
        if(z > 0)
        {
            z = Mathf.Clamp(z, 2, 10);
        }
        else 
        {
            z = Mathf.Clamp(z, -10, -2);
        }
        */

        /*세로
        x = Mathf.Clamp(x, -2, 2);
        */

        /* 원형 역세로
        if (x > 0)
        {
            x = Mathf.Clamp(x, 2, 10);
        }else{
            x = Mathf.Clamp(x, -10, -2);
        }
        */

        /*가로
        z = Mathf.Clamp(z, -2, 2);
        */

        /*원형 역가로
        if (z > 0)
        {
            z = Mathf.Clamp(z, 2, 10);
        }
        else
        {
            z = Mathf.Clamp(z, -10, -2);
        }
        */

        /*오른쪽 반원형
         x = Mathf.Clamp(x, 0, 10);
        */

        /* 위쪽 반원형 
        z = Mathf.Clamp(z, 0, 10);
        */

        point = point + (new Vector3(x, 0f, z) * limitRange);
        Debug.DrawRay(point, Vector3.up, Color.red, ray_Duration);

        return point;
    }
}
