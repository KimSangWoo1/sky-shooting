using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMatchManager : Singleton<DeathMatchManager>
{
    private PlaneManager PM;
    public Map map;
    public Material[] materials;
    public GameObject Player;

    private int aiCount;
    private bool aiCreation;

    //Test
    private GameObject[] AIs = new GameObject[50];
    public bool show;
    private void Awake()
    {
        GamePlayer.init();
        GamePlayer.Make_RandomName();
        aiCount = 0;
    }

    void Start()
    {
        PM = PlaneManager.Instance;

        Player.GetComponent<Renderer>().material = materials[GameManager.planeNumber];
        Player.GetComponent<PlaneBase>().profile.skinType = GameManager.planeNumber;

        //방 입장시
        PlayerCreation(); //Player생성
    }

    void Update()
    {
        if (!aiCreation)
        {
            StartCoroutine(AICreation()); //AI생성
        }


        if (show)
        {
            for(int i=0; i < AIs.Length; i++)
            {
                print(AIs[i].transform.name+" : "+AIs[i].activeSelf);
            }
            show = false;
        }
    }

    //1.AI생성
    IEnumerator AICreation()
    {
        //비행기 Pop
        GameObject cloneAI = PM.Plane_Pop(ObjectPooling.PlaneState.AI);
        //material 설정
        int randomColor = Random.Range(0, 3);
        cloneAI.GetComponent<PlaneBase>().profile.skinType = randomColor;
        cloneAI.GetComponent<Renderer>().material = materials[randomColor];
        //부스터 설정
        cloneAI.GetComponent<PlaneBase>().profile.busterType = Random.Range(0, 3);
        //위치 설정   
        cloneAI.transform.position = map.Random_Position();
        //각도 설정          

        //Object Active
        cloneAI.SetActive(true);

        //Test
        cloneAI.transform.name = aiCount + " AI";
        AIs[aiCount] = cloneAI;
        
        
        aiCount++;
        yield return new WaitUntil(() => aiCount >= 50);
        aiCreation = true;
    }

    //2.Player 생성
    private void PlayerCreation()
    {

    }


    //----

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

}
