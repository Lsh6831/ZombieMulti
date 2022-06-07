    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;// 체력을 표시할 UI슬라이더

    //아래 3가지는 스크랩터블 오브젝트로 처리함(하면 좋음)
    public AudioClip dethClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트
    

    private void Awake()
     //사용할 컴포넌트 가져오기
    {   
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();

    }

    protected override void OnEnable()
     // override = 재정의
     // LivingEntity의 OnEnable() 실행(상태초기화)
    {
        base.OnEnable();
        //base 부모 지칭

        // 체력 슬라이더 활성화 
        healthSlider.gameObject.SetActive(true);
        // 체력 슬라이더의 최댓값을 기본 체력값으로 변경
        healthSlider.maxValue=startinghealth;
        // 체력 슬라이더의 값을 현재 체력값으로 변경
        healthSlider.value=health;

        //플레이어 조작을 받는 컴포넌트 활성화
        playerMovement.enabled=true;
        playerShooter.enabled=true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    // LivingEntity 의 RestoreHealth() 실행(체력 증가)
    { 
        base.RestoreHealth(newHealth);
        //갱신된 체력으로 체력 슬라이더 갱신
        healthSlider.value=health;
        // SlowHealth.slowHealth(newHealth); 삭제 
    }
    // 대미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
   
    {
        if(!dead)
        {
            //사망하지 않은 경우에만 효과음 재생
            playerAudioPlayer.PlayOneShot(hitClip);
        }
         // Living Entity의 OnDamage() 실행(대미지 적용)
        base.OnDamage(damage, hitPoint, hitNormal);
        // 갱신된 체력을 체력 슬라이더에 반영
        healthSlider.value=health;
    }
    //사망 처리
    public override void Die()
    
    {
        //LivingEntity의 Die() 실행(사망적용)
        base.Die();

        // 체력 슬라이더 비활성화
        healthSlider.gameObject.SetActive(false);


        // 사망음 재생
        playerAudioPlayer.PlayOneShot(dethClip);
        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생
        playerAnimator.SetTrigger("Die");

        // 플레이어 조작을 받는 컴포넌트 비활성화
        playerMovement.enabled=false;
        playerShooter.enabled=false;
    }
    private void OnTriggerEnter(Collider other)
    //콜리전 개열은 콜리전 으로 받음 (둘의 차이는?)
    // 아이템과의 충돌한 경우 해당 아이템을 사용하는 처리
    {
        if(!dead)
        {
            //충돌한 상대방으로부터 IItem 컴포넌트 가져오기 시도
            IItem item = other.GetComponent<IItem>();

            //충돌한 상대방으로부터 IItem 컴포넌트 가져오는 데 성공했다면
            if(item!=null)
            {
                //Use 메서드를 실행하여 아이템 사용
                item.Use(gameObject);
                // 아이템 습득 소리 재생
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
    
}
