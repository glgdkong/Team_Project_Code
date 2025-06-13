using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bind", menuName = "SKill/Player/Druid/StatusAttackSkill/Bind")]
public class Bind : StatusAttackSkillSO
{
    [SerializeField] private Slow slow;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(slow.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(slow, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(slow, false);
        }
    }
}
