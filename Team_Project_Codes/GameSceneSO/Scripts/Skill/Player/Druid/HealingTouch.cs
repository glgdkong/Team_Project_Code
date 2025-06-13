using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingTouch", menuName = "SKill/Player/Druid/HealSkill/HealingTouch")]
public class HealingTouch : HealSkillSO
{
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        int value = Random.Range((int)impactValue.x, (int)impactValue.y + 1);
        int healAmount = isCriticalHit ? (value * 2) : value;
        targetCharacter.HealHealth(healAmount, isCriticalHit);
    }
}
