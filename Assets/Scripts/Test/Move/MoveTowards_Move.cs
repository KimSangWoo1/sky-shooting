using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards_Move : MonoBehaviour
{
    public Transform target;
    public float speed;
    public bool lerp;
    public bool movoTowards;

    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //위치 초기화
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = startPosition;
        }

        //움직임
        if (movoTowards)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed);
        }
        else if (lerp)
        {
            this.transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        }
    }
}
