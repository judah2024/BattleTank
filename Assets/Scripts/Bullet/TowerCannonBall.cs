using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCannonBall : MonoBehaviour
{
    [Header("Projectile")]
    public float kMoveSpeed = 50.0f;
    public int kDamageValue = 5;
    public float kLifespan = 3.0f;
    
    [Header("사운드 클립")]
    public AudioClip[] kArrHitClip;
    public AudioClip kBlastClip;
    AudioSource mAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.clip = kBlastClip;
        mAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (kLifespan <= 0.0f)
        {
            Destroy(gameObject);
            return;
        }
        
        // 이전 프레임 사이에서 충돌이 발생했는지 확인
        Vector3 newPosition = transform.position + transform.forward * kMoveSpeed * Time.deltaTime;
        if (IsHit(newPosition, out RaycastHit hit) == true)
        {
            OnHit(hit);
        }

        kLifespan -= Time.deltaTime;
        transform.position = newPosition;
    }
    
    /// <summary>
    /// 프레임 사이에 충돌이 일어났는지 확인하는 함수
    /// </summary>
    /// <param name="_dest"> 목적지 </param>
    /// <param name="hit"> out으로 반환될 RaycastHit </param>
    /// <returns> Raycast가 성공하면 true를 반환 </returns>
    bool IsHit(Vector3 _dest, out RaycastHit hit)
    {
        float distance = Vector3.Distance(transform.position, _dest);
        return Physics.Raycast(transform.position, transform.forward, out hit, distance);
    }
    
    void OnHit(RaycastHit hit)
    {
        Debug.Log(hit.transform.gameObject);
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어 공격
            Tank tank = hit.transform.gameObject.GetComponent<Tank>();
            tank.Damaged(kDamageValue);
        }
                
        // 이펙트 생성
        ParticleSystem loadParticle = Resources.Load<ParticleSystem>("Particle/SmallExplosionEffect");
        ParticleSystem explosionEffect = Instantiate(loadParticle);
        explosionEffect.transform.position = transform.position;
        
        // 사운드 재생
        int randomIdx = Random.Range(0, kArrHitClip.Length);
        AudioSource.PlayClipAtPoint(kArrHitClip[randomIdx], transform.position, 0.5f);
        
        Destroy(gameObject);
    }
}
