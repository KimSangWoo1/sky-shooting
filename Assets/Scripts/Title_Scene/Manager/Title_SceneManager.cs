using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_SceneManager : Singleton<Title_SceneManager>
{
    public enum Action_State { wait, circleAction, fireAction, revengeAction, cameraBreakAction}
    Action_State action = Action_State.wait;
    public bool reStart;

    // Scene 비행기 오브젝트
    public GameObject circlePlane; //circleAction
    public GameObject flowObejct; //circleAction
    public GameObject firePlane;  //fireAction
    public GameObject avoidPlane; //fireAction
    public Transform revengeFire; //revengeAction
    public Transform revengeAvoid; //revengeAction
    public GameObject breakPlane; //cameraBreakAction

    // Scene 비행기 시작 위치
    private Vector3 circlePlane_StartPosition; //circleAction
    private Vector3 flowObejct_StartPosition; //circleAction
    private Vector3 firePlane_StartPosition; //fireAction
    private Vector3 avoidPlane_StartPosition; //fireAction
    //private Vector3 firePlane_StartPosition2; //revengeAction
    //private Vector3 avoidPlane_StartPosition2; //revengeAction
    private Vector3 breakPlane_StartPosition; //cameraBreakAction

    //시간변수
    private float circleAction_Time;
    private float fireAction_Time;
    private float revengeAction_Time;
    private float cameraBreakAction_Time;

    private float nextAction_WaitTime;

    private void Awake()
    {
        circleAction_Time = 1f;
        fireAction_Time = 1f;
        revengeAction_Time = 1.5f;
    }
    void Start()
    {
        reStart = false;

        circlePlane.SetActive(false);
        flowObejct.SetActive(false);
        firePlane.SetActive(false);
        avoidPlane.SetActive(false);

        circlePlane_StartPosition = circlePlane.transform.position;
        flowObejct_StartPosition = flowObejct.transform.position;
        firePlane_StartPosition = firePlane.transform.position;
        avoidPlane_StartPosition = avoidPlane.transform.position;

        //액션 시작
        action = Action_State.revengeAction;
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
            case Action_State.cameraBreakAction:
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
        //n초 후 시작
        yield return new WaitForSeconds(circleAction_Time);
        //오브젝트 활성화 시켜 액션ON
        circlePlane.SetActive(true);
        flowObejct.SetActive(true);

        action = Action_State.wait;
    }

    IEnumerator fireAction_Start()
    {
        circlePlane.SetActive(false);
        flowObejct.SetActive(false);

        yield return new WaitForSeconds(fireAction_Time);
        avoidPlane.SetActive(true);
        firePlane.SetActive(true);

        action = Action_State.wait;   
    }

    IEnumerator revengeAction_Start()
    {
        avoidPlane.SetActive(false);
        firePlane.SetActive(false);
        avoidPlane.transform.position = revengeFire.position;
        firePlane.transform.position = revengeAvoid.position;

        avoidPlane.transform.rotation = Quaternion.Euler(-90f, 180f, 180f);
        firePlane.transform.rotation = Quaternion.Euler(-90f, 180f, 180f);

        yield return new WaitForSeconds(revengeAction_Time);
        avoidPlane.SetActive(true);
        firePlane.SetActive(true);
        action = Action_State.wait;
    }

    private void retry()
    {
        action = Action_State.wait;
        StopAllCoroutines();

        reStart = false;

        circlePlane.SetActive(false);
        flowObejct.SetActive(false);

        firePlane.SetActive(false);
        avoidPlane.SetActive(false);

        circlePlane.transform.position = circlePlane_StartPosition;
        flowObejct.transform.position = flowObejct_StartPosition;
        firePlane.transform.position = firePlane_StartPosition;
        avoidPlane.transform.position = avoidPlane_StartPosition;

        nextAction_WaitTime = 0f;

        action = Action_State.circleAction;
    }
}
