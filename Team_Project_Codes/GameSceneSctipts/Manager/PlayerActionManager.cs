using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 플레이어 행동 타입
public enum Player_Action
{
    Skill_Use, // 스킬 사용
    Move, // 이동
    Stay // 대기
}
public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private PlayerActionUI playerActionUI;
    private Player_Action playerAction = Player_Action.Stay;
    private PlayableCharacter playableCharacter;
    [SerializeField] private SkillInfoSO skillInfoSO;
    private GameObject target;
    public Player_Action Player_Action { get => playerAction; set => playerAction = value; }

    public PlayableCharacter PlayableCharacter { get => playableCharacter; set => playableCharacter = value; }
    public PlayerActionUI PlayerActionUI { get => playerActionUI; set => playerActionUI = value; }
    public SkillInfoSO SkillInfoSO => skillInfoSO;

    private void Update()
    {
        // 플레이어가 턴을 종료한 상태면 리턴
        if (playableCharacter == null||playableCharacter.PlayerAction) return;
        PlayerAction();
    }
    // 플레이어 행동
    private void PlayerAction()
    {
        switch (playerAction)
        {
            case Player_Action.Skill_Use: //스킬 사용 상태
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 레이 생성
                    RaycastHit hit;  // 충돌 정보를 담을 변수
                    GameObject previousTarget = target;  // 이전 타겟 저장
                    if (Physics.Raycast(ray, out hit))  // 레이가 충돌했는지 검사
                    {
                        target = hit.collider.gameObject;  // 타겟 변수에 저장
                    }
                    else // 충돌한 오브젝트가 없다면
                    {
                        target = null; // null 처리
                    }

                    if (target != null && (target.CompareTag("Player") || target.CompareTag("Enemy")))
                    {
                        Character character = target.GetComponent<Character>();
                        if (BattleManager.Instance.SkillUsable(playableCharacter, skillInfoSO, character))
                        {
                            playableCharacter.PlayerActionEnd();
                            playerAction = Player_Action.Stay;
                            playableCharacter.UseSkill(skillInfoSO, character);
                            string skillName = skillInfoSO.SkillName;
                            ClearPlayerInfo();
                            BattleManager.Instance.PlayerActionManager.PlayerActionUI.ShowSkillName(skillName);

                        }
                    }
                }
                break;
            case Player_Action.Move: // 이동 상태면
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 마우스 위치에서 레이 생성
                    RaycastHit hit;  // 충돌 정보를 담을 변수
                    GameObject previousTarget = target;  // 이전 타겟 저장
                    if (Physics.Raycast(ray, out hit))  // 레이가 충돌했는지 검사
                    {
                        target = hit.collider.gameObject;  // 타겟 변수에 저장
                    }
                    else // 충돌한 오브젝트가 없다면
                    {
                        target = null; // null 처리
                    }

                    if (target != null && target.CompareTag("Player"))
                    {
                        PlayableCharacter targetPos = target.GetComponent<PlayableCharacter>();

                        // 현재 캐릭터와 대상이 같은 경우 처리 방지
                        if (targetPos == playableCharacter)
                        {
                            return;
                        }

                        PlayableCharacterSO currentPlayer = (PlayableCharacterSO)playableCharacter.CharacterInfo;

                        // 현재 위치 및 이동 가능한 범위 계산
                        int currentIndex = playableCharacter.CurrentPositionIndex;
                        int minIndex = Mathf.Max(0, currentIndex - currentPlayer.MoveRange);
                        int maxIndex = Mathf.Min(BattleManager.Instance.PlayerCharacterList.Count - 1, currentIndex + currentPlayer.MoveRange);

                        // 대상 캐릭터의 인덱스
                        int targetIndex = BattleManager.Instance.PlayerCharacterList.IndexOf(targetPos);

                        // 대상 인덱스가 이동 가능 범위 내인지 확인
                        if (targetIndex >= minIndex && targetIndex <= maxIndex)
                        {

                            // 자리 이동 처리 호출
                            if (BattleManager.Instance.SeatManager.MoveCharacterToSlot(playableCharacter, targetIndex))
                            {
                                playableCharacter.SetTurnEnd();
                                ClearPlayerInfo();
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"이동 불가: {targetIndex} (허용 범위: {minIndex}-{maxIndex})");
                        }
                    }
                }
                break;
            case Player_Action.Stay:
                break;
            default:
                break;
        }
    }
    // 현재 턴의 플레이어 정보 가져오기
    public void PlayerTurn(PlayableCharacter playableCharacter)
    {
        this.playableCharacter = playableCharacter;
        playerActionUI.ShowSkillInfo(this.playableCharacter);
    }

    public void PlayerTurnEnd()
    {
        playableCharacter.SetTurnEnd();
        ClearPlayerInfo();
    }
    public void ClearPlayerInfo()
    {
        playerActionUI.ClearSkillInfos();
        skillInfoSO = null;
    }
    public void GetSkillInfo(SkillInfoSO skillInfo)
    {
        skillInfoSO = skillInfo;
    }

}
