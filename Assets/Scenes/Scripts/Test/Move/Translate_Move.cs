using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate_Move : MonoBehaviour
{
    // Start is called before the first frame update
    public bool vector;
    public bool check;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vector)
        {
            VectorMove();
        }
        else
        {
            transformMove();
        }
    }

    void VectorMove()
    {
        if (check)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 10f, Space.World);
            print("Vector , Spece.World");
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 10f, Space.Self);
            print("Vector , Spece.Self");
        }
    }
    void transformMove()
    {
        if (check)
        {
            transform.Translate(transform.forward * Time.deltaTime * 10f, Space.World);
            print("transform , Spece.World");
        }
        else
        {
            transform.Translate(transform.forward * Time.deltaTime * 10f, Space.Self);
            print("transform , Spece.Self");
        }
    }

}
