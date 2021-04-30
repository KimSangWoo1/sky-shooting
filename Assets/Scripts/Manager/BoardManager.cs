using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BoardManager : Singleton<BoardManager>
{
    public bool update;

    public Text[] playerNames;
    public Text[] scores;

    private int maxShowNum =5;
    private void Awake()
    {

    }
    void Start()
    {
        Show_PlayerList();
    }

    void Update()
    {
        if (update)
        {
            Show_PlayerList();
            update = false;
        }
    }
    
    public void Show_PlayerList()
    {
        //Linq
        var aliveList = from player in GamePlayer.playList
                   orderby player.score descending
                   select player;

        int i = 0;
        foreach(Profile profile in aliveList)
        {
            if (i < maxShowNum)
            {
                playerNames[i].text = profile.name;
                scores[i].text = profile.score.ToString();
                i++;
            }
        }

        if (i < maxShowNum)
        {
            //Linq
            var deadList = from player in GamePlayer.deadPlayList
                       orderby player.score descending
                       select player;

            foreach(Profile profile in deadList)
            {
                if (i < maxShowNum)
                {
                    playerNames[i].text = profile.name;
                    playerNames[i].color = Color.red;
                    scores[i].text = profile.score.ToString();
                    scores[i].color = Color.red;
                    i++;
                }
            }
        }
    }

    public void Add_Score(string _name, int _score)
    {
        foreach (Profile profile in GamePlayer.playList)
        {
            if (profile.name.Equals(_name))
            {
                profile.score += _score;
                break;
            }
        }
        update = true;
    }

    public int Get_Score(string _name)
    {
        int score = 0;
        foreach (Profile profile in GamePlayer.playList)
        {
            if (profile.name.Equals(_name))
            {
                score = profile.score;
                break;
            }
        }
        return score;
    }

    //죽은 플레이어와 살아있는 플레이어를 나눈다.
    public void Reset_Score(string _name)
    {
        //GamePlayer.playList.Find(x => x.name == _name);
        foreach (Profile profile in GamePlayer.playList)
        {
            if(profile.name.Equals(_name))
            {
                GamePlayer.deadPlayList.Add(profile);
                GamePlayer.playList.Remove(profile);
                break;
            }
        }
        update = true;
    }
}
