using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TankHpBar : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text kHpText;
    
    Slider mSlider;

    void Awake()
    {
        mSlider = GetComponent<Slider>();
    }

    public void Initialize(int _maxHp)
    {
        mSlider.maxValue = _maxHp;
        mSlider.value = _maxHp;
        kHpText.text = $"{_maxHp} / {_maxHp}";
    }

    public void SetHpBar(int _hp)
    {
        kHpText.text = $"{_hp} / {mSlider.maxValue}";
        mSlider.value = _hp;
    }
}
