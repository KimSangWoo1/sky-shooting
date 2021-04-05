using UnityEngine;

public class Map : MonoBehaviour
{
    [Range(1f, 100f)]
    public float scale; //Map 크기
    [SerializeField]
    [Range(10,30)]
    private int margine; //Map 경계선 거리두기 길이 (마진 남기기)
    [SerializeField]
    private float ray_Duration; // 레이져 시간

    private Renderer rend; //MeshRender
    void Start()
    {
        rend = GetComponent<Renderer>();
        scale = 10f;
        margine = 1;
        ray_Duration = 3f;
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
}
