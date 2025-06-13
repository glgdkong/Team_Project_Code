using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin_Crash", menuName = "SKill/Enemy/BossSkill/AttackSkill/Goblin_Crash")]
public class Goblin_Crash : StatusAttackSkillSO
{
    [SerializeField] private DefDown defDown;
    [SerializeField] private Move move;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(defDown.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, false);
        }
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
