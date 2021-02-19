using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1_Manager : Singleton<Scene1_Manager>
{

    private void Start()
    {
        ObjectPooling.cloudPooling.Pop();
    }
    void Update()
    {

    }
}
