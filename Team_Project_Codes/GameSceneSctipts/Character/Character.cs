using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterInfoSO characterInfo;
    // 턴을 정할 변수
    [SerializeField] protected float currentTurnSpeed;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int currentPositionIndex;  // 캐릭터의 현재 위치 인덱스
    [SerializeField] protected List<SkillInfoSO> hasSkillList; // 현재 가지고 있는 스킬들
    [SerializeField] protected int currentSpeed;
    
    protected StatusEffectHandler statusEffectHandler; // 상태이상 관리 컴포넌트
    protected AnimationHandler animationHandler; // 애니메이션 핸들러 참조
    protected SkillUseAnimtionController skillUseAnimtionController;

    // 턴 종료 여부
    protected bool turnEnd = false;
    // 사망 여부
    protected bool isDeath = false;
    protected WaitForSeconds delay = new WaitForSeconds(0.5f);
    protected Coroutine currentCoroutine;


    public bool IsDeath { get => isDeath; }
    public float CurrentTurnSpeed { get => currentTurnSpeed; set => currentTurnSpeed = value; }
    public int CurrentHp { get => currentHp; set => currentHp = value; }
    public int CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public CharacterInfoSO CharacterInfo { get => characterInfo; set => characterInfo = value; }
    public int CurrentPositionIndex { get => currentPositionIndex; set => currentPositionIndex = value; }
    public List<SkillInfoSO> HasSkillList => hasSkillList;
    public bool TurnEnd => turnEnd;
    public StatusEffectHandler StatusEffectHandler { get => statusEffectHandler;}
    public AnimationHandler AnimationHandler => animationHandler;


    protected virtual void Awake()
    {
        statusEffectHandler = GetComponent<StatusEffectHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        skillUseAnimtionController = GetComponentInChildren<SkillUseAnimtionController>();
    }

    protected virtual void Start()
    {
        currentSpeed = characterInfo.CharacterSpd;
        currentHp = characterInfo.CharacterHp;
        // 턴 매니저 캐릭터 리스트에 컴포넌트 추가
        //BattleManager.Instance.TurnManager.Characters.Add(this);
    }

    public virtual IEnumerator TakeTurnCoroutine()
    {
        if (statusEffectHandler.HasSpecificEffect(StatusType.TurnDealy))
        {
            yield return StartCoroutine(statusEffectHandler.ApplyEffect(StatusType.TurnDealy));
            BattleManager.Instance.DelayEffect(this);
            //yield break;
        }
        else
        {
            if (statusEffectHandler.HasSpecificEffect(StatusType.DotDamage))
            {
                yield return StartCoroutine(statusEffectHandler.ApplyEffect(StatusType.DotDamage));
                if (isDeath)
                {
                    yield break;
                }
                else
                {
                    animationHandler.SwitchCharacterState();
                    yield return new WaitForSeconds(0.25f);
                }
            }
            if (!isDeath && statusEffectHandler.HasSpecificEffect(StatusType.TurnSkip))
            {
                if (!isDeath && statusEffectHandler.HasSpecificEffect(EffectCategory.Debuff) || statusEffectHandler.HasSpecificEffect(EffectCategory.Buff))
                {
                    statusEffectHandler.DecreaseCount(EffectCategory.Debuff);
                    statusEffectHandler.DecreaseCount(EffectCategory.Buff);
                }
                yield return StartCoroutine(statusEffectHandler.ApplyEffect(StatusType.TurnSkip));
                //yield break;
            }
            else
            {
                if (!isDeath)
                {
                    yield return StartCoroutine(TurnActionCoroutine()); // 행동 딜레이
                    // 애니메이션이 끝날 때까지 대기
                    yield return new WaitUntil(() =>
                    (animationHandler.Animator.GetCurrentAnimatorStateInfo(0).IsName("SkillUse") && // "SkillUse" 애니메이션이 실행 중인지
                    animationHandler.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) || turnEnd); // 애니메이션이 끝날 때까지
                    if (!isDeath && statusEffectHandler.HasSpecificEffect(EffectCategory.Debuff) || statusEffectHandler.HasSpecificEffect(EffectCategory.Buff))
                    {
                        statusEffectHandler.DecreaseCount(EffectCategory.Debuff);
                        statusEffectHandler.DecreaseCount(EffectCategory.Buff);
                    }
                }

            }
        }

        if (!isDeath)
        {
            animationHandler.SwitchCharacterState();
        }
    }
    
    protected abstract IEnumerator TurnActionCoroutine();

    public virtual void UseSkill(SkillInfoSO skillInfoSO, Character targetCharacter)
    {
        // 현재 애니메이터의 컨트롤러를 가져옴
        skillUseAnimtionController.GetInfos(targetCharacter, skillInfoSO);
        //Debug.Log("스킬 이름 " + skillInfoSO.SkillName);
        BattleManager.Instance.PlayerActionManager.PlayerActionUI.ShowSkillName(skillInfoSO.SkillName);
        animationHandler.SetAnimationClip(skillInfoSO.SkillAnimationClip);
    }

    public virtual void ResetTurn()
    {
        turnEnd = false;
        float speed = statusEffectHandler.HasSpecificEffect(DebuffType.Slow) ? currentSpeed : (currentSpeed + 1);
        currentTurnSpeed = Random.Range((speed - 1), speed);
    }

    
    public virtual void HealHealth(int addHp, bool isCriticalHit = false)
    {
        currentHp += addHp;
        if(currentHp > characterInfo.CharacterHp)
        {
            currentHp = characterInfo.CharacterHp;
        }
        BattleManager.Instance.HitLogManager.ShowDamageLog(transform, isCriticalHit ? HitLog_Type.CriticalHealthHeal : HitLog_Type.HealthHeal, addHp.ToString());
        UpdateHpInfo();
    }
    public virtual void HealMana(int addMana, bool isCritical = false)
    {

    }

    protected virtual void UpdateHpInfo()
    {
        float amount = (float)currentHp / (float)characterInfo.CharacterHp;
    }
    public virtual void Hit(int damage, bool isHit, bool hitAnimationPlay, HitLog_Type hitLog_Type)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            BattleManager.Instance.HitLogManager.ShowDamageLog(transform, hitLog_Type, damage.ToString());
            Death();
        }
        else
        {
            if(hitAnimationPlay)
            {
                if (isHit)
                {
                    animationHandler.PlayHitAnimation(AnimationType.Hit);
                }
                else
                {
                    animationHandler.PlayHitAnimation(AnimationType.Dodge);
                }
            }
            BattleManager.Instance.HitLogManager.ShowDamageLog(transform, hitLog_Type, damage.ToString());
        }
    }

    // 사망처리 메소드
    public virtual void Death()
    {
        isDeath = true;
        BattleManager.Instance.AddDeathList(this);
        //StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }
    protected virtual IEnumerator DeathCoroutine()
    {
        //Debug.Log("사망 코루틴 시작");
        animationHandler.DeathAnimationPlay();
        yield return new WaitUntil(() =>
        (animationHandler.Animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && // "SkillUse" 애니메이션이 실행 중인지
        animationHandler.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)); // 애니메이션이 끝날 때까지
        //Debug.Log("사망 코루틴 엔드");
        BattleManager.Instance.RemoveDeathList(this);
        BattleManager.Instance.CharacterDeath(this);
    }
}
