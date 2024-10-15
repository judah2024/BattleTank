using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("추격 대상")]
    public Transform kFollowTarget;

    [Header("추격 강도")]
    public float kFollowTensor = 10f;

    Vector3 mTargetDistance;
    void Start()
    {
        // 초기거리 지정
        mTargetDistance = transform.position - kFollowTarget.position;
    }

    void Update()
    {
        if (kFollowTarget != null)
        {
            // 부드러운 이동을 위해 Lerp를 사용한다.
            transform.position = Vector3.Lerp(transform.position, kFollowTarget.position + mTargetDistance, kFollowTensor * Time.deltaTime);
        }
    }
}
