using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject target;
    
    public float cloudSpeed = 10.0f;
    private Vector3 startPosition;


    private void OnEnable()
    {
        startPosition = this.transform.position;
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.Find("EndPoint")  ;
        }
        
    }

    void Update()
    {

        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance < 50f)
        {
            this.gameObject.SetActive(false);
            ObjectPooling.cloudPooling.Push(this.gameObject);
            
        }
        print("거리" + distance);
        transform.Translate(transform.forward * cloudSpeed * Time.deltaTime, Space.Self); 
    }
  
}
