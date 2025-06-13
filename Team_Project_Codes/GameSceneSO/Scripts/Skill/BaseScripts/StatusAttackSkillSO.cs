using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusAttackSkillSO : AttackSkillSO
{
    [SerializeField, Header("상태이상 부여 확률"), Range(0f, 2f)] protected float statusEffectChance;
    protected float chance;

    // 상태이상 부여 확률을 계산하는 메소드
    protected float CalcEffectChance(StatusEffectType statusType, Character target, bool isCriticalHit)
    {
        float totalResistanceUpValue = 0f;
        if(target.StatusEffectHandler.HasSpecificEffect(BuffType.Resistance_Up)) 
        {
            List<ResUp> res = new List<ResUp>();
            foreach (StatusEffectBase item in target.StatusEffectHandler.StatusEffects)
            {
                if ((item is BuffEffect buffEffect && buffEffect.BuffType == BuffType.Resistance_Up))
                {
                    res.Add((ResUp)item);
                }
            }
            foreach (ResUp resistanceUpEffect in res)
            {
                // StatusResistance가 구조체이므로 기본 값 체크
                var resistance = resistanceUpEffect.StatusResistances.Find(res => res.StatusType == statusType);

                // 기본 값과 비교
                if (!EqualityComparer<StatusResistance>.Default.Equals(resistance, default(StatusResistance)))
                {
                    totalResistanceUpValue += resistance.ResistanceValue;
                }
                else
                {
                    // 해당 상태 타입에 대한 저항 값이 없는 경우
                    //Debug.LogWarning("해당 상태 타입에 대한 저항 값이 없습니다.");
                }
            }
        }

        totalResistanceUpValue = totalResistanceUpValue > 1f ? 1f : totalResistanceUpValue;

        if (target is PlayableCharacter targetPlayableCharacter)
        {
            ItemStatType itemStatType = targetPlayableCharacter.PlayableCharacterSO.TotalItemStat();
            if (itemStatType != null)
            {
                if (itemStatType.ResistanceValues.ContainsKey(statusType))
                {
                    totalResistanceUpValue += itemStatType.ResistanceValues[statusType];
                }
            }
        }

        float totalResistanceValue = target.CharacterInfo.Resistances[(int)statusType].ResistanceValue + totalResistanceUpValue;
        float addStatusEffectChance = isCriticalHit ? statusEffectChance * 2 : statusEffectChance;
        return addStatusEffectChance - totalResistanceValue;
    }
}
