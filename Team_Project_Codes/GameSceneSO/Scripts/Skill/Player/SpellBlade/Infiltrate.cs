using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Infiltrate", menuName = "SKill/Player/SpellBlade/AttackSkill/Infiltrate")]
public class Infiltrate : AttackSkillSO
{
    [SerializeField] private Move move;

    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(move, true, false);
    }
}
