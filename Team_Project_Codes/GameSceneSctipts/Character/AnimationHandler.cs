using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 현재 캐릭터의 상태를 나타내는 이넘
public enum Character_State
{
    Idle,       // 기본 대기 상태  
    Injured,    // 도트 데미지 받은 상태
    Stun,       // 기절 상태
    Electrocute,// 감전 상태
    Freeze            // 빙결 상태
}
public enum AnimationType
{
    Hit,     // 피격 애니메이션
    Dodge    // 회피 애니메이션
}
//
public class AnimationHandler : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;  // 오버라이드 컨트롤러를 멤버로 선언
    [SerializeField] List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
    protected Character_State currentState = Character_State.Idle;
    protected Character character;
    [SerializeField] private float hit;
    [SerializeField] private float dodge;
    [SerializeField] private float death;
    private StatusEffectParticleHandler particleHandler;

    [SerializeField] private float delayTime;
    [SerializeField] private WaitForSeconds delay;


    private AnimatorStateInfo stateInfo;
    public Character_State CurrentState { get => currentState; set => currentState = value; }
    public Animator Animator => animator;
    public AnimatorStateInfo StateInfo => stateInfo;

    public WaitForSeconds Delay => delay; 

    // 감전 -> 행동불가류(빙결, 기절) -> 도트
    // 감전, 기절, 빙결의 애니메이션은 같은 애니메이션
    // 감전이랑 빙결은 정지한 상태의 애니메이션
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        character = GetComponent<Character>();
        particleHandler = GetComponentInChildren<StatusEffectParticleHandler>();
        delay = new WaitForSeconds(delayTime);
    }
    private void Start()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        RuntimeAnimatorController currentController = animator.runtimeAnimatorController;
        AnimatorOverrideController newoverrideController = (AnimatorOverrideController)(currentController);
        overrideController = new AnimatorOverrideController(currentController);

        // 기존 애니메이션 오버라이드 클립들을 가져오기
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        newoverrideController.GetOverrides(overrides);


        // 변경된 오버라이드 리스트를 적용하여 새로운 AnimatorOverrideController 생성
        //newOverrideController.ApplyOverrides(overrides);
        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }

    public void SwitchAnimation()
    {
        animator.speed = 1;
        int index = 0;
        switch (currentState)
        {
            case Character_State.Idle:
                animator.CrossFade("Idle", 0.1f);
                index = 0;
                break;
            case Character_State.Injured:
                animator.CrossFade("Injured", 0.1f);
                index = 1;
                break;
            case Character_State.Stun:
                animator.CrossFade("Stun", 0.1f);
                index = 2;
                break;
            case Character_State.Electrocute:
            case Character_State.Freeze:
                index = 3;
                animator.speed = 0;
                animator.Play("Static", 0, 0.383f);
                //animator.CrossFade("Static", 0.1f, 0, 0.383f);
                break;
        }

        animator.SetInteger("State", index);
    }

    public void SwitchCharacterState()
    {
        Character_State previousState = currentState;
        if (character.StatusEffectHandler.HasSpecificEffect(StatusEffectType.Freeze))
        {
            currentState = Character_State.Freeze;
        }
        else if (character.StatusEffectHandler.HasSpecificEffect(StatusEffectType.Electrocute))
        {
            currentState = Character_State.Electrocute;
        }
        else if (character.StatusEffectHandler.HasSpecificEffect(StatusEffectType.Stun))
        {
            currentState = Character_State.Stun;
        }
        else if (character.StatusEffectHandler.HasSpecificEffect(StatusType.DotDamage))
        {
            currentState = Character_State.Injured;
        }
        else
        {
            currentState = Character_State.Idle;
        }
        particleHandler?.PlayParticle(previousState, currentState);
        SwitchAnimation();
    }

    public void PlayHitAnimation(AnimationType animationType)
    {
        StartCoroutine(HitAndDodgeAnimationAfterPause(animationType));
    }
    
    private IEnumerator HitAndDodgeAnimationAfterPause(AnimationType animationType)
    {
        switch (currentState)
        {
            case Character_State.Idle:
            case Character_State.Injured:
                switch (animationType)
                {
                    case AnimationType.Hit:
                        animator.Play(animationType.ToString(), 0, hit);
                        break;
                    case AnimationType.Dodge:
                        animator.Play(animationType.ToString(), 0, dodge);
                        break;
                    default:
                        break;
                }
                break;
        }

        animator.speed = 0;
        yield return delay;
        animator.speed = 1;

        yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) || (animator.GetCurrentAnimatorStateInfo(0).IsName("Static")));
        SwitchCharacterState();
    }
    public void DeathAnimationPlay()
    {
        particleHandler?.StopStateParticle(currentState);
        particleHandler?.PlayDeathParticle();
        //Debug.Log("사망 애니메이션 재생");
        StartCoroutine(DeathAnimationPlayCoroutine());
    }
    private IEnumerator DeathAnimationPlayCoroutine()
    {
        animator.speed = 1;
        animator.Play("Death", 0, death);

        animator.speed = 0;
        yield return delay;
        animator.speed = 1f;
    }


    public void SetAnimationClip(AnimationClip newClip)
    {
        // 현재 애니메이터의 컨트롤러 가져오기
        //RuntimeAnimatorController currentController = animator.runtimeAnimatorController;

        // 기존 RuntimeAnimatorController에서 AnimatorOverrideController 생성
        //AnimatorOverrideController overrideController = (AnimatorOverrideController)(currentController);
        //AnimatorOverrideController newOverrideController = new AnimatorOverrideController(currentController);

        // 기존 애니메이션 오버라이드 클립들을 가져오기
        overrideController.GetOverrides(overrides);

        // 새로운 "SkillUse" 애니메이션을 교체
        for (int i = 0; i < overrides.Count; i++)
        {
            // "SkillUse" 애니메이션을 새로운 애니메이션 클립으로 교체
            if (overrides[i].Key.name == "SkillUse")
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newClip);
                break;
            }
        }

        // 변경된 오버라이드 리스트를 적용하여 새로운 AnimatorOverrideController 생성
        //newOverrideController.ApplyOverrides(overrides);
        overrideController.ApplyOverrides(overrides);

        // 애니메이터에 새로 생성된 오버라이드 컨트롤러 적용
        //animator.runtimeAnimatorController = overrideController;

        // "SkillUse" 애니메이션 실행
        animator.CrossFade("SkillUse", 0.2f);
    }

}
