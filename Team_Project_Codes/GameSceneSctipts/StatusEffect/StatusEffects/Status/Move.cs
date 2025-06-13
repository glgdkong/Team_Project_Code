using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Move : StatusEffect
{
    public override StatusType StatusType => StatusType.Move;
 
    public override StatusEffectType StatusEffectType => StatusEffectType.Move;


    [SerializeField, Header("뒤로 이동 여부")] private bool isMovingBackward;
    public bool IsMovingBackward => isMovingBackward;

    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Move move)
        {
            BattleManager.Instance.SeatManager.ForceMoveCharacterToSlot(statusEffectHandler.Character, move.Count, move.IsMovingBackward);
        }
    }

    public override string StatusInfo()
    {
        return null;
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#808080>강제 이동</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#808080>강제 이동</color> <color=#B7C5CD>저항</color>";
    }

}
