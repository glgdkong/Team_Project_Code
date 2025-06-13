using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    // 선택 가능 플레이어 표시 UI
    [SerializeField, Header("선택 가능 표시UI(플레이어)")] private SelectButtonUI[] playerSelection;
    // 선택 가능 적 표시 UI
    [SerializeField, Header("선택 가능 표시UI(적)")] private SelectButtonUI[] enemySelection;
    [SerializeField] private PlayerActionUI playerActionUI;

    private void Start()
    {
        SetUIPositions();
    }
    // 선택 가능 표시 UI 배치
    private void SetUIPositions()
    {
        int count = Mathf.Min(playerSelection.Length, enemySelection.Length);

        for (int i = 0; i < count; i++)
        {
            // 플레이어의 월드 좌표를 화면 좌표로 변환
            Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(BattleManager.Instance.SeatManager.PlayerPositions[i].position);
            // 플레이어 UI 요소의 RectTransform을 화면 좌표에 맞게 설정
            playerSelection[i].RectTransform.position = playerScreenPosition;
            playerSelection[i].SelectUI = this;
            if (playerSelection[i].gameObject.activeSelf)
            { 
                playerSelection[i].gameObject.SetActive(false); 
            }
            // 적의 월드 좌표를 화면 좌표로 변환
            Vector3 enemyScreenPosition = Camera.main.WorldToScreenPoint(BattleManager.Instance.SeatManager.EnemyPositions[i].position);
            // 적 UI 요소의 RectTransform을 화면 좌표에 맞게 설정
            enemySelection[i].RectTransform.position = enemyScreenPosition;
            enemySelection[i].SelectUI = this;
            if (enemySelection[i].gameObject.activeSelf)
            { 
                enemySelection[i].gameObject.SetActive(false); 
            }
        }
    }

    public void DisplayTargetableEnemies(bool[] targetablePos, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (targetablePos[i] && BattleManager.Instance.EnemyCharacterList[i] != null)
            {
                enemySelection[i].gameObject.SetActive(true);
            }
            else
            {
                enemySelection[i].gameObject.SetActive(false);
            }
        }

    }
    public void DisplayTargetablePlayer()
    {
        int count = Mathf.Min(BattleManager.Instance.PlayerCharacterList.Count, playerSelection.Length);
        for (int i = 0; i < count; i++) 
        {
            playerSelection[i].gameObject.SetActive(true);
        }
    }
    public void DisplayTargetableOnePlayer(int index)
    {
        playerSelection[index].gameObject.SetActive(true);
    }

    public void DisplayCharLocation(Character character)
    {
        switch (character.CharacterInfo.CharacterType)
        {
            case CharacterType.Player:
                playerSelection[character.CurrentPositionIndex].gameObject.SetActive(true);
                playerSelection[character.CurrentPositionIndex].PortraitMouseEnter();
                break;
            case CharacterType.Enemy:
                enemySelection[character.CurrentPositionIndex].gameObject.SetActive(true);
                enemySelection[character.CurrentPositionIndex].PortraitMouseEnter();
                break;
            default:
                break;
        }
    }

    public void DisableDisplayCharLocation()
    {
        switch (BattleManager.Instance.PlayerActionManager.Player_Action)
        {
            case Player_Action.Skill_Use:
                playerActionUI.ShowSkillRange(playerActionUI.SkillInfoSO);
                break;
            case Player_Action.Move:
                DisableTargetUI();
                playerActionUI.ShowMoveRange();
                break;
            case Player_Action.Stay:
                DisableTargetUI();
                break;
            default:
                break;
        }
        /*if (playerActionUI.SkillInfoSO != null) 
        {
            Debug.Log("1");
            playerActionUI.ShowSkillRange(playerActionUI.SkillInfoSO);
        }
        else
        {
            Debug.Log("2");
            DisableTargetUI();
        }*/
    }



    public void DisplayMovePosition(int min, int max, int currentIndex)
    {
        // 이동 가능 범위 UI 활성화 및 위치 설정
        for (int i = min; i <= max; i++)
        {
            if (i != currentIndex && BattleManager.Instance.PlayerCharacterList[i] != null) // 현재 자리 제외
            {
                playerSelection[i].gameObject.SetActive(true); // UI 활성화
            }
        }

    }

    public void DisableTargetUI()
    {
        int count = Mathf.Min(playerSelection.Length, enemySelection.Length);
        for (int i = 0; i < count; i++) 
        {
            if(playerSelection[i].gameObject.activeSelf)
            { 
                playerSelection[i].gameObject.SetActive(false); 
            }
            if (enemySelection[i].gameObject.activeSelf)
            {
                enemySelection[i].gameObject.SetActive(false); 
            }
        }
    }

    public void ActivateAreaTargets(SelectButtonUI selectButtonUI, SkillInfoSO skillInfoSO)
    {
        if (skillInfoSO.SkillRangeType != SkillRangeType.Area) return;
        int count = 0;
        switch (selectButtonUI.CharacterType)
        {
            case CharacterType.Player:
                count = Mathf.Min(BattleManager.Instance.PlayerCharacterList.Count, playerSelection.Length);
                break;
            case CharacterType.Enemy:
                count = Mathf.Min(BattleManager.Instance.EnemyCharacterList.Count, enemySelection.Length); 
                break;
        }

        switch (skillInfoSO.SkillType)
        {
            case SkillType.Attack:
                //Debug.Log("다중공격 체크");
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;
                for (int i = 0; i < count; i++)
                {
                    if (attackSkillSO.TargetPositions[i])
                    {
                        if (enemySelection[i].gameObject.activeSelf)
                        {
                            enemySelection[i].Active();
                        }
                    }
                }
                break;
            case SkillType.Heal:
            case SkillType.Buff:
                //Debug.Log("다중스킬 체크");
                for (int i = 0; i < count; i++)
                {
                    playerSelection[i].Active();
                }
                break;
        }
    }

    public void DeactivateAreaTargets(SelectButtonUI selectButtonUI, SkillInfoSO skillInfoSO)
    {
        if (skillInfoSO.SkillRangeType != SkillRangeType.Area) return;
        int count = 0;
        switch (selectButtonUI.CharacterType)
        {
            case CharacterType.Player:
                count = Mathf.Min(BattleManager.Instance.PlayerCharacterList.Count, playerSelection.Length);
                break;
            case CharacterType.Enemy:
                count = Mathf.Min(BattleManager.Instance.EnemyCharacterList.Count, enemySelection.Length);
                break;
        }

        switch (selectButtonUI.CharacterType)
        {
            case CharacterType.Player:
                for (int i = 0; i < count; i++)
                {
                    if (playerSelection[i].gameObject.activeSelf)
                    {
                        playerSelection[i].DeActiave();
                    }
                }
                break;
            case CharacterType.Enemy:
                for (int i = 0; i < count; i++)
                {
                    if (enemySelection[i].gameObject.activeSelf)
                    {
                        enemySelection[i].DeActiave();
                    }

                }
                break;
        }

    }

}
