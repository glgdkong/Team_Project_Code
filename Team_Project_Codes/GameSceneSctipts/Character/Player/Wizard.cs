using System.Collections;

public class Wizard : PlayableCharacter
{
    protected override IEnumerator TurnActionCoroutine()
    {
        PassiveSkill();
        yield return null;
        BattleManager.Instance.PlayerActionManager.PlayerTurn(this);


        while (!playerAction)
        {
            yield return null;
            //yield return null;
        }
    }

    protected override void PassiveSkill()
    {
        HealMana(5);
    }
}
