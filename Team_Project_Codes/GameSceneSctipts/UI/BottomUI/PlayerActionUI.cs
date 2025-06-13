using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionUI : MonoBehaviour
{
    // 플레이어 정보 표시 UI
    [SerializeField, Header("플레이어 정보 표시 UI들")] private GameObject[] playerInfoUI;
    // 플레이어 이름 표시할 텍스트
    [SerializeField, Header("플레이어 이름 표시할 텍스트 참조")] private TextMeshProUGUI playerName;
    // 스킬 이름 표시할 텍스트
    [SerializeField, Header("스킬 이름 표시할 텍스트 참조")] private TextMeshProUGUI skillName;
    // 스킬 표시 UI 배열  
    [SerializeField, Header("스킬 표시 UI 배열")] private SkillIconUI[] skillUIs;
    /*// 선택 가능 플레이어 표시 UI
    [SerializeField, Header("선택 가능 표시UI(플레이어)")] private Image[] playerSelection;
    // 선택 가능 적 표시 UI
    [SerializeField, Header("선택 가능 표시UI(적)")] private Image[] enemySelection;*/
    [SerializeField] private SelectUI selectUI;

    [SerializeField, Header("사용가능 자리표시 UI")] private GameObject usablePos;
    [SerializeField] private Image[] usbaleImages;
    [SerializeField, Header("타겟가능 자리표시 UI")] private GameObject targetablePos;
    [SerializeField] private Image[] targetAbleImage;

    [SerializeField, Header("공격 스킬 정보표시 UI")] private SkillTargetInfoUI skillTargetInfoUI;

    private PlayableCharacter currentPlayer;

    private SkillInfoSO skillInfoSO;

    private bool swapButtonSelect;

    public SkillTargetInfoUI SkillTargetInfoUI => skillTargetInfoUI;

    public SkillInfoSO SkillInfoSO => skillInfoSO;

    public SelectUI SelectUI { get => selectUI; set => selectUI = value; }

    private void Start()
    {
        skillTargetInfoUI.PlayerActionUI = this;
        DisablePlayerInfos();
    }


    public void SkillSelect(SkillInfoSO skillInfo)
    {
        SkillAllDeSelect();
        swapButtonSelect = false;
        ShowSkillRange(skillInfo);
        skillInfoSO = skillInfo;
        /*
        switch (skillInfo.SkillType)
        {
            case SkillType.Attack:
                skillTargetInfoUI.gameObject.SetActive(true);
                break;
            default:
                skillTargetInfoUI.gameObject.SetActive(false);
                break;
        }*/
        skillTargetInfoUI.GetSkillInfo(skillInfo);
        BattleManager.Instance.PlayerActionManager.Player_Action = Player_Action.Skill_Use;
        BattleManager.Instance.PlayerActionManager.GetSkillInfo(skillInfo);
    }

    public void ShowSkillName(string skillName)
    {
        //Debug.Log("스킬 이름 표시 1 " + skillName);
        if(skillName != null)
        {
            //Debug.Log("스킬 이름 표시 2 " + skillName);
            this.skillName.text = skillName;
        }
        else
        {
            this.skillName.text = null;
        }
    }
    public void ShowSkillRange(SkillInfoSO skillInfo)
    {
        if (skillInfo == null)
        {
            //Debug.Log("비활성화");
            usablePos.SetActive(false);
            targetablePos.SetActive(false);
            return; 
        }
        selectUI.DisableTargetUI();
        for (int i = 0; i < skillInfo.CanUsePos.Length; i++) 
        {
            if (skillInfo.CanUsePos[i])
            {
                usbaleImages[i].gameObject.SetActive(true);
            }
            else
            {
                usbaleImages[i].gameObject.SetActive(false);
            }
        }
        usablePos.SetActive(true);
        targetablePos.SetActive(false);
        switch (skillInfo.SkillType)
        {
            case SkillType.Attack:
                AttackSkillSO atkSkillInfo = (AttackSkillSO)skillInfo;
                usablePos.SetActive(true);
                targetablePos.SetActive(true);
                AttackSkill(atkSkillInfo);
                break;
            case SkillType.Heal:
                HealSkillSO healSkillSO = (HealSkillSO)skillInfo;
                HealAndSkill(healSkillSO);
                break;
            case SkillType.Buff:
                BuffSkillSO buffSkillSO = (BuffSkillSO)skillInfo;
                BuffSkill(buffSkillSO);
                break;
            default:
                break;
        }
    }
    private void AttackSkill(AttackSkillSO attackSkillSO) 
    {
        int count = Mathf.Min(BattleManager.Instance.EnemyCharacterList.Count, targetAbleImage.Length);
        selectUI.DisplayTargetableEnemies(attackSkillSO.TargetPositions, count);
        for (int i = 0; i < targetAbleImage.Length; i++) 
        {
            if (attackSkillSO.TargetPositions[i])
            {
                targetAbleImage[i].gameObject.SetActive(true);
            }
            else
            {
                targetAbleImage[i].gameObject.SetActive(false);
            }

        }
    }
    private void HealAndSkill(HealSkillSO healSkillSO)
    {
        switch (healSkillSO.SkillRangeType)
        {
            case SkillRangeType.Single:
            case SkillRangeType.Area:
                selectUI.DisplayTargetablePlayer();
                break;
            case SkillRangeType.Self:
                /*for (int i = 0; i < usbaleImages.Length; i++)
                {
                    if(!(currentPlayer.CurrentPositionIndex == i))
                    {
                        usbaleImages[i].gameObject.SetActive(false);
                    }
                }*/
                selectUI.DisplayTargetableOnePlayer(currentPlayer.CurrentPositionIndex);
                break;
            default:
                break;
        }
    }
    private void BuffSkill(BuffSkillSO buffSkillSO)
    {
        switch (buffSkillSO.SkillRangeType)
        {
            case SkillRangeType.Single:
            case SkillRangeType.Area:
                selectUI.DisplayTargetablePlayer();
                break;
            case SkillRangeType.Self:
                /*for (int i = 0; i < usbaleImages.Length; i++)
                {
                    if (!(currentPlayer.CurrentPositionIndex == i))
                    {
                        usbaleImages[i].gameObject.SetActive(false);
                    }
                }*/
                selectUI.DisplayTargetableOnePlayer(currentPlayer.CurrentPositionIndex);
                break;
            default:
                break;
        }
    }

    public void ShowSkillInfo(PlayableCharacter player)
    {
        currentPlayer = player;
        //Debug.Log("플레이어 정보UI 표시");
        foreach (GameObject playerInfoUI in playerInfoUI)
        {
            playerInfoUI.SetActive(true);
        }
        playerName.text = player.CharacterInfo.CharacterName;
        for (int i = 0; i < skillUIs.Length; i++)
        {
            skillUIs[i].InIt(player.HasSkillList[i]);
            skillUIs[i].DisableSkillButton(player);
        }
    }

    private void DisablePlayerInfos()
    {
        //Debug.Log("플레이어 정보UI 표시");
        foreach (GameObject playerInfoUI in playerInfoUI)
        {
            if(playerInfoUI.gameObject.activeSelf)
            { playerInfoUI.SetActive(false); }
        }
    }

    // 
    public void SkillAllDeSelect()
    {
        BattleManager.Instance.PlayerActionManager.GetSkillInfo(null);
        skillTargetInfoUI.GetSkillInfo(null);
        skillName.text = null;
        //Debug.Log("현재 스킬 이름 "+ skillName.text);
        BattleManager.Instance.PlayerActionManager.Player_Action = Player_Action.Stay;
        for (int i = 0; i < skillUIs.Length; i++)
        {
            skillUIs[i].SkillDeSelect();
        }
        int count = Mathf.Min(usbaleImages.Length, targetAbleImage.Length);
        usablePos.SetActive(false);
        targetablePos.SetActive(false);
        skillTargetInfoUI.gameObject.SetActive(false);
        selectUI.DisableTargetUI();
        for (int i = 0; i < count; i++)
        {
            usbaleImages[i].gameObject.SetActive(false);
            targetAbleImage[i].gameObject.SetActive(false);
        }
        skillInfoSO = null;
    }
    public void ClearSkillInfos()
    {
        DisablePlayerInfos();
        for (int i = 0; i < skillUIs.Length; i++)
        {
            skillUIs[i].ClearSkillUI();
        }
        swapButtonSelect = false;
        currentPlayer = null;
        SkillAllDeSelect();
    }
    // 턴 스킵 버튼 이벤트
    public void SkipTurn()
    {
        //Debug.Log("턴 스킵 버튼 누름");
        SkillAllDeSelect();
        BattleManager.Instance.PlayerActionManager.PlayerTurnEnd();
    }

    public void SwapPosition()
    {
        //Debug.Log("자리 이동 버튼 누름");
        SkillAllDeSelect();
        BattleManager.Instance.PlayerActionManager.Player_Action = Player_Action.Move;
        ShowMoveRange();
        if (!swapButtonSelect)
        {
            ShowSkillName("자리 이동");
            swapButtonSelect = true;
        }
        else
        {
            swapButtonSelect = false;
            BattleManager.Instance.PlayerActionManager.Player_Action = Player_Action.Stay;
            selectUI.DisableTargetUI();
        }
    }
    public void ShowMoveRange()
    {
        // 현재 캐릭터의 인덱스
        int currentIndex = currentPlayer.CurrentPositionIndex;
        // 앞뒤 이동 범위 계산
        int minIndex = Mathf.Max(0, currentIndex - currentPlayer.PlayableCharacterSO.MoveRange);
        int maxIndex = Mathf.Min(BattleManager.Instance.PlayerCharacterList.Count - 1, currentIndex + currentPlayer.PlayableCharacterSO.MoveRange);
        selectUI.DisplayMovePosition(minIndex, maxIndex, currentIndex);
    }
}
