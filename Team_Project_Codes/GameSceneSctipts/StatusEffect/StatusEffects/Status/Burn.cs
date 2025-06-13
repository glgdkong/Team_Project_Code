using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Burn : StatusEffect
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Burn;
    public override StatusType StatusType => StatusType.DotDamage; 
    [SerializeField, Header("도트 데미지")] private int damage;
    public int Damage { get => damage; set => damage = value; }
    public Burn(Burn other)
    {
        count = other.count;
        damage = other.isCriticalHit ? (other.damage * 2) : other.damage;
    }

    public override void ActivateEffect(Character character)
    {
        character.Hit(Damage, true, false, HitLog_Type.Burn);
        character.StatusEffectHandler.ParticleHandler?.PlayParticle(StatusEffectType);
        count--;
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Burn burn)
        {
            Burn addEffect = new Burn(burn);
            Burn currentEffects = (Burn)statusEffectHandler.GetSpecificEffect(addEffect.StatusEffectType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
                currentEffects.Damage = currentEffects.Damage > addEffect.damage ? currentEffects.Damage : addEffect.Damage;
            }
            else
            {
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
    }

    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#FE332E>{damage}</color>만큼의 <color=#BF2D00>화상</color> 데미지를 받습니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#BF2D00>화상</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#BF2D00>화상</color> <color=#B7C5CD>저항</color>";
    }


}
