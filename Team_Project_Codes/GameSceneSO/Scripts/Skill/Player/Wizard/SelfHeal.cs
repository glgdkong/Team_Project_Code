using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SelfHeal", menuName = "SKill/Player/Wizard/HealSkill/SelfHeal")]
public class SelfHeal : HealSkillSO
{
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        int healAmount = isCriticalHit ? (int)(impactValue.y * 2) : (int)impactValue.y;
        targetCharacter.HealHealth(healAmount, isCriticalHit);
    }
}
