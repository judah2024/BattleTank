using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerHpBar : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text kHpText;

    Tower mTower;
    Slider mSlider;
    RectTransform mRectTransform;
    void Awake()
    {
        mSlider = GetComponent<Slider>();
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(int _maxHp)
    {
        gameObject.SetActive(false);
        mSlider.maxValue = _maxHp;
        mSlider.value = _maxHp;
        kHpText.text = $"{_maxHp} / {_maxHp}";
    }

    public void SetHpBar(int _hp)
    {
        gameObject.SetActive(true);
        kHpText.text = $"{_hp} / {mSlider.maxValue}";
        mSlider.value = _hp;
    }

    public void UpdatePosition(Vector3 _towerPosition)
    {
        // 카메라 상에서 오브젝트의 위를 향하도록 위치를 변경한다.
        Vector3 point = Camera.main.WorldToScreenPoint(_towerPosition + Vector3.up * 2.5f);
        mRectTransform.position = CanvasManager.Instance.uiCamera.ScreenToWorldPoint(point);
    }
}
