using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("HP")] 
    public int kMaxHp = 100;
    int mHp;

    [Header("발사 위치")]
    public Transform kShootTransform;

    [Header("총알 프리팹")] 
    public GameObject kBulletPrefab;
    public float kBulletCoolTime = 1.0f;
    float mBulletTime;
    
    Tank mTank;
    TowerHpBar mHpBar;
    
    // Start is called before the first frame update
    void Start()
    {
        mBulletTime = 0.0f;
    }

    void OnEnable()
    {
        mHp = kMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (mTank != null)
        {
            if (mTank.IsDead() == true)
            {
                return;
            }
            
            // 감지된 Target방향으로 포신 회전
            transform.forward = Vector3.MoveTowards(transform.forward, mTank.transform.position - transform.position, Time.deltaTime * 50.0f);

            mBulletTime += Time.deltaTime;
            if (mBulletTime >= kBulletCoolTime)
            {
                mBulletTime -= kBulletCoolTime;
                
                // 총알 발사
                GameObject bullet = Instantiate(kBulletPrefab);
                bullet.transform.position = kShootTransform.position;
                bullet.transform.forward = kShootTransform.forward;

                TowerMissile missile = bullet.GetComponent<TowerMissile>();
                if (missile != null)
                {
                    missile.SetTarget(mTank.transform);
                }
            }
        }
        
        // UI 위치 업데이트
        mHpBar.UpdatePosition(transform.position);
    }

    public void SetHpBar(TowerHpBar _hpBar)
    {
        mHpBar = _hpBar;
        mHpBar.Initialize(kMaxHp);
    }

    /// <summary>
    /// TowerRadar에서 Target 감지시 호출되는 함수 
    /// </summary>
    /// <param name="tank"> 감지된 Target </param>
    public void SetTargetTank(Tank tank)
    {
        mTank = tank;
    }

    /// <summary>
    /// 감지 범위 내에 Target이 없을 경우 호출되는 함수
    /// </summary>
    public void ClearTargetTank()
    {
        mTank = null;
        mBulletTime = 0.0f;
    }
    
    public void Damaged(int _value)
    {
        mHp = Mathf.Clamp(mHp - _value, 0, kMaxHp);
        mHpBar.SetHpBar(mHp);
        if (mHp == 0)
        {
            Mng.stage.DestroyedTower();
            Destroy(mHpBar.gameObject);
            Destroy(gameObject);
        }
    }
}
