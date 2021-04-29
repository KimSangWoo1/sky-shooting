using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink_Text: MonoBehaviour
{
    private Text titleText;
    private float alpha;
    private Color blankColor;
    
    void Start()
    {
        titleText = GetComponent<Text>();    
    }

    void Update()
    {
        alpha = Mathf.PingPong(Time.time, 1);
        Color color = titleText.color;
        blankColor = new Color(color.r, color.g, color.b, alpha);
        titleText.color = blankColor;
    }
}
