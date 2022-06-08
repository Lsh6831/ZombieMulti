using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;


// 생명체로 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 대미지 받아들이기 , 사망 가능 , 사망 이벤트를 제공


public class LivingEntity : MonoBehaviourPun,IDamageable
{
    public float startinghealth = 100f; //시작 체력
    public float health{get;protected set;} // 현재 체력
    // protected = 자식만 수정할수 있음
    public bool dead{get;protected set;} // 사망 상태
    public event Action onDeath; // 사망 시 발동할 이벤트
    // event - 구독
    // 생명체가 활성화될 때 상태를 리셋


    // 호스트 ->모든 클라이언트 방향으로 체력과 사망 상태를 동기화하는 메서드
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth,bool newDead)
    {
        health = newHealth;
        dead = newDead;
    }
    protected virtual void OnEnable() 
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 최기화
        health = startinghealth;
    }

    [PunRPC]
    // 대미지를 입는 기능
    public virtual void OnDamage(float damage,Vector3 hitPoint,Vector3 hitNormal)
    {   //대미지만큼 체력 감소
        health-=damage;
        // 체력이 0이하&&아직죽지 않았다면 사망 처리 실행

        // 호스트에서 클라이언트로 동기화
        photonView.RPC("ApplyUpdatedHealth",RpcTarget.Others,health,dead);

        // 다른 클라이언트도 OnDamage를 실행하도록 함
        photonView.RPC("OnDamage",RpcTarget.Others,damage,hitPoint,hitNormal);

        // 체력이 0이하&&아직 죽지 않았다면 사망 처리 실행
        if(health<=0&&!dead)
        {
            Die();
        }
    }
    
    // 체력을 회복하는 기능
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
        {// 이미 사망한 경우 체력을 회복할수 없음
            return;
        }
        // 호스트만 체력을 직접 갱신 가능
        if(PhotonNetwork.IsMasterClient)
        {
        //체력 추가
        health+=newHealth;
        // 서버에 클라이언트 동기화
        photonView.RPC("ApplyUpdatedHealth",RpcTarget.Others,health,dead);

        // 다른 클라이언트도 RestoreHealth를 실행하도록 함
        photonView.RPC("RestoreHealth",RpcTarget.Others,newHealth);
        }
    }
    // 사망 처리
    public virtual void Die()
    {   //onDeath 이벤트에 등록된 메서드가 있다면 실행
        if(onDeath !=null)
        {
            onDeath();
        }
        //사망 상태를 참으로 변경
        dead = true;
    }
   
}
