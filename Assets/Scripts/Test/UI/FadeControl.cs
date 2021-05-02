using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour
{
    public Image fadeImg;
    public float fadeSpeed;

    public bool fadeOut;
    public bool fadeOut2;

    public bool check;
    public bool reset;
    private Color fadeColor = Color.black;
    private float fadeTime;

    void Start()
    {
        print("Start1");
        StartCoroutine(FadeOut3());
        print("Start2");
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut());
        }else if (fadeOut2)
        {
            StartCoroutine(FadeOut2());
        }
        if (check)
        {
            fadeTime += Time.deltaTime / fadeSpeed; // 페이드 시간
        }else if (reset)
        {
            fadeTime = 0f;
        }   
    }

    //페이드 아웃
    IEnumerator FadeOut()
    {
        fadeImg.gameObject.SetActive(true); //페이드 ON
        fadeImg.color = Color.black;
        
        fadeTime += Time.deltaTime / fadeSpeed; // 페이드 시간
        fadeColor.a = 1f - fadeTime; //페이드 Alpha
        fadeImg.color = fadeColor; //페이드 Color 적용

        print("페이드 아웃중" + fadeTime);

        yield return new WaitWhile(() => fadeTime < 1f);

        print("페이드 아웃 종료" + fadeTime);
        fadeOut = false;
        fadeTime = 0f;
        fadeColor = Color.black;
        fadeImg.color = fadeColor;
        fadeImg.gameObject.SetActive(false);
    }

    //페이드 아웃
    IEnumerator FadeOut2()
    {
        fadeImg.gameObject.SetActive(true); //페이드 ON
        fadeImg.color = Color.black;

        fadeTime += Time.deltaTime / fadeSpeed; // 페이드 시간
        fadeColor.a = 1f - fadeTime; //페이드 Alpha
        fadeImg.color = fadeColor; //페이드 Color 적용
        print("페이드 아웃중" + fadeTime);
        yield return new WaitUntil(() => fadeTime > 1f);
         
        print("페이드 아웃 종료" + fadeTime);
        fadeOut2 = false;
        fadeTime = 0f;
        fadeColor = Color.black;
        fadeImg.color = fadeColor;
        fadeImg.gameObject.SetActive(false);
    }

    IEnumerator FadeOut3()
    {
        fadeImg.gameObject.SetActive(true); //페이드 ON
        fadeImg.color = Color.black;

        while (fadeTime < 1f)
        {
            fadeTime += Time.deltaTime / fadeSpeed; // 페이드 시간
            fadeColor.a = 1f - fadeTime; //페이드 Alpha
            fadeImg.color = fadeColor; //페이드 Color 적용
            print("페이드 아웃중" + fadeTime);
        }
        yield return new WaitUntil(() => fadeTime > 1f);

        print("페이드 아웃 종료" + fadeTime);
        fadeOut2 = false;
        fadeTime = 0f;
        fadeColor = Color.black;
        fadeImg.color = fadeColor;
        fadeImg.gameObject.SetActive(false);
    }


    IEnumerator FadeOut4()
    {
        fadeImg.gameObject.SetActive(true); //페이드 ON
        fadeImg.color = Color.black;

        while (true)
        {
            fadeTime += Time.deltaTime / fadeSpeed; // 페이드 시간
            fadeColor.a = 1f - fadeTime; //페이드 Alpha
            fadeImg.color = fadeColor; //페이드 Color 적용
            print("페이드 아웃중" + fadeTime);
            yield return new WaitUntil(() => fadeTime > 1f);
        }
      

        print("페이드 아웃 종료" + fadeTime);
        fadeOut2 = false;
        fadeTime = 0f;
        fadeColor = Color.black;
        fadeImg.color = fadeColor;
        fadeImg.gameObject.SetActive(false);
    }
    /*
    //페이드 인
    IEnumerator FadeIn(string SceneName)
    {
        print("페이드 인");
        if (!isFading)
        {
            isFading = true;
            fadeImg.gameObject.SetActive(true);

            while (fadeTime < 1f)
            {
                fadeTime += Time.deltaTime;
                fadeColor.a = Mathf.Clamp01(fadeTime * 0.1f);
                fadeImg.color = fadeColor;

            }

            yield return new WaitUntil(() => fadeTime >= 1f);

            AsyncOperation async = SceneManager.LoadSceneAsync(SceneName);
            yield return new WaitUntil(() => async.isDone == true);

            print("페이드 인 종료");
            fadeImg.gameObject.SetActive(false);
            audioSource.Stop();

            //Fade Out으로 초기화
            fadeTime = 0f;
            fadeImg.color = Color.black;
            StartCoroutine(FadeOut());
        }
    }
    */

}
