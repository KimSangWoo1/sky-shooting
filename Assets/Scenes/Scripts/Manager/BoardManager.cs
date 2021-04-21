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
        GamePlayer.init();
        GamePlayer.Make_RandomName();
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
        var list = from player in GamePlayer.playList
                   orderby player.score descending
                   select player;

        int i = 0;
        foreach(Profile profile in list)
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
            for(int j=i; j < playerNames.Length; j++)
            {
                playerNames[i].text = "";
                scores[i].text = "";
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
            }
        }
        update = true;
    }

    public Profile Get_Grade(string _name)
    {

        foreach (Profile profile in GamePlayer.playList)
        {
            if (profile.name.Equals(_name))
            {
                return profile;
            }
        }
        return null;
    }

    public void Reset_Score(string _name)
    {
        foreach(Profile profile in GamePlayer.playList)
        {
            if(profile.name.Equals(_name))
            {
                GamePlayer.playList.Remove(profile);
                break;
            }
        }
        update = true;
    }
}
