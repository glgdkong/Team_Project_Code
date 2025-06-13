using System.Collections;
using UnityEngine;

public class SkillUseAnimtionController : MonoBehaviour
{
    private AnimationHandler animationHandler;
    private Character character;

    private Character skillTargetCharcter;
    private SkillInfoSO selectedSkill;


    private void Start()
    {
        character = GetComponentInParent<Character>();
        animationHandler = GetComponentInParent<AnimationHandler>();
    }

    public void GetInfos(Character skillTargetCharcter, SkillInfoSO selectedSkill)
    {
        this.skillTargetCharcter = skillTargetCharcter;
        this.selectedSkill = selectedSkill;

    }

    public void SkillUse()
    {
        StartCoroutine(AnimationDelay());
    }

    private IEnumerator AnimationDelay()
    {
        BattleManager.Instance.SkillUsage(selectedSkill, character, skillTargetCharcter);
        float originalSpeed = animationHandler.Animator.speed;
        animationHandler.Animator.speed = 0;
        yield return animationHandler.Delay;
        animationHandler.Animator.speed = originalSpeed;
        BattleManager.Instance.PlayerActionManager.PlayerActionUI.ShowSkillName(null);
    }
}
