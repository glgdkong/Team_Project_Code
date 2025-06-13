using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Slow : DebuffEffect
{
    public override DebuffType DebuffType => DebuffType.Slow;
    private int baseSpeed;

    public Slow(Slow other)
    {
        count = other.count;
    }
    public Slow(int count)
    {
        this.count = count;
    }

    public override void ActivateEffect(Character character)
    {
        count--;
        if(count <= 0)
        {
            character.CurrentSpeed = character.CharacterInfo.CharacterSpd;
        }
    }
    public override void AddEffect(StatusEffectBase effectBase, StatusEffectHandler statusEffectHandler)
    {
        if (effectBase is Slow slow)
        {
            Slow addEffect = new Slow(slow);
            Slow currentEffects = (Slow)statusEffectHandler.GetSpecificEffect(addEffect.DebuffType);
            if (currentEffects != null)
            {
                currentEffects.Count += addEffect.Count;
            }
            else
            {
                baseSpeed = statusEffectHandler.Character.CurrentSpeed;
                statusEffectHandler.Character.CurrentSpeed -= 1;
                statusEffectHandler.RegisterNewEffect(addEffect);
            }
        }
    }

    public override string StatusInfo()
    {
        return $"<color=#FFD500>{count}</color>턴 동안 <color=#5EA152>속도</color>가 1 감소됩니다.";
    }
    public override string ShowGrantSuccessMessage()
    {
        return "<color=#709AD1>둔화</color> <color=#FFB74D>부여</color>";
    }

    public override string ShowGrantFailureMessage()
    {
        return "<color=#709AD1>둔화</color> <color=#B7C5CD>저항</color>";
    }
}
