using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPun,IItem
{
 public int score = 200; // 증가할 점수

 public void Use (GameObject target)
 {
     // 게임 매니저에 접근해 점수 추가
     Gamemanager.instance.AddScore(score);
     //사용되었으므로 자신을 파괴
     PhotonNetwork.Destroy(gameObject);
 }
    
}
