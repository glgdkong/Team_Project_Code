using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IronWill", menuName = "SKill/Player/Guardian/BuffSkill/IronWill")]
public class IronWill : BuffSkillSO
{
    [SerializeField] protected DefUp defUp; 
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        defUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(defUp, true);
    }
}
