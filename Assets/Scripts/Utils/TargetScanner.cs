using UnityEngine;
using UnityEditor;

[System.Serializable]
public class TargetScanner
{
    [Header("거리 및 반경")]
    public float radius;
    [Range(0f,360f)]
    public float angle;
    [Header("레이더 검출 최대 갯수")]
    public int maxColliders;
    [Header("색 조정")]
    public Color red;
    public Color blue;
    [Header("Layer설정")]
    public LayerMask blockLayerMask; //장애물 레이어
    public LayerMask targetLayerMask; //목표물 레이어

    public bool canSee;

    public int count; //범위 안에 있는 타겟 갯수
    public Transform Detect(Transform transform)
    {
        //받아들일 콜라이더 갯수 설정
        Collider[] hitColliders = new Collider[maxColliders];
        //Player Layer된 콜라이더 갯수만 가져오기
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, targetLayerMask, QueryTriggerInteraction.Ignore);  
        count = 0;

        if (numColliders != 0)
        {
            float shortDistance = radius * radius;
            int index = maxColliders+1;
          
            for (int i = 0; i < numColliders; i++)
            {
                Vector3 _target = (hitColliders[i].transform.position + Vector3.up) - transform.position;
                _target.y = 0;
                //angle안에 있을 경우
                if (Vector3.Dot(_target.normalized, transform.forward) > Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                {
                    canSee = false;
                    canSee |= !Physics.Raycast(transform.position, _target.normalized, radius, blockLayerMask, QueryTriggerInteraction.Ignore);

                    if (canSee)
                    {
                        count++;
                        Debug.DrawRay(transform.position, _target, Color.black);

                        float targetDistance = _target.sqrMagnitude;
                        //가장 가까운 적 index
                        if (targetDistance <= shortDistance)
                        {
                            shortDistance = targetDistance;
                            index = i;
                        }
                    }
                }
                //범위 안에는 있지만 angle안에 없을 경우
                else
                {
                    if (count <= 0)
                    {
                        canSee = false;
                    }                            
                    //target이 범위 안에 있다가 밖으로 나간 경우 (아직 구현안됨)
                }
            }
            if(index!= (maxColliders+1))
            {
                return hitColliders[index].transform;
            }
        }
        //싹다 범위 밖에 있었던 경우
        else
        {
            Debug.Log("없다");
            canSee = false;
            count = 0;
        }
        count = Mathf.Clamp(count, 0, maxColliders);
        return null;
    }

#if UNITY_EDITOR
        public void EditorScanner(Transform transform)
    {
        Handles.color = canSee ? red : blue; //색 설정
        //부채꼴로 각도 그려줌
        Vector3 rotateForward = Quaternion.Euler(0, -angle * 0.5f, 0f) * transform.forward;
        Handles.DrawSolidArc(transform.position, Vector3.up, rotateForward, angle, radius);
    }
#endif
}
