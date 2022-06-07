using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 점수와 게임오버 여부를 관리하는 게임 매니저
public class Gamemanager : MonoBehaviour
{
    // 싱글턴 접근용 프로퍼티
    public static Gamemanager instance
    {
        get
        {
            //만약 싱글턴 변수에 아직 오브젝트가 할당되지 않았다면
            if(m_instance==null)
            {
                // 씬에서 GameManager 오브젝트를 찾아서 할당
                m_instance=FindObjectOfType<Gamemanager>();
            }
            // 싱글턴 오브젝트 반환
            return m_instance;
        }
    }
    private static Gamemanager m_instance; //싱글턴이 할당될 static 변수

    private int score = 0; // 현재 게임 점수
    public bool isGameover { get;private set;} // 게임 오버 상태
    private void Awake() 
    {
    // 씬에 싱글턴 오브젝트가 된 다른 GameManager 오브젝트가 있다면
    if(instance != this)
    {       
        // 자신을 파괴
        Destroy(gameObject);
    }    
    }
    private void Start() {
        {//플레이어 캐릭터의 사망 이벤트 발생 시 게임오버
        FindObjectOfType<PlayerHealth>().onDeath+=EndGame;
        // onDeath에 EndGame 을 구독 처리 하여 onDeath 실행시 , EndGame 같이 실행
        }
    }
    // 점수 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        //게임 오버가 아닌 상태에서만 점수 추가 가능
        if(!isGameover)
        {
            //점수 추가 
            score+=newScore;
            //점수 UI텍스트 갱신
            UIManager.instance.UpdataeScoreText(score);
        }
    }
    // 게임 오버 처리
    public void EndGame()
    {
        // 게임오버 상태를 참으로 변경
        isGameover = true;
        //게임오버 UI 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }
}

    
