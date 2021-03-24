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

    public int count;
    public Transform Detect(Transform transform)
    {
        //받아들일 콜라이더 갯수 설정
        Collider[] hitColliders = new Collider[maxColliders];
        //Player Layer된 콜라이더 갯수만 가져오기
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, targetLayerMask, QueryTriggerInteraction.Ignore);
        count = 0;
        count = Mathf.Clamp(count, 0, maxColliders);
        if (numColliders != 0)
        {
            float shortDistance = radius * radius;
            int index = maxColliders+1;
          
            for (int i = 0; i < numColliders; i++)
            {
                Vector3 _target = (hitColliders[i].transform.position + Vector3.up) - transform.position;
                _target.y = 0;
                if (Vector3.Dot(_target.normalized, -transform.up) > Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                {
                    canSee = false;
                    canSee |= !Physics.Raycast(transform.position, _target.normalized, radius, blockLayerMask, QueryTriggerInteraction.Ignore);

                    if (canSee)
                    {
                        count++;
                        Debug.DrawRay(transform.position, _target, Color.black);

                        float targetDistance = _target.sqrMagnitude;
                        if (targetDistance <= shortDistance)
                        {
                            shortDistance = targetDistance;
                            index = i;
                        } 
                    }
                }
                else
                {
                    count--;
                    canSee = false;

                    //target이 범위 안에 있다가 밖으로 나간 경우 

                    //target이 범위안에 한개라도 있을 경우

                    //싹다 범위 밖에 있었던 경우

                }
            }
            if(index!= (maxColliders+1))
            {
                return hitColliders[index].transform;
            }
        }
        else
        {
            canSee = false;
        }
        return null;
    }

#if UNITY_EDITOR
        public void EditorScanner(Transform transform)
    {
        Handles.color = canSee ? red : blue; //색 설정
        //부채꼴로 각도 그려줌
        Vector3 rotateForward = Quaternion.Euler(0, -angle * 0.5f, 0f) * -transform.up;
        Handles.DrawSolidArc(transform.position, Vector3.up, rotateForward, angle, radius);
    }
#endif
}
