using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; //앞뒤 움직임을 위한 입력축 이름
    public string rotareAxisName = "Horizontal"; //좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름

    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; }// 감지된 회전 입력값
    public bool fire { get; private set; }// 감지된 발사 입력값
    public bool reload { get; private set; }// 감지된 재장전 입력값


    // Update is called once per frame
    void Update()
    {
        //게임오버 상태에서는 사용자 입력을 감지하지 않음
    if(Gamemanager.instance!=null && Gamemanager.instance.isGameover)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }
    // 프로퍼티**
        //move에 관한 입력 감지
        move = Input.GetAxis(moveAxisName);
        //rotate 에 관란 입력감지
        rotate = Input.GetAxis(rotareAxisName);
        //fire 에 관한 입력감지
        fire = Input.GetButton(fireButtonName);
        //reload에 관한 입력감지
        reload = Input.GetButtonDown(reloadButtonName);
    }

}
