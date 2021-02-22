﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Move : MathFunction
{
    public Transform target;
    
    public float x;
    public float y;
    public float z;

    private Vector3 circle_StartPosition;
    private float move_Time;
    private int timeSpeed;

    private bool circle;

    private void Start()
    {
        timeSpeed = 2;

    }

    void Update()
    {
        move_Time += Time.deltaTime * timeSpeed;
        //6초 후 원이동
        if (move_Time >= 6f)
        {
            if (!circle)
            {
                circle = true;
                circle_StartPosition = this.transform.position; //원 돌기전 포인트!
            }
        }
        else
        {
            Straight(); 
        }
        if (circle)
        {
            StartCoroutine(Circle_Movement()); 
        }
       
    }
    //원 이동 및 체크
    IEnumerator Circle_Movement()
    {
        Vercical_Circle(); //원 이동
        float result = transform.position.y - circle_StartPosition.y; //길이 및 방향
        float dot = Vector3.Dot(Vector3.forward, (transform.position - circle_StartPosition).normalized); //내적

        // 내적하여 원 이동 시작지점 뒤에 비행기가 있고 거리 길이가 1보다 작을때까지 기달린다.
        yield return new WaitUntil(() => dot<0 && result <= 1f);
        // 다 돌았다
        move_Time = 0f;
        circle = false;
  
    }

    //원 이동
    private void Vercical_Circle()
    {
        if (target != null)
        {
            Vector3 diret = target.position - transform.position;
            Quaternion rotation;

            // Vector3.up 기준 타겟 방향
            if (diret.normalized.z > 0f)
            {
                rotation = Quaternion.LookRotation(diret, Vector3.up) * Quaternion.AngleAxis(90f, -transform.right);
            }
            else
            {
                rotation = Quaternion.LookRotation(diret, Vector3.down) * Quaternion.AngleAxis(90f, -transform.right);
            }
            this.transform.rotation = rotation;
            this.transform.position = Vector3.Lerp(this.transform.position, target.position,Time.deltaTime *speed);
        }
        else
        {
            this.transform.position += new Vector3(x, Sin(), Cos());
        }
    }

    //직진 이동
    private void Straight()
    {
        this.transform.Translate(-transform.forward *speed, Space.Self) ;
    }
    private void Horizontal_Circle()
    {
        if (target != null)
        {
            Vector3 diret = target.position - transform.position;
            diret = diret.normalized;
            this.transform.rotation = Quaternion.LookRotation(diret) * Quaternion.AngleAxis(90f, Vector3.left);

            this.transform.position = Vector3.Lerp(this.transform.position, target.position, Time.deltaTime * speed);
        }
        else
        {
            this.transform.position += new Vector3(Cos(), y, Sin());
        }
    }
}

/* 파도 처럼 움직이기
Straight();
StartCoroutine(Circle_Movement());
*/