using UnityEngine;

public class Map : MonoBehaviour
{
    [Range(10, 100)]
    public float scale; //Map 크기
    [SerializeField]
    [Range(10,30)]
    private int margine; //Map 경계선 거리두기 길이 (마진 남기기)

    public float limitRange; // 범위 정하기 
    [Range(0,10)]
    public float permit_RangeMin; // 허용 범위 최소
    [Range(0, 10)]
    public float permit_RangeMax; // 허용 범위 최대 
    [SerializeField]
    private float ray_Duration; // 레이져 시간

    public LayerMask obstacleLayer;
    private Renderer rend; //MeshRender

    void Start()
    {
        rend = GetComponent<Renderer>();
        scale = 10f;
        margine = 10;
        limitRange = 1;
        permit_RangeMin = 2;
        permit_RangeMax = 10;
        ray_Duration = 3f;
    }
    private void Update()
    {
        Spot_RendomPosition(transform.position);
    }
    //Map 크기 설정
    internal void GeneratorMap()
    {
        transform.localScale = new Vector3(scale, 1f, scale);
    }
    //Map 랜덤 좌표 받기
    public Vector3 Random_Position()
    {
        Vector3 size = rend.bounds.extents; //bounds는 Object 실제 크기  .extends는 size의 반값
        size = size - Vector3.one * margine; //margine두기
        float x = Random.Range(-size.x , size.x);
        float z = Random.Range(-size.z, size.z);
        Vector3 postion = new Vector3(x, 0f, z);
        Debug.DrawRay(postion, Vector3.up, Color.red, ray_Duration);

        return postion;
    }
    // Point 중심의 주변 랜덤 좌표 얻기
    public Vector3 Spot_RendomPosition(Vector3 point)
    {
        Vector3 size = rend.bounds.extents; //bounds는 Object 실제 크기  .extends는 size의 반값
        size = size - Vector3.one * margine; //margine두기

        Vector3 d = Random.insideUnitSphere; 
        float x = d.x * 10f;
        float z = d.z * 10f;

        //원형 구멍       
        if (z > -permit_RangeMin && z < permit_RangeMin)
        {
            if (x >= 0 && x < permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance < permit_RangeMin)
                {
                    return Vector3.zero;
                }
            }
            else if (x <= 0 && x > -permit_RangeMin)
            {
                Vector3 point2 = point + new Vector3(x, 0f, z);
                float distance = Vector3.Distance(point, point2);
                if (distance < permit_RangeMin)
                {
                    return Vector3.zero;
                }
            }
        }

        //범위 키우기
        x = x * limitRange; 
        z = z * limitRange;

        //Point가 경계선을 넘을 경우 
        x = Mathf.Clamp(x,-size.x, size.x);
        z = Mathf.Clamp(z, -size.z, size.z);

        //장애물이 있을 경우
        point = point + new Vector3(x, 0f, z);

        Collider[] hitColliders = new Collider[2];
        //Player Layer된 콜라이더 갯수만 가져오기
        int numColliders = Physics.OverlapSphereNonAlloc(point, 10f, hitColliders, obstacleLayer, QueryTriggerInteraction.Collide);
        if(numColliders > 1)
        {
            
            print("발견" + hitColliders[0].gameObject.transform.name);
            return Vector3.zero;
        }

        Debug.DrawRay(point, Vector3.up, Color.red, ray_Duration);

        return point;
    }
}
