using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbySceneControl : MonoBehaviour
{
    LoadingManager LM;

    private void Start()
    {
        LM = LoadingManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Load_DeathMatchScene()
    {
        StartCoroutine(LoadingScene());
        LM.StartLoading();
    }

    IEnumerator LoadingScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DeathMatch_Scene");
        LM.Get_Operation(asyncOperation.progress);
        if (asyncOperation.isDone)
        {
            LM.EndLoading();
        }
        yield return new WaitWhile(() => !asyncOperation.isDone);
    }
}
