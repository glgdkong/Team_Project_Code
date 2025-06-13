using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UntidyMind", menuName = "SKill/Player/Guardian/AttackSkill/UntidyMind")]
public class UntidyMind : BuffSkillSO
{
    [SerializeField] private ResUp resUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        resUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(resUp, true);
    }
}
