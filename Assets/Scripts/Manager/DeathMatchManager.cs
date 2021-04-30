using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMatchManager : Singleton<DeathMatchManager>
{
    public Map map;
    public Material[] materials;
    public GameObject Player;

    private void Awake()
    {
        GamePlayer.init();
        GamePlayer.Make_RandomName();
    }

    void Start()
    {
        Player.GetComponent<Renderer>().material = materials[GameManager.planeNumber];
        Player.GetComponent<PlaneBase>().profile.skinType = GameManager.planeNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Plane_ReadyPosition()
    {
        Vector3 pos = map.Random_Position();

        //플레이어든 AI든 위치 및 각도 설정
        GameObject plane = new GameObject(); //Plane.Pop();
        plane.transform.position = pos;
        plane.transform.LookAt(Vector3.one);
        plane.SetActive(true);

        //Player면 Plane OnEnable 무적 3초 보호막 (발사, 아이템 먹기 금지) 

        //AI면 Wait 상태로 무적3초 보호막 Found x Attack X 아이템 먹기X , Emergency 가동)
        
        //비행기 최대 갯수 관리 50대
        //Player 들어오면 점수 가장 낮은 AI 폭파

        //Player 들어올시 꽉 차면 다른 방에 참가
    }

    //1.AI생성
    private void AIPlaneCreation()
    {
        for(int i=0; i < GamePlayer.maxPlayer - GamePlayer.playerCount; i++)
        {
            //비행기 Pop

            //material 설정

            //부스터 설정

            //위치 설정   
        }

    }
    //2.Player 생성
    private void PlayerPlaneCreation()
    {

    }
}
