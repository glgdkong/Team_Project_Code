using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LimitSituation", menuName = "SKill/Player/Guardian/BuffSkill/LimitSituation")]
public class LimitSituation : BuffSkillSO
{
    [SerializeField] private DodgeUp dodgeUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        dodgeUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(dodgeUp, true);
    }
}
