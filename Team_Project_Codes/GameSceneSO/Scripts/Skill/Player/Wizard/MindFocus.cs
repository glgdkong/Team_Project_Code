using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MindFocus", menuName = "SKill/Player/Wizard/HealSkill/MindFocus")]
public class MindFocus : HealSkillSO
{
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        int healAmount = isCriticalHit ? (int)(impactValue.y * 2) : (int)impactValue.y;
        targetCharacter.HealMana(healAmount, isCriticalHit);
    }
}
