using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum GameState
{
    NextTurn, // 다음턴
    NextRound, // 다음 라운드
    PlayerTurn, // 플레이어턴
    EnemyTurn, // 에너미턴
    Win, // 승리
    Lose // 패배
}

public class TurnManager : MonoBehaviour
{
    // 현재 게임 상태값
    private GameState gameState;
    [SerializeField] private TurnUI turnUI;
    // 현재 전투중인 캐릭터 참조
    [SerializeField] private List<Character> characters = new List<Character>();
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private GameObject defeatUI;
    private int roundCount = 0;
    // 턴을 저장할 큐
    private LinkedList<Character> turnQueue = new LinkedList<Character>();
    // 현재 턴을 가지는 캐릭터 참조변수
    private Character currentTurnCharacter;
    private bool inBattle;

    public List<Character> Characters { get => characters; set => characters = value; }
    public LinkedList<Character> TurnQueue { get => turnQueue; set => turnQueue = value; }
    public Character CurrentTurnCharacter => currentTurnCharacter;
    public TurnUI TurnUI => turnUI;

    public GameState GameState => gameState;

    private WaitForSeconds smallDelay = new WaitForSeconds(0.5f);
    private WaitForSeconds mediumDelay = new WaitForSeconds(1.5f);
    private WaitForSeconds largeDelay = new WaitForSeconds(3f);

    private void Start()
    {
        if (victoryUI.activeSelf)
        {
            victoryUI.SetActive(false);
        }
        if (defeatUI.activeSelf)
        {
            defeatUI.SetActive(false);
        }
    }

    public void GameStart()
    {
        if (inBattle) return;
        ChangeState(GameState.NextRound);
        inBattle = true;
    }
    // 게임 상태 변경 메소드
    public void ChangeState(GameState state)
    {
        // 이전 코루틴 종료
        StopCoroutine(gameState.ToString());
        // 현재 게임 상태 값
        gameState = state;
        // 현재 게임 상태에 맞는 코루틴 실행
        StartCoroutine(gameState.ToString());
    }
    private void CheckGameResult()
    {
        if (BattleManager.Instance.PlayerCharacterList.Count == 0)
        {
            ChangeState(GameState.Lose);
        }
        if (BattleManager.Instance.EnemyCharacterList.Count == 0)
        {
            ChangeState(GameState.Win);
        }
    }
    private IEnumerator NextTurn()
    {
        while (BattleManager.Instance.DeadCharacters.Count > 0)
        {
            // 리스트가 비어있으면 1초마다 대기
            yield return smallDelay;
        }

        CheckGameResult();
        yield return smallDelay; 
        // 모든 캐릭터들의 턴이 끝났다면
        if(turnQueue.Count == 0)
        {
            // 다음 라운드로 진행
            foreach (PlayableCharacter player in BattleManager.Instance.PlayerCharacterList)
            {
                player.HealMana(5);
            }
            ChangeState(GameState.NextRound); 
        }

        else // 아니라면
        {
            // 큐에서 요소를 하나 제거하고 currentTrunCharacter값에 저장 
            currentTurnCharacter = turnQueue.First();  // 첫 번째 요소 참조 (삭제는 안됨)
            turnQueue.RemoveFirst();                   // 첫 번째 요소 제거
            // 현재 턴의 캐릭터의 타입이
            switch (currentTurnCharacter.CharacterInfo.CharacterType)
            {
                // 플레이어라면
                case CharacterType.Player:
                    ChangeState(GameState.PlayerTurn); // 플레이어 턴으로 전환
                    break;
                // 적이라면
                case CharacterType.Enemy:
                    ChangeState(GameState.EnemyTurn); // 적의 턴으로 전환
                    break;
                default:
                    Debug.Log("캐릭터 타입이 정해지지 않음");
                    break;
            }
        }

    }

    // 전투 시작
    private IEnumerator NextRound()
    {
        // 턴 순서 초기화
        InitializeTurnOrder();
        turnUI.ResetTurnUI();
        turnUI.PrintTurnUI(turnQueue);
        roundCount++;
        turnUI.CurrentRoundUpdate(roundCount);
        // 전투 시작의 효과 (이펙트, 애니메이션이나 사운드 재생 등등...)
        yield return smallDelay;

        // 다음턴 진행
        ChangeState(GameState.NextTurn);
    }

    // 초기 턴 순서를 설정 (속도 기반 정렬)
    public void InitializeTurnOrder()
    {
        for (int i = 0; i < characters.Count; i++) 
        {
            characters[i].ResetTurn();
        }
        if (turnQueue != null)
        {
            turnQueue.Clear(); 
        }
        turnQueue = new LinkedList<Character>(characters.OrderByDescending(c => c.CurrentTurnSpeed));
    }

    // 플레이어 턴
    private IEnumerator PlayerTurn()
    {
        // 플레이어 컴포넌트 다운캐스팅
        PlayableCharacter player = (PlayableCharacter)currentTurnCharacter;
        player.PlayerCharacterUI.ActivateTurnDisplay();
        // 플레이어 턴에 대한 로직 작성
        // 데미지 or 스킬 등등...
        // 코루틴 호출 지연

        yield return StartCoroutine(currentTurnCharacter.TakeTurnCoroutine());

        yield return smallDelay;
        player.PlayerCharacterUI.DeactivateTurnDisplay();
        turnUI.NextTurnUpdate(currentTurnCharacter);
        // 행동 후 다음 턴
        ChangeState(GameState.NextTurn);
    }


    // 적의 턴
    private IEnumerator EnemyTurn()
    {
        // 적의 애니메이션, 효과 작성

        EnemyCharacter enemy = (EnemyCharacter)currentTurnCharacter;

        enemy.EnemyCharacterUI.ActivateTurnDisplay();

        yield return StartCoroutine(currentTurnCharacter.TakeTurnCoroutine());

        yield return smallDelay;
        enemy.EnemyCharacterUI.DeactivateTurnDisplay();
        turnUI.NextTurnUpdate(currentTurnCharacter);

        // 행동 후 다음 턴
        ChangeState(GameState.NextTurn);
    }

    // 승리
    private IEnumerator Win()
    {

        yield return smallDelay; // 승리 효과 재생

        victoryUI.SetActive(true);
    }

    // 패배
    private IEnumerator Lose()
    {

        yield return smallDelay; // 패배 효과 재생

        defeatUI.SetActive(true);

    }



}


