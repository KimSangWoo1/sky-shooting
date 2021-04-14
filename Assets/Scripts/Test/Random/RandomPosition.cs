using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    [Header("랜덤 포지션 설정")]
    public float limitRange; // 범위 정하기 
    public float permit_RangeMin; // 허용 범위 최소
    public float permit_RangeMax; // 허용 범위 최대 
    [SerializeField]
    private float ray_Duration; // 레이져 시간
    public float closeDistance; // 인접 거리

    public LayerMask obstacleLayer; //장애물 레이어

    public Transform target;
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
        limitRange = 1;
        permit_RangeMin = 2;
        permit_RangeMax = 10;
        ray_Duration = 3f;
        closeDistance = 10f;
    }

    void Update()
    {
        //Spot_RendomPosition(transform.position);
        //ObstacleAvoid_RandomPosition(transform.position);
        //AngleAvoid_RandomPosition(target.position);
        RightRandom(target.position);
    }
    private void AngleAvoid_RandomPosition(Vector3 point)
    {
        Vector3 size = rend.bounds.extents; //bounds는 Object 실제 크기  .extends는 size의 반값
        size = size - Vector3.one * closeDistance + transform.position; //margine두기    +transform.position은 땅바닥 위치

        Vector2 d = Random.insideUnitCircle;
        float x = d.x * 10f;
        float z = d.y * 10f;

        //원형구멍
        if (z >= 0 && z <= permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
        }
        else if (z <= 0 && z >= -permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
        }

        point = point + (new Vector3(x, 0f, z) * limitRange);

        //비행기 360도 안에 좌표중에서만
        Vector3 _target = point - target.position;
        if (Vector3.Dot(_target.normalized, -target.forward) > Mathf.Cos(270f * 0.5f * Mathf.Deg2Rad))
        {
            //Debug.DrawRay(point, Vector3.up, Color.blue, 5f);
        }
        else
        {
            Quaternion v3Rotation = Quaternion.Euler(0f, 180f, 180f);  // 회전각     
            Vector3 v3RotatedDirection = v3Rotation * _target; // (회전각 * 회전시킬 벡터)
            point = v3RotatedDirection + target.position; //따라서 현재 포지션 더해줌
        }

        
        //좌표가 맵 경계선을 넘지 않도록 
        x = Mathf.Clamp(point.x, -size.x, size.x);
        z = Mathf.Clamp(point.z, -size.z, size.z);

        point = new Vector3(x, target.position.y, z);
        
        //고착된 경우 너무 가까이 좌표가 되는걸 방지
        float distance = Vector3.Distance(target.position, new Vector3(point.x, target.position.y, point.z));
        if (distance < permit_RangeMin * limitRange) //limitRange 곱해주는 이유는  이미 범위를 키웠기 때문에 그 범위와 동일하게 거리를 재야해서
        {

            Debug.DrawRay(point, Vector3.up, Color.blue, 5f);
        }
        else
        {
            Debug.DrawRay(point, Vector3.up, Color.red, 5f);
        }
        
    }
    private Vector3 ObstacleAvoid_RandomPosition(Vector3 point)
    {
        Vector2 d = Random.insideUnitCircle;
        float x = d.x * 10f;
        float z = d.y * 10f;

        //원형구멍
        if (z >= 0 && z <= permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
        }
        else if (z <= 0 && z >= -permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
        }

        point = point + (new Vector3(x, 0f, z) * limitRange);
        //장애물이 있을 경우
        Collider[] hitColliders = new Collider[1]; //장애물 인지 갯수 1개 해도 되고 여러개 해도 됨
        int numColliders = Physics.OverlapSphereNonAlloc(point, 10f, hitColliders, obstacleLayer, QueryTriggerInteraction.Collide);
        if (numColliders != 0)
        {
            Vector3 _point = Vector3.zero;
           // for(int i =0; i< numColliders; i++) //장애물 갯수 여러개 인지로 하고 싶을 경우
           // {
                _point = hitColliders[0].bounds.ClosestPoint(point);

                Vector3 center = hitColliders[0].bounds.center;

                Vector3 dir = _point - center;

                dir = dir.normalized;

                _point = _point + (dir * closeDistance );
                
           // }
            Debug.DrawRay(_point, Vector3.up, Color.blue, ray_Duration);
        }
        else
        {
            Debug.DrawRay(point, Vector3.up, Color.red, ray_Duration);
        }

        return point;
    }

    //레이 종류별로
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

        //원형구멍 Upgrade
        if (z >= 0 && z <= permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
        }
        else if(z <= 0 && z >= -permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
        }

        point = point + (new Vector3(x, 0f, z) * limitRange);
        Debug.DrawRay(point, Vector3.up, Color.red, ray_Duration);

        return point;
    }

    private void RightRandom(Vector3 point)
    {
        Vector3 size = rend.bounds.extents; //bounds는 Object 실제 크기  .extends는 size의 반값
        size = size - Vector3.one * closeDistance + transform.position; //margine두기    +transform.position은 땅바닥 위치

        Vector2 d = Random.insideUnitCircle;
        float x = d.x * 10f;
        float z = d.y * 10f;

        //원형구멍
        if (z >= 0 && z <= permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = permit_RangeMin;
                }
            }
        }
        else if (z <= 0 && z >= -permit_RangeMin)
        {
            if (x > 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
            else if (x < 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float _distance = Vector3.Distance(point, point2);
                if (_distance <= permit_RangeMin)
                {
                    x = -permit_RangeMin;
                    z = -permit_RangeMin;
                }
            }
        }

        point = point + (new Vector3(x, 0f, z) * limitRange);

        //비행기 360도 안에 좌표중에서만
        Vector3 _target = point - target.position;
        if (Vector3.Dot(_target.normalized, target.right) > Mathf.Cos(60f * 0.5f * Mathf.Deg2Rad))
        {
            print(point);
            Debug.DrawRay(point, Vector3.up, Color.blue, 5f);
        }
        else
        {
            Quaternion q = Quaternion.FromToRotation(_target, target.right);
            
            Vector3 v3RotatedDirection = q * _target; // (회전각 * 회전시킬 벡터)
            point = v3RotatedDirection + target.position; //따라서 현재 포지션 더해줌
            /*
            Quaternion v3Rotation = Quaternion.Euler(0f, 180f, 0f);  // 회전각     
            Vector3 v3RotatedDirection = v3Rotation * _target; // (회전각 * 회전시킬 벡터)
            point = v3RotatedDirection + target.position; //따라서 현재 포지션 더해줌
            //print("2 : " + point);
            */
            Debug.DrawRay(point, Vector3.up, Color.red, 5f);
        }


        //좌표가 맵 경계선을 넘지 않도록 
        x = Mathf.Clamp(point.x, -size.x, size.x);
        z = Mathf.Clamp(point.z, -size.z, size.z);

        point = new Vector3(x, target.position.y, z);

    }
}
