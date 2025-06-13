using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untouchable", menuName = "SKill/Player/SpellBlade/BuffSkill/Untouchable")]
public class Untouchable : BuffSkillSO
{
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        targetCharacter.StatusEffectHandler.DeleteStatusEffect(StatusType.DotDamage);
    }
}
