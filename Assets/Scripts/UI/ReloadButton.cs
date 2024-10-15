using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    [Header("Player")]
    public Tank kTank;

    [Header("재장전 시간")]
    public float kReloadDuration = 2.0f;
    float mReloadTime = 0.0f;

    bool mIsReloading = false;

    [Header("UI")]
    public TMP_Text kReloadText;
    public Image kReloadImage;
    
    void Start()
    {
        Reload();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R) == true)
        {
            Reloading();
        }

        if (mIsReloading == true)
        {
            mReloadTime += Time.deltaTime;

            if (mReloadTime >= kReloadDuration)
            {
                Reload();
                return;
            }

            // 재장전 시간동안 UI가 1바퀴 회전한다
            float angle = (mReloadTime / kReloadDuration) * 360.0f;
            kReloadImage.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
        
    }

    void Reload()
    {
        mReloadTime = 0.0f;
        mIsReloading = false;
        kReloadImage.transform.localRotation = Quaternion.Euler(Vector3.zero);
        kTank.FullMissile();
    }

    void Reloading()
    {
        if (mIsReloading == true)
        {
            return;
        }

        if (kTank.IsFullMissile() == true)
        {
            return;
        }

        mIsReloading = true;
    }

    public void OnReloadButtonClick()
    {
        Reloading();
    }

    public void SetMissileInfo(int curCount, int totalCount)
    {
        kReloadText.text = $"{curCount} / {totalCount}";
    }
    
    public bool IsReloading()
    {
        return mIsReloading == true;
    }
}
