using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStageInfo : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text kStageText;
    public TMP_Text kTimeText;

    public void UpdateTime(float _seconds)
    {
        // TimeSpan을 사용하여 가독성 좋게 분:초를 출력한다.
        TimeSpan timeSpan = TimeSpan.FromSeconds(_seconds);
        kTimeText.text = timeSpan.ToString(@"mm\:ss\.ff");
    }

    public void UpdateStageInfo(int _stageIndex)
    {
        kStageText.text = $"Stage{_stageIndex + 1}";
    }
}
