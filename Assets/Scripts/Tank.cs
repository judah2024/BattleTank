using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tank : MonoBehaviour
{
    [Header("HP")] 
    public int kMaxHp = 100;
    int mHp;
    
    [Header("이동 속도")]
    public float kMoveSpeed = 20.0f;
    public float kRotateSpeed = 10.0f;

    [Header("Pivot 회전값")] 
    public Transform kTowerTransform;
    public Transform kShootTransform;

   [Header("미사일 Prefab")] 
    public GameObject kMissileGo;

    [Header("최대 미사일 수")] 
    public int kTotalMissileCount = 10;
    int mCurMissileCount;

    [Header("UI")] 
    public TankHpBar kHpBar;
    public ReloadButton kReloadButton;

    Vector3 mOldPosition;
    Animator mAnimator;
    
    void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // 다음 스테이지 이동시 위치와 상태 초기화
        transform.position = Vector3.zero;
        
        mOldPosition = transform.position;
        mCurMissileCount = kTotalMissileCount;

        mHp = kMaxHp;
        kHpBar.Initialize(kMaxHp);
    }

    void Update()
    {
        if (Mng.stage.IsPlaying() == true)
        {
            MoveAndBodyRotate();
            PivotAim();
            FireMissile();
        }
    }

    /// <summary>
    /// 위치이동, 방향전환시 부드럽게 회전한다.
    /// </summary>
    void MoveAndBodyRotate()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W) == true)
        {
            dir += Vector3.forward;
        }
        
        if (Input.GetKey(KeyCode.S) == true)
        {
            dir += Vector3.back;
        }
        
        if (Input.GetKey(KeyCode.D) == true)
        {
            dir += Vector3.right;
        }
        
        if (Input.GetKey(KeyCode.A) == true)
        {
            dir += Vector3.left;
        }

        // 이동
        Vector3 newPosition = transform.position + dir.normalized * kMoveSpeed * Time.deltaTime;
        transform.position = newPosition;

        // 방향전환
        Vector3 tankForward = transform.position - mOldPosition;
        if (tankForward.magnitude >= 0.01f)
        {
            float signedAngleY = Vector3.SignedAngle(Vector3.forward, tankForward, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, signedAngleY, 0.0f), Time.deltaTime * kRotateSpeed);
            mOldPosition = transform.position;
        }
    }
    
    /// <summary>
    /// 마우스의 방향으로 포신을 회전한다.
    /// </summary>
    void PivotAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) == true)
        {
            Vector3 targetVector = hit.point - transform.position;
            targetVector.y = 0.0f;
            
            kTowerTransform.forward = targetVector;
        }
    }

    /// <summary>
    /// 공격 함수, 왼쪽클릭으로 실행된다.
    /// </summary>
    void FireMissile()
    {
        if (Input.GetMouseButtonUp(0) == true)
        {
            // 탄환이 없으면 발사 불가능
            if (mCurMissileCount == 0)
                return;

            // 재장전 중에는 발사 불가능
            if (kReloadButton.IsReloading() == true)
                return;

            // UI 업데이트
            kReloadButton.SetMissileInfo(mCurMissileCount, kTotalMissileCount);
            
            // 미사일 생성
            GameObject missileGO = Instantiate(kMissileGo);
            missileGO.transform.position = kShootTransform.position;
            missileGO.transform.forward = kTowerTransform.forward;
            
            // 발사 애니메이션 재생
            mAnimator.Play("TankShoot", 0, 0.0f);
            mCurMissileCount--;
        }
    }

    public void FullMissile()
    {
        mCurMissileCount = kTotalMissileCount;
        kReloadButton.SetMissileInfo(mCurMissileCount, kTotalMissileCount);
    }

    public bool IsFullMissile()
    {
        return mCurMissileCount == kTotalMissileCount;
    }

    public void Damaged(int _value)
    {
        mHp = Mathf.Clamp(mHp - _value, 0, kMaxHp);
        kHpBar.SetHpBar(mHp);

        if (mHp == 0)
        {
            Mng.stage.GameOver();
            Destroy(gameObject);
        }
    }
    
    public bool IsDead() => mHp == 0;
}
