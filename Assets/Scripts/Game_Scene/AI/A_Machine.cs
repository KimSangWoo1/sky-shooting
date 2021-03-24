using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
public class A_Machine : MonoBehaviour
{

    public float radius;
    public float angle;

    public LayerMask blockLayerMask;
    private LayerMask playerLayerMask;
    private Color red = new Color(1f, 0f, 0f, 0.2f);
    private Color blue = new Color(0f, 1f, 1f, 0.2f);

    int maxColliders = 10;

    [SerializeField]
    private bool canSee;
    private void Start()
    {
        playerLayerMask = LayerMask.GetMask("Player");
    }
    void Update()
    {
            Detect();
    }

    //고정 target이 없을 경우
    public void Detect()
    {
        //받아들일 콜라이더 갯수 설정
        Collider[] hitColliders = new Collider[maxColliders];
        //Player Layer된 콜라이더 갯수만 가져오기
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, playerLayerMask, QueryTriggerInteraction.Ignore);
   
        if (numColliders != 0)
        {
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
                        Debug.DrawRay(transform.position, _target, Color.black);
                    }
                }
                else
                {
                    canSee = false;
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = canSee ? red : blue; //색 설정
        //부채꼴로 각도 그려줌
        Vector3 rotateForward = Quaternion.Euler(0, -angle * 0.5f, 0f) * -transform.up;
        Handles.DrawSolidArc(transform.position, Vector3.up, rotateForward, angle, radius);
    }
#endif
}
