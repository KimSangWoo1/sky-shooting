using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneControl : MonoBehaviour
{
    //Awake -> OnEnable -> SceneManager.sceneLoaded -> Start의 순이다
    private void Awake()
    {
        print("AWake");
    }

    private void OnEnable()
    {
        print("OnEnable");
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene _scene, LoadSceneMode mode)
    {

        print("SceneLoad : "+_scene.name +" : "+mode);

    }

    private void Start()
    {
        print("Start");
    }

    public void Load_LobbyScene()
    {
        SceneManager.LoadScene("Lobby_Scene");
    }
    private void OnDisable()
    {
        print("OnDisable");
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
}
