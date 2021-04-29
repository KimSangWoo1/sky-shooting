using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoice : MonoBehaviour
{
    public GameObject[] players;

    void Start()
    {
        players[GameManager.planeNumber].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChoicePlane(int num)
    {
        GameManager.planeNumber = num;
        switch (num)
        {
            case 0:
                players[0].gameObject.SetActive(true);
                players[1].gameObject.SetActive(false);
                players[2].gameObject.SetActive(false);
                players[3].gameObject.SetActive(false);

                break;
            case 1:
                players[0].gameObject.SetActive(false);
                players[1].gameObject.SetActive(true);
                players[2].gameObject.SetActive(false);
                players[3].gameObject.SetActive(false);
                break;
            case 2:
                players[0].gameObject.SetActive(false);
                players[1].gameObject.SetActive(false);
                players[2].gameObject.SetActive(true);
                players[3].gameObject.SetActive(false);
                break;
            case 3:
                players[0].gameObject.SetActive(false);
                players[1].gameObject.SetActive(false);
                players[2].gameObject.SetActive(false);
                players[3].gameObject.SetActive(true);
                break;
            default:
                players[0].gameObject.SetActive(true);
                players[1].gameObject.SetActive(false);
                players[2].gameObject.SetActive(false);
                players[3].gameObject.SetActive(false);
                break;
        }
    }
}
