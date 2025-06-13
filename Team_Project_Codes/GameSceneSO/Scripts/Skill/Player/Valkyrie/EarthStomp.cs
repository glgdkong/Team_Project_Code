using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthStomp", menuName = "SKill/Player/Valkyrie/AttackSkill/EarthStomp")]
public class EarthStomp : AttackSkillSO
{
    [SerializeField] private Move move;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        casterCharacter.StatusEffectHandler.UpdateOrAddEffect(move, true, false);
    }
}
