using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("타이틀")]
    public GameObject kTitleButton;
    
    [Header("플레이어")]
    public Tank kTank;

    [Header("타워 HpBar")] 
    public TowerHpBar kHpBar;

    [Header("스테이지 프리팹")]
    public GameObject[] kArrStagePrefab;
    Stage mCurStage;

    bool mIsPlaying;
    int mRemainingTower;
    float mLimitTime;
    float mCurPlayTime;

    void Awake()
    {
        Instance = this;
        mCurStage = Instantiate(kArrStagePrefab[0]).GetComponent<Stage>();
        mCurStage.transform.position = Vector3.zero;
        kTitleButton.SetActive(true);
    }

    void Start()
    {
        // 종속된 클래스들의 Awake를 기다려야 하므로 Start에서 호출
        StartStage(0);
    }

    void Update()
    {
        if (mCurPlayTime >= mLimitTime)
        {
            // 제한 시간이 지난다면 게임 오버
            GameOver();
        }
        
        mCurPlayTime += Time.deltaTime;
        Mng.canvas.stageInfo.UpdateTime(mCurPlayTime);
    }

    /// <summary>
    /// 다음 스테이지를 부르는 함수, 마지막 스테이지라면 게임 클리어!
    /// </summary>
    void GoNextStage()
    {
        int stageIndex = mCurStage.kIndex + 1;
        if (stageIndex == kArrStagePrefab.Length)
        {
            Success();
        }
        else
        {
            StartStage(stageIndex);
        }
    }

    /// <summary>
    /// 스테이지를 시작하는 함수
    /// </summary>
    /// <param name="stageNumber"> 시작될 스테이지 인덱스 </param>
    void StartStage(int stageNumber)
    {
        Stage oldStage = mCurStage;
        
        // 다음 스테이지 생성
        mCurStage = Instantiate(kArrStagePrefab[stageNumber]).GetComponent<Stage>();
        mCurStage.transform.position = Vector3.zero;
        CanvasManager.Instance.stageInfo.UpdateStageInfo(mCurStage.kIndex);
        
        // 새 Stage에 타워들의 HpBar 생성
        CreateTowerHpBar();
        
        mLimitTime = mCurStage.kLimitTimeSecond;
        mCurPlayTime = 0.0f;
        
        // 플레이어 초기화
        kTank.gameObject.SetActive(false);
        kTank.gameObject.SetActive(true);
        
        // 이전 스테이지 제거
        Destroy(oldStage.gameObject);
    }

    /// <summary>
    /// 스테이지 내의 Tower에 HpBar를 생성한다.
    /// </summary>
    void CreateTowerHpBar()
    {
        Tower[] towers = mCurStage.GetComponentsInChildren<Tower>(true);
        foreach (Tower tower in towers)
        {
            TowerHpBar hpBar = Instantiate<TowerHpBar>(kHpBar);
            tower.SetHpBar(hpBar);
            tower.gameObject.SetActive(true);

            // Canvas/Tower 에 HPBar를 모아서 관리한다.
            hpBar.transform.parent = CanvasManager.Instance.transform.Find("Tower");
            hpBar.transform.localScale = Vector3.one;
        }

        mRemainingTower = towers.Length;
    }

    public bool IsPlaying()
    {
        return mIsPlaying;
    }

    public void StartGame()
    {
        mIsPlaying = true;
    }

    public void DestroyedTower()
    {
        // 타워가 모두 파괴되면 다음 스테이지 시작
        mRemainingTower--;
        if (mRemainingTower <= 0)
        {
            GoNextStage();
        }
    }

    public void GameOver()
    {
        CanvasManager.Instance.EndGame(false);
    }

    public void Success()
    {
        CanvasManager.Instance.EndGame(true);
    }

    public void Restart()
    {
        CanvasManager.Instance.Restart();
        StartStage(0);
    }
}
