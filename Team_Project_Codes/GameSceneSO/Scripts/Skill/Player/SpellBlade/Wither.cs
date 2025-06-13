using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wither", menuName = "SKill/Player/SpellBlade/StatusAttackSkill/Wither")]
public class Wither : StatusAttackSkillSO
{
    [SerializeField] private Move move;
    [SerializeField] private AtkDown atkDown;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(move, true, false);

        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(atkDown.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkDown, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkDown, false);
        }
    }
}