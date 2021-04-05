using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_break : MathFunction
{
    public Transform target;

    private Title_SceneManager TS;
    private void Start()
    {
        TS = Title_SceneManager.Instance;
    }

    void Update()
    {
        Vector3 diret = target.position - transform.position;
        diret = diret.normalized;
        this.transform.rotation = Quaternion.LookRotation(diret);

        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position,speed);

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= 2f)
        {
            TS.SetActionState(Title_SceneManager.Action_State.cameraBreakAction);
        }
    }
}
