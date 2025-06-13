using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum Action_Type
{
    Move,
    TurnEnd
}
public class ActionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Action_Type actionType;
    private PlayerActionUI actionUI;
    // Start is called before the first frame update
    void Start()
    {
        actionUI = GetComponentInParent<PlayerActionUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (actionType)
        {
            case Action_Type.Move:
                actionUI.SelectUI.DisableTargetUI();
                actionUI.ShowMoveRange();
                actionUI.ShowSkillName("자리 이동");
                break;
            case Action_Type.TurnEnd:
                actionUI.SelectUI.DisableTargetUI();
                actionUI.ShowSkillName("턴 넘기기");
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (BattleManager.Instance.PlayerActionManager.Player_Action)
        {
            case Player_Action.Skill_Use:
            case Player_Action.Stay:
                if (actionUI.SkillInfoSO != null)
                {
                    actionUI.ShowSkillName(actionUI.SkillInfoSO.SkillName);
                    actionUI.ShowSkillRange(actionUI.SkillInfoSO);
                }
                else
                {
                    actionUI.ShowSkillRange(null);
                    actionUI.SkillAllDeSelect();
                }
                break;
            case Player_Action.Move:
                actionUI.ShowSkillRange(null);
                actionUI.SelectUI.DisableTargetUI();
                actionUI.ShowSkillName("자리 이동");
                actionUI.ShowMoveRange();
                break;
            default:
                break;
        }
    }
}
