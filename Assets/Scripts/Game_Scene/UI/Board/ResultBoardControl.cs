using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultBoardControl : MonoBehaviour
{
    public Image board;
    public Text playerNameText;
    public Text scoreText;
    public Text dollarText;

    public void Set_ResultBoard(Profile profile)
    {

        board.gameObject.SetActive(true);

        playerNameText.text = profile.name;
        scoreText.text = profile.score.ToString();
        dollarText.text = profile.dollar.ToString();
    }

    public void Load_LobbyScene()
    {
        SceneManager.LoadScene("Lobby_Scene");
    }

    public void Load_DeathMatchScene()
    {
        SceneManager.LoadScene("DeathMatch_Scene");
    }
}
