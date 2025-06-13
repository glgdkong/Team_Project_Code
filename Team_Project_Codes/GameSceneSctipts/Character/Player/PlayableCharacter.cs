using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : Character
{
    [SerializeField] protected int currentMana;
    protected PlayerCharacterUI playerCharacterUI;
    protected bool playerAction = false;
    public PlayerCharacterUI PlayerCharacterUI => playerCharacterUI;


    public int CurrentMana { get => currentMana; set => currentMana = value; }
    public bool PlayerAction { get => playerAction; set => playerAction = value; }
    public PlayableCharacterSO PlayableCharacterSO => (PlayableCharacterSO)characterInfo;

    protected override void Awake()
    {
        base.Awake();
        playerCharacterUI = GetComponentInChildren<PlayerCharacterUI>();
    }

    protected override void Start()
    {
        if(characterInfo != null)
        {
            SetCharacterInfo(PlayableCharacterSO);
        }
    }
    /*public override IEnumerator TakeTurnCoroutine()
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
                    // 턴 시작 시
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
                    // 턴 엔드 시
                }

            }
        }

        if (!isDeath)
        {
            animationHandler.SwitchCharacterState();
        }
    }*/


    protected override IEnumerator TurnActionCoroutine()
    {
        BattleManager.Instance.PlayerActionManager.PlayerTurn(this);


        while(!playerAction)
        {
            yield return null;
        }
        BattleManager.Instance.PlayerActionManager.ClearPlayerInfo();
    }
    public void SetCharacterInfo(PlayableCharacterSO playableCharacterSO)
    {
        ItemStatType itemStatType = playableCharacterSO.TotalItemStat();
        currentHp = playableCharacterSO.CharacterHp;
        currentMana = playableCharacterSO.Mana;
        currentSpeed = playableCharacterSO.CharacterSpd;
        if(itemStatType != null)
        {
            currentHp += itemStatType.Hp;
            currentMana += itemStatType.Mana;
            currentSpeed += itemStatType.Speed;
        }
        characterInfo = playableCharacterSO;
        hasSkillList.Clear();
        hasSkillList = new List<SkillInfoSO>(playableCharacterSO.SelectedSkillInfos);
    }
    public void PlayerActionEnd()
    {
        playerAction = true;
    }
    public void SetTurnEnd()
    {
        playerAction = true;
        turnEnd = true;
    }
    protected virtual void PassiveSkill(){}
    public override void ResetTurn()
    {
        base.ResetTurn();
        playerAction = false;
    }

    public override void UseSkill(SkillInfoSO skillInfoSO, Character character)
    {
        base.UseSkill(skillInfoSO, character);
        currentMana -= skillInfoSO.ManaCost;
        if (currentMana <= 0) 
        {
            currentMana = 0;
        }
        UpdateMana();
    }

    public override void Hit(int damage, bool isHit, bool hitAnimationPlay, HitLog_Type hitLog_Type)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            BattleManager.Instance.HitLogManager.ShowDamageLog(transform, hitLog_Type, damage.ToString());
            Death();
        }
        else
        {
            if (hitAnimationPlay)
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
        UpdateHpInfo();
    }
    protected override void UpdateHpInfo()
    {
        ItemStatType itemStatType = PlayableCharacterSO.TotalItemStat();
        float amount = (float)currentHp / ((float)characterInfo.CharacterHp + itemStatType.Hp);
        playerCharacterUI.UpdateCharacterInfo(amount);
    }

    public  void UpdateMana()
    {
        ItemStatType itemStatType = PlayableCharacterSO.TotalItemStat();
        float amount = (float)currentMana / ((float)PlayableCharacterSO.Mana+ itemStatType.Mana);
        playerCharacterUI.UpdateMana(amount);
    }

    public override void HealMana(int addMana, bool isCritical = false)
    {
        ItemStatType itemStatType = PlayableCharacterSO.TotalItemStat();
        currentMana += addMana; 
        if (currentMana >= (PlayableCharacterSO.Mana + itemStatType.Mana))
        {
            
            currentMana = (PlayableCharacterSO.Mana + itemStatType.Mana);
        }
        BattleManager.Instance.HitLogManager.ShowDamageLog(transform, isCritical ? HitLog_Type.CriticalManaHeal : HitLog_Type.ManaHeal, addMana.ToString());
        UpdateMana();
    }

    public override void HealHealth(int addHp, bool isCriticalHit = false)
    {
        ItemStatType itemStatType = PlayableCharacterSO.TotalItemStat();
        currentHp += addHp;
        if (currentHp > (characterInfo.CharacterHp + itemStatType.Hp))
        {
            currentHp = characterInfo.CharacterHp + itemStatType.Hp;
        }
        BattleManager.Instance.HitLogManager.ShowDamageLog(transform, isCriticalHit ? HitLog_Type.CriticalHealthHeal : HitLog_Type.HealthHeal, addHp.ToString());
        UpdateHpInfo();
    }
}
