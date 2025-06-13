using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthlyHymn", menuName = "SKill/Player/Druid/BuffSkill/EarthlyHymn")]
public class EarthlyHymn : BuffSkillSO
{
    [SerializeField] private AtkUp atkUp;
    public override void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {
        atkUp.IsCurrentTurnGranted = (casterCharacter == targetCharacter);
        targetCharacter.StatusEffectHandler.UpdateOrAddEffect(atkUp, true);
    }
}
