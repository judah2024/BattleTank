using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerRadar : MonoBehaviour
{
    public Tower kTower;
    
    [Header("감지 조절 데이터")]
    public float kDetectionRadius = 10.0f;
    public float kDetectionCoolTime = 0.5f;
    public LayerMask kTargetLayer;

    private Collider[] mArrTargetCollider = new Collider[4];

    private void Awake()
    {
        StartCoroutine(DetectCoroutine());
    }

    /// <summary>
    /// 주기적으로 감지를 반복하는 코루틴
    /// </summary>
    IEnumerator DetectCoroutine()
    {
        // 불필요한 객체 생성을 방지
        WaitForSeconds wait = new WaitForSeconds(kDetectionCoolTime);
        while (true)
        {
            CheckForTarget();
            yield return wait;
        }
    }

    /// <summary>
    /// Target이 있는지 확인하고 결과를 따른 Tower 함수를 실행한다.
    /// </summary>
    void CheckForTarget()
    {
        if (TryFindTarget() == false)
        {
            // 감지된 타겟이 없다면 저장된 타겟을 지운다.
            kTower.ClearTargetTank();
        }
    }

    /// <summary>
    /// 감지 범위 내에 타겟이 있는지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    bool TryFindTarget()
    {
        // NonAlloc 함수로 재할당 방지
        int num = Physics.OverlapSphereNonAlloc(transform.position, kDetectionRadius, mArrTargetCollider, kTargetLayer);

        for (int i = 0; i < num; i++)
        {
            // 먼저 발견된 타겟을 전달한다.
            if (mArrTargetCollider[i].TryGetComponent(out Tank tank))
            {
                kTower.SetTargetTank(tank);
                return true;
            }
        }
        return false;
     }

    
    void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            DrawDetectionRange();
        }
    }
    
    void OnDrawGizmosSelected()
    {
        DrawDetectionRange();
    }

    /// <summary>
    /// Editor 에서 탐지 범위를 시각화 하기위해 Gizmos를 그리는 함수
    /// </summary>
    void DrawDetectionRange()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, kDetectionRadius);
    }
}
