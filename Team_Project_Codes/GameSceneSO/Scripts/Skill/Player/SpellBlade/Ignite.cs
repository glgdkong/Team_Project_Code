using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ignite", menuName = "SKill/Player/SpellBlade/StatusAttackSkill/Ignite")]
public class Ignite : StatusAttackSkillSO
{
    [SerializeField] private Move move;
    [SerializeField] private Burn burn;
    [SerializeField] private DefDown defDown;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(move, true, false);
        
        if (targetCharacter.IsDeath || !hitSuccess) { return; }
        chance = Random.value;
        if (chance < CalcEffectChance(burn.StatusEffectType, targetCharacter, isCriticalHit))
        {
            burn.IsCriticalHit = isCriticalHit;
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(burn, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(burn, false);
        }
        chance = Random.value;
        if (chance < CalcEffectChance(defDown.StatusEffectType, targetCharacter, isCriticalHit))
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, true);
        }
        else
        {
            targetCharacter.StatusEffectHandler.UpdateOrAddEffect(defDown, false);
        }
    }
}
