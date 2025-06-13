using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreathControl", menuName = "SKill/Player/Valkyrie/HealSkill/BreathControl")]
public class BreathControl : HealSkillSO
{
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        int healAmount = isCriticalHit ? (int)(impactValue.y * 2) : (int)impactValue.y;
        targetCharacter.HealHealth(healAmount, isCriticalHit);
    }
}
