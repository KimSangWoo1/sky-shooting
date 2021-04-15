using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Map")]
    public GameObject map;

    [Header("벽들")]
    public GameObject upWall;
    public GameObject downWall;
    public GameObject leftWall;
    public GameObject rightWall;

    Vector3 euler;
    private float size;
    void Start()
    {
        size = map.GetComponent<Map>().scale;
        euler = map.transform.rotation.eulerAngles;

        //크기 설정
        upWall.transform.localScale = new Vector3(size * 10f, 10f, 1f);
        downWall.transform.localScale = new Vector3(size * 10f, 10f, 1f);
        leftWall.transform.localScale = new Vector3(size * 10f, 10f, 1f);
        rightWall.transform.localScale = new Vector3(size * 10f, 10f, 1f);

        //각도 설정
        transform.rotation = Quaternion.Euler(euler);
    }

    
}
