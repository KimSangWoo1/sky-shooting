using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    private static ObjectPooling bulletPooling = new ObjectPooling();

    //총알 오브젝트 셋팅
    void Start()
    {
        bulletPooling.setState(ObjectPooling.Pooling_State.Bullet);
        bulletPooling.Creation();
    }

    //총알 정리
    internal void bullet_Control(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPooling.Push(bullet);
    }

    //총 발사
    internal void bullet_Fire(Transform firePosition)
    {
        GameObject bullet = bulletPooling.Pop();
        bullet.transform.position = firePosition.position;
        bullet.transform.rotation = firePosition.rotation;
        bullet.SetActive(true);
    }
}
