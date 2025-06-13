using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 플레이어 행동 UI
    private PlayerActionUI actionUI;
    // 스킬 정보
    private SkillInfoSO skillInfoSO;
    private Button button;
    private SpriteState spriteState;
    // 스킬 이미지
    [SerializeField] private Image skillIcon;
    // 스킬 선택 표시 이미지
    [SerializeField] private GameObject skillSelectedIcon;
    [SerializeField] private GameObject skillHighlightIcon;
    // 스킬 선택 여부
    [SerializeField] private bool isSelected = false;
    // 스킬 정보 표시 UI
    [SerializeField] private SkillInfoUI skillInfoUI;

    private void Awake()
    {
        actionUI = GetComponentInParent<PlayerActionUI>();
        skillIcon = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    private void Start()
    {
        //button.onClick.AddListener(SkillSelect);
        if (!isSelected)
        {
            skillHighlightIcon.gameObject.SetActive(false);
            skillSelectedIcon.gameObject.SetActive(false);
        }
    }

    public void InIt(SkillInfoSO skillInfoSO)
    {
        this.skillInfoSO = skillInfoSO;
        skillIcon.sprite = this.skillInfoSO.SkillEImage;
        spriteState.disabledSprite = this.skillInfoSO.SkillDImage;
        button.spriteState = spriteState;
    }
    // 스킬 정보 초기화
    public void ClearSkillUI()
    {
        skillInfoSO = null;    // 스킬 데이터 초기화
        skillIcon.sprite = null; // 스킬 스프라이트 이미지 초기화
        spriteState.disabledSprite = null;
        button.spriteState = spriteState;
    }

    public void SkillSelect()
    {
        // 스킬 정보가 없다면 리턴 
        if (skillInfoSO == null) return;
        if (isSelected) 
        {
            actionUI.SkillAllDeSelect();
            if (skillHighlightIcon.activeSelf)
                skillHighlightIcon?.SetActive(false);
            return; 
        }
        // 선택한 스킬 정보를 UI에 출력함
        actionUI.SkillSelect(skillInfoSO);
        //skillSelectedIcon.gameObject.SetActive(true);
        skillInfoUI.gameObject.SetActive(true);
        skillInfoUI.PrintSkillInfo(skillInfoSO);
        // 선택이 안된 상태면
        if (!isSelected)
        {
            // 선택된 상태로 표시함
            skillSelectedIcon?.SetActive(true);
            if(skillHighlightIcon.activeSelf)
            skillHighlightIcon?.SetActive(false);
            //Debug.Log("스킬 선택을 처리함");
            isSelected = true;
        }
    }



    public void SkillDeSelect()
    {
        // 이미 선택된 상황이면
        if (isSelected) 
        {
            // 선택표시 비활성화
            //skillSelectedIcon.SetActive(false);
            isSelected = false;
            skillSelectedIcon.gameObject.SetActive(false);
            skillInfoUI.gameObject.SetActive(false);
        }
    }
    public void DisableSkillButton(PlayableCharacter player)
    {
        if (skillInfoSO.CanUsePos[player.CurrentPositionIndex] && skillInfoSO.ManaCost <= player.CurrentMana)
        {
            switch (skillInfoSO.SkillType)
            {
                case SkillType.Attack:
                    AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;
                    bool isActive = false;
                    int index = 0;
                    index = Mathf.Min(attackSkillSO.TargetPositions.Length, BattleManager.Instance.EnemyCharacterList.Count);
                    for (int i = 0; i < index; i++)
                    {
                        isActive = attackSkillSO.TargetPositions[i];
                        if (isActive)
                        {
                            break;
                        }
                    }
                    button.interactable = isActive;
                    break;
                case SkillType.Heal:
                case SkillType.Buff:
                    button.interactable = true;
                    break;
                default:
                    break;
            }
            
        }
        else
        {
            button.interactable = false;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillInfoUI.gameObject.SetActive(true);
        skillHighlightIcon.gameObject.SetActive(true);
        skillInfoUI.PrintSkillInfo(skillInfoSO);
        actionUI.ShowSkillRange(skillInfoSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillHighlightIcon.gameObject.SetActive(false);
        switch (BattleManager.Instance.PlayerActionManager.Player_Action)
        {
            case Player_Action.Skill_Use:
            case Player_Action.Stay:
                if (actionUI.SkillInfoSO != null)
                {
                    skillInfoUI.PrintSkillInfo(actionUI.SkillInfoSO);
                    actionUI.ShowSkillRange(actionUI.SkillInfoSO);
                }
                else
                {
                    skillInfoUI.PrintSkillInfo(null);
                    actionUI.SkillAllDeSelect();
                }
                break;
            case Player_Action.Move:
                actionUI.ShowSkillRange(null);
                actionUI.SelectUI.DisableTargetUI();
                skillInfoUI.PrintSkillInfo(null);
                actionUI.ShowSkillName("자리 이동");
                actionUI.ShowMoveRange();
                break;
            default:
                break;
        }
    }
}
