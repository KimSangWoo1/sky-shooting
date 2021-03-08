using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    [SerializeField]
    private GameVersion gameVersion;

    public Text versionText;
    public Text startText;

    void Start()
    {
        if (gameVersion == null)
        {
            gameVersion = Resources.Load("Asset/GameVersion") as GameVersion;
        }

        versionText.text = gameVersion.get_Version();
        startText.text = gameVersion.get_StartGameText();
    }
}
