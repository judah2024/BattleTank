using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasManager : MonoBehaviour
{
    static public CanvasManager Instance = null;
    
    [HideInInspector]
    public UIStageInfo stageInfo;
    [HideInInspector]
    public Camera uiCamera;

    GameObject mUISuccess;
    GameObject mUIGameOver;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        
        stageInfo = GetComponentInChildren<UIStageInfo>(true);
        uiCamera = GetComponentInChildren<Camera>(true);

        // 게임 종료시 이미지 UI
        mUISuccess = transform.Find("Success").gameObject;
        mUIGameOver = transform.Find("GameOver").gameObject;
    }

    public void Restart()
    {
        mUISuccess.SetActive(false);
        mUIGameOver.SetActive(false);
    }

    /// <summary>
    /// 게임 결과에 따른 UI를 활성화 시켜준다.
    /// </summary>
    /// <param name="_isPlayerWin"> 승패결과(플레이어 기준) </param>
    public void EndGame(bool _isPlayerWin)
    {
        if (_isPlayerWin == true)
        {
            mUISuccess.SetActive(true);
        }
        else
        {
            mUIGameOver.SetActive(true);
        }
    }
}
