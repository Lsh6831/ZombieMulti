using Cinemachine; // 시네머신 관련 코드
using Photon.Pun; // PUN관련 코드
using UnityEngine;

public class CameraSetup : MonoBehaviourPun
{
    
    void Start()
    // 만약 자신이 로컬 플레이어 라면
    {
        if(photonView.IsMine)
        {
        // 씬에 있는 시네머신 가상 카메라를 찾고
        CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
        // 가상 카메라의 추적 대상을 자신의 트랜스폼으로 변경
        followCam.Follow=transform;
        followCam.LookAt=transform;
        }
        
    }

    
}
