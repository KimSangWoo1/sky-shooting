using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Map map;
    void Start()
    {
        transform.localScale = new Vector3(map.scale * 10f, 10f, 1f);
    }

    
}
