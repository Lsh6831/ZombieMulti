using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{

    public Gun gun;  //사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount;  // 총읜 왼쪽 손잡이,왼손이 위치할 지점
    public Transform rightHandMount;  // 총의 오른쪽 손잡이. 오른손이 위치할 지점

    private PlayerInput playerInput;  // 플레이어의 입력
    private Animator playerAnimator;  // 애니메이터 컴포넌트


    // Start is called before the first frame update
    void Start()
    {//사용할 컴포넌트 가져오기
        playerInput=GetComponent<PlayerInput>();
        playerAnimator=GetComponent<Animator>();               
    }

    void OnEnable()
    {//슈터가 활성화될 때 총도 함꼐 활성화
        gun.gameObject.SetActive(true);
    }
    void OnDisable()
    {// 슈터가 비활성화될 떄 총도 함꼐 비활성화
        gun.gameObject.SetActive(false);
    }
    private void Update()
    {//입력을 감지하고 총을 발사하거나 재장전
    if(playerInput.fire)
    {// 발사 입력 감지 시 총 발사
        gun.Fire();
    }
    else if(playerInput.reload)
    {
        // 재장전 입력 감지 시 재장전
        if(gun.Reload())
        {
            // 재장전 성공 시에만 재장전 애니메이션 재생
            playerAnimator.SetTrigger("Reload");
        }
    }

    //탄알 UI갱신
    UPdateUI();
    
    }
    
    private void UPdateUI()
    { 
        //UI 매니저의 탄알 텍스트에 탄창의 탄알과 남은 전체 탄알 표시
        if(gun!=null&&UIManager.instance !=null)
        {//UI메니저는 싱글턴 처리
            UIManager.instance.UpdateAmmoText(gun.magAmmo,gun.ammoRemain);
        }
    }
    // 에니메이터의 IK갱신
    private void OnAnimatorIK(int layerIndex)
    {// 총의 기준점 gunPivo을 3D 모델의 오른쪽 팔꿈치 위치로 이동
    gunPivot.position=playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

    //IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
    playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1.0f);
    playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1.0f);

    playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand,leftHandMount.position);
    playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand,leftHandMount.rotation);

     //IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤
    playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand,1.0f);
    playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand,1.0f);

    playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,rightHandMount.position);
    playerAnimator.SetIKRotation(AvatarIKGoal.RightHand,rightHandMount.rotation);
    }

}
