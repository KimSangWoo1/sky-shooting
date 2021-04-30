using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Title_SceneManager : Singleton<Title_SceneManager>
{
    public enum Action_State { wait, circleAction, fireAction, revengeAction, cameraBumpAction, cameraBreakAction}
    Action_State action = Action_State.wait;
    public bool reStart;

    [Header("비행기 오브젝트")]
    // Scene 비행기 오브젝트
    public GameObject circlePlane; //circleAction
    public GameObject flowObejct; //circleAction
    public GameObject firePlane;  //fireAction
    public GameObject avoidPlane; //fireAction
    public GameObject breakPlane; //cameraBreakAction

    public Transform fire_FirePosition; //fireAction 위치 
    public Transform fire_AvoidPosition; //fireAction 위치
    public Transform revenge_FirePosition; //revengeAction 위치
    public Transform revenge_AvoidPosition; //revengeAction 위치

    [Header("UI 오브젝트")]
    //UI 오브젝트
    public Image break_Img;  //화면 깨지는 Image
    public Image fade_Img; //페이드 인 아웃 Image

    // Scene 비행기 시작 위치
    private Vector3 circlePlane_StartPosition; //circleAction
    private Vector3 flowObejct_StartPosition; //circleAction
    private Vector3 breakPlane_StartPosition; //cameraBreakAction

    //TEST
    private Quaternion fire_StartRotation;
    private Quaternion avoid_StartRotation;

    //UI 변수
    private Color fade_Color;
    private float alpha;
    [SerializeField]
    private float fadeSpeed;

    //시간변수
    private float circleAction_Time;
    private float fireAction_Time;
    private float revengeAction_Time;
    private float cameraBumpAction_Time;
    private float cameraBreakAction_Time;

    /*
    [Header("액션 진행 상태")]
    //현재 액션 
    [SerializeField]
    private bool action_Wait;
    [SerializeField]
    private bool action_Circle;
    [SerializeField]
    private bool action_Fire;
    [SerializeField]
    private bool action_Bump;
    [SerializeField]
    private bool action_Break;
    */
    [SerializeField]
    private bool action_Revenge;



    private void Awake()
    {
        circleAction_Time = 1f;
        fireAction_Time = 1f;
        revengeAction_Time = 1f;
        cameraBumpAction_Time = 1f;
        cameraBreakAction_Time = 1f;

        fadeSpeed = 0.3f;
    }
    void Start()
    {
        reStart = false;

        circlePlane.SetActive(false);
        flowObejct.SetActive(false);
        firePlane.SetActive(false);
        avoidPlane.SetActive(false);
        breakPlane.SetActive(false);

        circlePlane_StartPosition = circlePlane.transform.position;
        flowObejct_StartPosition = flowObejct.transform.position;
        breakPlane_StartPosition = breakPlane.transform.position;

        //test
        fire_StartRotation = firePlane.transform.rotation;
        avoid_StartRotation = avoidPlane.transform.rotation;

        fade_Color = fade_Img.color;

        //액션 시작
        action = Action_State.circleAction;
    }

    private void Update()
    {
        if (reStart)
        {
            retry();
        }

        switch (action)
        {
            case Action_State.wait:
                //action_Wait = true;
                break;
            case Action_State.circleAction:
                StartCoroutine(circleAction_Start());
                break;
            case Action_State.fireAction:
                StartCoroutine(fireAction_Start());
                break;
            case Action_State.revengeAction:
                StartCoroutine(revengeAction_Start());
                break;
            case Action_State.cameraBumpAction:
                StartCoroutine(bumpAction_Start());
                break;
            case Action_State.cameraBreakAction:
                StartCoroutine(breackAction_Start());
                break;
            default:
                retry();
                break;
        }
    }
    public void SetActionState(Action_State state)
    {
        action = state;
    }
    IEnumerator circleAction_Start()
    {
       // action_Wait = false;
       // action_Circle = true;
        //n초 후 시작
        yield return new WaitForSeconds(circleAction_Time);
        //오브젝트 활성화 시켜 액션ON
        circlePlane.SetActive(true);
        flowObejct.SetActive(true);
        
        action = Action_State.wait;
    }

    IEnumerator fireAction_Start()
    {
       // action_Wait = false;
       // action_Fire = true;
        //circle비행기 비활성화
        circlePlane.SetActive(false);
        flowObejct.SetActive(false);

        yield return new WaitForSeconds(fireAction_Time);
        //위치 초기화
        avoidPlane.transform.position = fire_AvoidPosition.position;
        firePlane.transform.position = fire_FirePosition.position;

        //각도 초기화   * 곱해준 이유는 더 자연스럽게 보이기 위해 +(각도 초기화하면 반원 운동이 약간 대각선으로 만 움직여서)
        firePlane.transform.rotation = fire_StartRotation;
        avoidPlane.transform.rotation = avoid_StartRotation;

        //총 발사 비행기 선택
        avoidPlane.GetComponent<Plane_AvoidMove>().fireCheck = false;
        firePlane.GetComponent<Plane_AvoidMove>().fireCheck = true;
        //속도 및 원 크기 설정
        avoidPlane.GetComponent<Plane_AvoidMove>().speed = 2f;
        avoidPlane.GetComponent<Plane_AvoidMove>().limit = 0.5f;
        firePlane.GetComponent<Plane_AvoidMove>().speed = 2f;
        firePlane.GetComponent<Plane_AvoidMove>().limit = 0.5f;
        //오브젝트 활성화
        avoidPlane.SetActive(true);
        firePlane.SetActive(true);

        action = Action_State.wait;   
    }

    IEnumerator revengeAction_Start()
    {
     //   action_Wait = false;
     //   action_Revenge = true;
        //fire 비행기 비활성화
        avoidPlane.SetActive(false);
        firePlane.SetActive(false);

        yield return new WaitForSeconds(revengeAction_Time);

        //위치 변경
        avoidPlane.transform.position = revenge_FirePosition.position;
        firePlane.transform.position = revenge_AvoidPosition.position;
        //각도 변경
        avoidPlane.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        firePlane.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //총 발사 비행기 선택
        avoidPlane.GetComponent<Plane_AvoidMove>().fireCheck = true;
        firePlane.GetComponent<Plane_AvoidMove>().fireCheck = false;
        //속도 및 원 크기 설정
        avoidPlane.GetComponent<Plane_AvoidMove>().speed = 1f;
        avoidPlane.GetComponent<Plane_AvoidMove>().limit =1f;
        firePlane.GetComponent<Plane_AvoidMove>().speed = 1f;
        firePlane.GetComponent<Plane_AvoidMove>().limit = 1f;
        //오브젝트 활성화
        avoidPlane.SetActive(true);
        firePlane.SetActive(true);

        action = Action_State.wait;
    }

    IEnumerator bumpAction_Start()
    {
    //    action_Wait = false;
     //   action_Bump = true;
        //revenge 비행기 비활성화
        avoidPlane.SetActive(false);
        firePlane.SetActive(false);

        yield return new WaitForSeconds(cameraBumpAction_Time);
        //break 비행기 활성화
        breakPlane.SetActive(true);

        action = Action_State.wait;
    }
    IEnumerator breackAction_Start()
    {
    //    action_Break = true;
        //break 비행기 활성화
        breakPlane.SetActive(false);

        //UI 활성화
        break_Img.gameObject.SetActive(true);
        fade_Img.gameObject.SetActive(true);
 
        //알파 값 조절
        alpha += Time.deltaTime * fadeSpeed;
        alpha = Mathf.Clamp(alpha, 0f, 1.5f);
        if (alpha >= 1f)
        {
            yield return new WaitForSeconds(cameraBreakAction_Time);
            retry();
        }
        else
        {
            fade_Img.color = new Color(fade_Img.color.r, fade_Img.color.g, fade_Img.color.b, alpha);
        }
    }

    private void retry()
    {
        action = Action_State.wait; //상태 대기
        StopAllCoroutines(); //진행중인 코루틴 종료
        reStart = false; 

        //비행기 끄기
        circlePlane.SetActive(false);
        flowObejct.SetActive(false);
        firePlane.SetActive(false);
        avoidPlane.SetActive(false);
        breakPlane.SetActive(false);
        //UI 끄기
        break_Img.gameObject.SetActive(false);
        fade_Img.gameObject.SetActive(false);

        //UI 칼라 초기화
        fade_Img.color = fade_Color;

        //위치 초기화
        circlePlane.transform.position = circlePlane_StartPosition;
        flowObejct.transform.position = flowObejct_StartPosition;
        firePlane.transform.position = fire_FirePosition.position;
        avoidPlane.transform.position = fire_AvoidPosition.position;
        breakPlane.transform.position = breakPlane_StartPosition;

        //각도 초기화
        firePlane.transform.rotation = fire_StartRotation;
        avoidPlane.transform.rotation = avoid_StartRotation;

        alpha = 0f;

        //액션 초기화
     //   action_Wait = false;
     //   action_Circle = false;
     //   action_Fire = false;
        action_Revenge = false;
     //    action_Bump = false;
     //   action_Break = false;
        
        //Circle_Action시작
        action = Action_State.circleAction;
    }
    public bool Check_Revenge()
    {
        return action_Revenge;
    }
}
