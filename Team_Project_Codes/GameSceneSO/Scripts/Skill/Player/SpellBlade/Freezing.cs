using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freezing", menuName = "SKill/Player/SpellBlade/StatusAttackSkill/Freezing")]
public class Freezing : StatusAttackSkillSO
{
    [SerializeField] private Move move;
    [SerializeField] private Freeze freeze;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(move, true, false);

        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(freeze.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(freeze, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(freeze, false);
        }
    }
}
