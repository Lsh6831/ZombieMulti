using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZombieSpawner : MonoBehaviour
{

    public Zombie zombiePreFab; // 생성할 좀비 원본 프리팹

    public ZombieData[] zombieDatas;// 사용할 좀비 셋업 데이터
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치

    private List<Zombie> zombies = new List<Zombie>(); //생성된 좀비를 담는 리스트
    private int wave; //현재 웨이브

    private void Update()
    {
        //게임 오버 상태일 때는 생성하지 않음
        if (Gamemanager.instance != null && Gamemanager.instance.isGameover)
        {
            return;
            //아래로 내려가지 마라 라는뜻
        }

        //좀비를 모두 물리친 경우 다음 스폰 실행
        if (zombies.Count <= 0)
        //리스트의 크기
        {
            SpawnWave();
        }

        //UI 갱신
        UpdateUI();
    }

    //웨이브 정보를 UI 로 표시
    private void UpdateUI()
    {
        //현재 웨이브와 남은 좀비 수 표시
        UIManager.instance.UpdateWaveText(wave, zombies.Count);
    }
    //현재 웨이브에 맞춰 좀비 생성
    private void SpawnWave()
    {
        // 웨이브 1증가
        wave++;

        // 현재 웨이브*1.5를 반올림한 수만큼 좀비 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        // 반올림 연산 MAthf.RoundToTint(공학처리용 클레스)

        // spawnCount 만큼 좀비 생성
        for (int i = 0; i < spawnCount; i++)
        {
            //좀비 생성 처리 실행
            CreateZombie();
        }
        // spawnCount 만큼 반복 실행

    }

    //좀비를 생성하고 좀비에 추적할 대상 할당
    private void CreateZombie()
    {
        //사용할 좀비 데이터 랜덤으로 결정
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];
        //                                                             Length 는 공간의 숫자 이므로 0~2 면 3개로 생각

        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //좀비 프리팹으로부터 좀비 생성
        Zombie zombie = Instantiate(zombiePreFab, spawnPoint.position, spawnPoint.rotation);
        //              생성한다    생성할 개체 , 위치                , 방향

        // 생성한 좀비 능력치 설정
        zombie.Setup(zombieData);

        // 생성된 좀비를 리스트에 추가
        zombies.Add(zombie);

        // 좀비의 onDeath 이벤트에 익명 메서드 등록
        // 사망한 좀비를 리스트에서 제거
        zombie.onDeath += () => zombies.Remove(zombie);
        // += -> 구독 처리 ()~에서() 까지 -> 익명함수 설정
        // 사망한 좀비를 10초 뒤에 파괴
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);
        // 좀비 사망 시 점수 상승
        zombie.onDeath += () => Gamemanager.instance.AddScore(100);
        // 위 3가지 같은걸 람다식,람다표현식 이라고 한다
    }
}
