using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SacrificialHymn", menuName = "SKill/Player/Druid/HealSkill/SacrificialHymn")]
public class SacrificialHymn : HealSkillSO
{
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        int healAmount = isCriticalHit ? (int)(impactValue.y * 2) : (int)impactValue.y;
        targetCharacter.HealHealth(healAmount, isCriticalHit);
    }
}
