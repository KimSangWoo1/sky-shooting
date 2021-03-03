using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Flow : MonoBehaviour
{
    public Transform player;

    [Header("카메라")]
    //카메라 감도
    [SerializeField]
    private float sensitivity;
    //카메라 속도
    [SerializeField]
    private float camSpeed;
    //카메라 높이 
    [SerializeField]
    private float height;
    private void Awake()
    {
        camSpeed = 50f;
        height = 50f;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        Vector3 camPosition = new Vector3(player.transform.position.x, height, player.transform.position.z);
        //this.transform.position = Vector3.MoveTowards(this.transform.position, camPosition,Time.deltaTime * camSpeed);
        this.transform.position = Vector3.Lerp(this.transform.position, camPosition, Time.deltaTime * camSpeed);
    }
}
