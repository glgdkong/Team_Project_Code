using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance => instance;

    private TargetManager targetManager;
    private PlayerActionManager playerActionManager;
    private TurnManager turnManager;
    private SeatManager seatManager;
    private HitLogManager hitLogManager;
    private EffectManager effectManager;
    
    public PlayerActionManager PlayerActionManager => playerActionManager; 
    public TargetManager TargetManager  => targetManager; 
    public TurnManager TurnManager => turnManager;
    public SeatManager SeatManager => seatManager;
    public HitLogManager HitLogManager => hitLogManager;
    public EffectManager EffectManager => effectManager;

    [SerializeField] private List<PlayableCharacter> playerCharacterList = new List<PlayableCharacter>();
    [SerializeField] private List<EnemyCharacter> enemyCharacterList = new List<EnemyCharacter>();
    [SerializeField] private Transform playerParentObject;
    [SerializeField] private Transform enemyParentObject;
    public List<PlayableCharacter> PlayerCharacterList { get => playerCharacterList; set => playerCharacterList = value; }
    public List<EnemyCharacter> EnemyCharacterList { get => enemyCharacterList; set => enemyCharacterList = value; }

    private const float baseHitChance = 0.05f;

    [SerializeField] private List<Character> deadCharacters = new List<Character>();
    public List<Character> DeadCharacters => deadCharacters;


    private void Awake()
    {
        instance = this;
        
        targetManager = GetComponentInChildren<TargetManager>();
        playerActionManager = GetComponentInChildren<PlayerActionManager>();
        turnManager = GetComponentInChildren<TurnManager>();
        seatManager = GetComponentInChildren<SeatManager>();
        hitLogManager = GetComponentInChildren<HitLogManager>();
        effectManager = GetComponentInChildren<EffectManager>();
    }
    private IEnumerator Start()
    {
        InitializePositions(CharacterLists.playableCharacterSOs);
        InitializePositions(CharacterLists.enemyCharacterSO);
        seatManager.InitializePositions(playerCharacterList);
        seatManager.InitializePositions(enemyCharacterList);
        yield return new WaitForEndOfFrame();
        turnManager.GameStart();

    }
    public void InitializePositions<T>(List<T> characterInfos) where T : CharacterInfoSO
    {
        foreach (CharacterInfoSO characterInfo in characterInfos) 
        {
            switch (characterInfo.CharacterType)
            {
                case CharacterType.Player:
                    PlayableCharacter playableCharacter = Instantiate(characterInfo.CharacterPrefab, playerParentObject).GetComponent<PlayableCharacter>();
                    playableCharacter.SetCharacterInfo((PlayableCharacterSO)characterInfo);
                    playerCharacterList.Add(playableCharacter);
                    turnManager.Characters.Add(playableCharacter);
                    break;
                case CharacterType.Enemy:
                    EnemyCharacter enemyCharacter = Instantiate(characterInfo.CharacterPrefab, enemyParentObject).GetComponent<EnemyCharacter>();
                    enemyCharacter.SetCharacterInfo((EnemyCharacterSO)characterInfo.Clone());
                    enemyCharacterList.Add(enemyCharacter);
                    turnManager.Characters.Add(enemyCharacter);
                    break;
                default:
                    break;
            }
        }
        
    }


    public bool SkillUsable(PlayableCharacter playableCharacter, SkillInfoSO skillInfoSO, Character targetCharacter)
    {
        // 현재 턴을 진행하는 캐릭터와 대상 캐릭터의 진영이 같은지 여부를 판단
        bool isSameFaction = playableCharacter.CharacterInfo.CharacterType == targetCharacter.CharacterInfo.CharacterType;

        switch (skillInfoSO.SkillType)
        {
            case SkillType.Attack:
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;
                if (attackSkillSO.SkillRangeType == SkillRangeType.Self)
                {
                    return false;
                }
                // 공격 스킬 사용 조건:
                // 1. 같은 진영이면 공격 불가 (false)
                // 2. 해당 위치의 타겟이 유효하지 않으면 공격 불가 (false)
                // 3. 둘 다 아니면 공격 가능 (true)
                return !isSameFaction && attackSkillSO.TargetPositions[targetCharacter.CurrentPositionIndex];

            case SkillType.Heal:
            case SkillType.Buff:
                // 힐과 버프 스킬은 같은 진영에게만 사용 가능switch (skillInfoSO.SkillRangeType)
                switch (skillInfoSO.SkillRangeType)
                {
                    case SkillRangeType.Single:
                    case SkillRangeType.Area:
                        return isSameFaction;
                    case SkillRangeType.Self:
                        return isSameFaction && targetCharacter == playableCharacter;
                    default:
                        Debug.Log("범위 타입 지정 오류");
                        return false;
                }
        }
        // 정의되지 않은 스킬 타입이 들어오면 오류 로그 출력 후 실패 처리
        Debug.Log("스킬 정보에 오류");
        return false;
    }
    public void SkillUsage(SkillInfoSO skillInfoSO, Character casterCharacter, Character targetCharacter) 
    {
        List<Character> targetCharacterList;
        switch (targetCharacter.CharacterInfo.CharacterType)
        {
            case CharacterType.Player:
                targetCharacterList = new List<Character>(playerCharacterList);
                break;
            case CharacterType.Enemy:
                targetCharacterList = new List<Character>(enemyCharacterList);
                break;
            default:
                Debug.Log("캐릭터 타입 오류");
                return;
        }
        switch (skillInfoSO.SkillType)
        {
            case SkillType.Attack:
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;
                Attack(attackSkillSO, casterCharacter, targetCharacter, targetCharacterList);
                break;
            case SkillType.Heal:
                HealSkillSO healSkillSO = (HealSkillSO)skillInfoSO;
                Heal(healSkillSO, casterCharacter, targetCharacter, targetCharacterList);
                break;
            case SkillType.Buff:
                BuffSkillSO buffSkillSO = (BuffSkillSO)skillInfoSO;
                Buff(buffSkillSO, casterCharacter, targetCharacter, targetCharacterList);
                break;
            default:
                Debug.Log("스킬 정보에 오류");
                return;
        }
    }
    private void Attack(AttackSkillSO attackSkillSO, Character casterCharacter, Character targetCharacter, List<Character> targetCharacterList)
    {
        bool isHit = false;
        switch (attackSkillSO.SkillRangeType)
        {
            case SkillRangeType.Single:
                HandleAttack(casterCharacter, targetCharacter, attackSkillSO, out isHit);
                break;

            case SkillRangeType.Area:
                int targetCount = Mathf.Min(attackSkillSO.TargetPositions.Length, targetCharacterList.Count);
                for (int i = 0; i < targetCount; i++)
                {
                    if (attackSkillSO.TargetPositions[i])
                    {
                        HandleAttack(casterCharacter, targetCharacterList[i], attackSkillSO, out isHit);
                    }
                }
                break;
            default:
                Debug.LogWarning("스킬의 범위 타입이 맞지 않음");
                isHit = false;
                break;
        }
        SoundManager._SoundManager?.SkillUseSound(casterCharacter.CharacterInfo, attackSkillSO.SkillId, isHit);
    }

    private void HandleAttack(Character casterCharacter, Character targetCharacter, AttackSkillSO attackSkillSO, out bool isHit)
    {
        //bool hitSuccess;
        bool isCriticalHit;
        CalculateAccuracy(casterCharacter, targetCharacter, attackSkillSO, out isHit);

        if (isHit)
        {
            Vector2 damageRange = ComputeSkillDamage(casterCharacter, attackSkillSO, targetCharacter);
            isCriticalHit = CheckCriticalHit(casterCharacter, attackSkillSO);
            float damage = CalculateDamage(damageRange, isCriticalHit);

            targetCharacter.Hit(Mathf.FloorToInt(damage + 0.5f), true, true, isCriticalHit ? HitLog_Type.CriticalHit : HitLog_Type.NormalHit);
            effectManager.SetSkillEffect(attackSkillSO, targetCharacter);
            attackSkillSO.ApplySkillEffect(casterCharacter, targetCharacter, isCriticalHit, isHit);
        }
        else
        {
            targetCharacter.Hit(0, false, true, HitLog_Type.Dodge);
            effectManager.SetSkillEffect(attackSkillSO, targetCharacter, false);
            attackSkillSO.ApplySkillEffect(casterCharacter, targetCharacter, hitSuccess: isHit);
        }
    }

    private void CalculateAccuracy(Character casterCharacter, Character targetCharacter, AttackSkillSO attackSkillSO, out bool hitSuccess)
    {
        // 회피 확률 계산 (기본값 + 버프 값)
        float targetCharTotalEvd = targetCharacter.CharacterInfo.CharacterEvd + targetCharacter.StatusEffectHandler.EvdB;
        
        // 타겟이 플레이어일 경우
        if (targetCharacter is PlayableCharacter targetPlayableCharacter)
        {
            ItemStatType itemStatType = targetPlayableCharacter.PlayableCharacterSO.TotalItemStat();
            if (itemStatType != null)
            {
                // 아이템 스탯값 만큼 회피 확률 증가
                targetCharTotalEvd += itemStatType.Evade;
            }
        }
        // 타겟이 기절, 감전, 빙결일 경우 회피 불가
        if(targetCharacter.StatusEffectHandler.HasSpecificEffect(StatusType.TurnSkip)
            || targetCharacter.StatusEffectHandler.HasSpecificEffect(StatusType.TurnDealy))
        {
            //Debug.Log("기절 또는 감전 또는 빙결 상태라 회피 불가");
            targetCharTotalEvd -= targetCharTotalEvd;
        }
        // 명중률 계산 (캐릭터 기본 명중률 - 대상의 회피값) + 기본 명중값
        float totalAcc = (casterCharacter.CharacterInfo.CharacterAcc - targetCharTotalEvd) + baseHitChance;
        // 공격자가 플레이어일 경우
        if (casterCharacter is PlayableCharacter casterPlayableCharacter)
        {
            ItemStatType itemStatType = casterPlayableCharacter.PlayableCharacterSO.TotalItemStat();
            if (itemStatType != null)
            {
                // 아이템 스텟값 만큼 명중률 증가
                totalAcc += itemStatType.Accuracy;
            }
        }
        hitSuccess = Random.value <= totalAcc;
    }

    private bool CheckCriticalHit(Character casterCharacter, AttackSkillSO attackSkillSO)
    {
        float totalCritChance = attackSkillSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
        if (casterCharacter is PlayableCharacter targetPlayableCharacter)
        {
            ItemStatType itemStatType = targetPlayableCharacter.PlayableCharacterSO.TotalItemStat();
            if (itemStatType != null)
            {
                totalCritChance += itemStatType.Critical;
            }
        }
        return Random.value <= totalCritChance;
    }

    private float CalculateDamage(Vector2 damageRange, bool isCriticalHit)
    {
        int damage = isCriticalHit ? (int)damageRange.y : Random.Range((int)damageRange.x, (int)damageRange.y + 1);

        return damage * (isCriticalHit ? 1.5f : 1f);
    }

    public Vector2 ComputeSkillDamage(Character attacker, SkillInfoSO skillInfoSO, Character target)
    {
        Vector2 damage = skillInfoSO.ImpactValue;

        float attackMultiplier = 1;
        if (attacker is PlayableCharacter attackerPlayableCharacter) 
        {
            ItemStatType itemStatType = attackerPlayableCharacter.PlayableCharacterSO.TotalItemStat();
            if (itemStatType != null)
            {
                attackMultiplier += itemStatType.Attack;
            }
        }
        attackMultiplier += attacker.StatusEffectHandler.AtkB;
        //attackMultiplier += attacker.StatusEffectHandler.HasSpecificEffect(DebuffType.ATK_Down) ? -0.25f : 0;

        damage *= attackMultiplier;
        int totalDef = target.CharacterInfo.CharacterDef;
        if (target is PlayableCharacter targetPlayableCharacter)
        {
            ItemStatType itemStatType = targetPlayableCharacter.PlayableCharacterSO.TotalItemStat();
            if (itemStatType != null)
            {
                attackMultiplier += itemStatType.Defense;
            }
        }
        totalDef += target.StatusEffectHandler.DefB;
        //totalDef += target.StatusEffectHandler.HasSpecificEffect(DebuffType.DEF_Down) ? -25 : 0;

        damage *= (100f - totalDef) / 100f;
        damage.x = Mathf.Max(0, Mathf.FloorToInt(damage.x + 0.5f));
        damage.y = Mathf.Max(0, Mathf.FloorToInt(damage.y + 0.5f));
        return damage;
    }


    private void Heal(HealSkillSO skillInfoSO, Character casterCharacter, Character targetCharacter, List<Character> targetCharacterList)
    {
        float value;
        float totalCritChance;
        bool isCriticalHit;
        switch (skillInfoSO.SkillRangeType)
        {
            case SkillRangeType.Single:
                value = Random.value;
                totalCritChance = skillInfoSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
                isCriticalHit = value < totalCritChance;
                skillInfoSO.ApplySkillEffect(casterCharacter, targetCharacter, isCriticalHit);
                effectManager.SetSkillEffect(skillInfoSO, targetCharacter);


                break;
            case SkillRangeType.Area:
                foreach (Character character in targetCharacterList)
                {
                    value = Random.value;
                    totalCritChance = skillInfoSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
                    isCriticalHit = value < totalCritChance;
                    skillInfoSO.ApplySkillEffect(casterCharacter, character, isCriticalHit);
                    effectManager.SetSkillEffect(skillInfoSO, character);
                }

                    break;
            case SkillRangeType.Self:
                value = Random.value;
                totalCritChance = skillInfoSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
                isCriticalHit = value < totalCritChance;
                skillInfoSO.ApplySkillEffect(casterCharacter, targetCharacter, isCriticalHit);
                effectManager.SetSkillEffect(skillInfoSO, targetCharacter);
                break;
            default:
                Debug.LogWarning("스킬의 범위 타입이 맞지 않음");
                break;
        }
        SoundManager._SoundManager?.SkillUseSound(casterCharacter.CharacterInfo, skillInfoSO.SkillId);
    }
    private void Buff(BuffSkillSO skillInfoSO, Character casterCharacter, Character targetCharacter, List<Character> targetCharacterList)
    {
        float value;
        float totalCritChance;
        bool isCriticalHit;
        switch (skillInfoSO.SkillRangeType)
        {
            case SkillRangeType.Single:
                value = Random.value;
                totalCritChance = skillInfoSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
                isCriticalHit = value < totalCritChance;
                skillInfoSO.ApplySkillEffect(casterCharacter, targetCharacter, isCriticalHit);
                effectManager.SetSkillEffect(skillInfoSO, targetCharacter);

                break;
            case SkillRangeType.Area:
                foreach (Character character in targetCharacterList)
                {
                    value = Random.value;
                    totalCritChance = skillInfoSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
                    isCriticalHit = value < totalCritChance;
                    skillInfoSO.ApplySkillEffect(casterCharacter, character, isCriticalHit);
                    effectManager.SetSkillEffect(skillInfoSO, character);
                }

                break;
            case SkillRangeType.Self:
                value = Random.value;
                totalCritChance = skillInfoSO.CritChance + casterCharacter.StatusEffectHandler.CritB;
                isCriticalHit = value < totalCritChance;
                skillInfoSO.ApplySkillEffect(casterCharacter, targetCharacter, isCriticalHit);
                effectManager.SetSkillEffect(skillInfoSO, targetCharacter);
                break;
            default:
                Debug.LogWarning("스킬의 범위 타입이 맞지 않음");
                break;
        }
        SoundManager._SoundManager?.SkillUseSound(casterCharacter.CharacterInfo, skillInfoSO.SkillId);
    }
    // 캐릭터 사망처리 호출
    public void CharacterDeath(Character character)
    {
        // 사망시 턴 캐릭터 리스트에서 제거
        turnManager.Characters.Remove(character);
        if (turnManager.TurnQueue.Contains(character)) // 다음턴중에 해당 컴포넌트가 있다면
        {
            // 턴에서 제거
            turnManager.TurnQueue.Remove(character);
            turnManager.TurnUI.DisableCharacterPortrait(character);
        }


        switch (character.CharacterInfo.CharacterType)
        {
            case CharacterType.Player:
                
                playerCharacterList.Remove((PlayableCharacter)character);
                seatManager.CharacterDeath((PlayableCharacter)character);
                break;
            case CharacterType.Enemy:
                enemyCharacterList.Remove((EnemyCharacter)character);
                seatManager.CharacterDeath((EnemyCharacter)character);
                break;
            default:
                Debug.LogWarning("캐릭터 타입 오류");
                break;
        }
        character.gameObject.SetActive(false);

    }
    public void AddDeathList(Character character)
    {
        deadCharacters.Add(character);
    }
    public void RemoveDeathList(Character character)
    {
        deadCharacters.Remove(character);
    }
    public void DelayEffect(Character character)
    {
        turnManager.TurnQueue.AddLast(character);
        turnManager.TurnUI.AddTurnUI(character, true);
    }

    
}
