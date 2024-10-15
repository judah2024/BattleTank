using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text mText;

    public void OnClickScreen()
    {
        Mng.stage.StartGame();
        gameObject.SetActive(false);
    }
}
