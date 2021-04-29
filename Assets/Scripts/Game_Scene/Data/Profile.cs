using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Profile 
{
    public string name;
    public int score = 0;
    public int dollar = 0;

    public void UpdateProfile(string _name, int _score, int _dollar)
    {
        name = _name;
        score = _score;
        dollar = _dollar;
    }
    public void UpdateName(string _name)
    {
        name = _name;
    }
    public void UpdateScore(int _score)
    {
        score = _score;
    }

    public void UpdateDollar(int _dollar)
    {
        dollar = _dollar;
    }
}
