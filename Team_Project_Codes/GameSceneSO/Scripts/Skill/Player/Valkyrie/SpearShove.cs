using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpearShove", menuName = "SKill/Player/Valkyrie/AttackSkill/SpearShove")]
public class SpearShove : StatusAttackSkillSO
{
    [SerializeField] private Move move;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(move.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(move, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(move, false);
        }
    }
}
