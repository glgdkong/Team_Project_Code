using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCharacter : Character
{
    protected WaitForSeconds seconds = new WaitForSeconds(0.5f);
    protected EnemyCharacterUI enemyCharacterUI;
    protected List<SkillInfoSO> usableSkillList = new List<SkillInfoSO>();
    protected List<SkillInfoSO> disabledSkillList = new List<SkillInfoSO>();

    public EnemyCharacterUI EnemyCharacterUI => enemyCharacterUI;
    protected override void Awake()
    {
        base.Awake();
        enemyCharacterUI = GetComponentInChildren<EnemyCharacterUI>();
    }
    protected override void Start()
    {
        base.Start();
        hasSkillList = characterInfo.SkillInfoSOs;   
    }
    public void SetCharacterInfo(EnemyCharacterSO enemyCharacterSO)
    {
        characterInfo = enemyCharacterSO;
        hasSkillList = enemyCharacterSO.SkillInfoSOs;
    }

    public override void Hit(int damage, bool isHit, bool hitAnimationPlay, HitLog_Type hitLog_Type)
    {
        base.Hit(damage, isHit, hitAnimationPlay, hitLog_Type);
        BattleManager.Instance.TargetManager.TargetUIManager.UpdateInfo(this);
        UpdateHpInfo();
    }
    protected override void UpdateHpInfo()
    {
        float amount = (float)currentHp / (float)characterInfo.CharacterHp;
        enemyCharacterUI.UpdateCharacterInfo(amount);
    }
    protected override IEnumerator TurnActionCoroutine()
    {
        usableSkillList.Clear(); // 사용 가능한 스킬리스트 초기화
        disabledSkillList.Clear();
        // 도발 걸린 캐릭터를 찾는 리스트 추출
        List<PlayableCharacter> tauntCharacter = GetTauntedCharacters();
        yield return null;
        // 도발 캐릭터가 있다면
        if (tauntCharacter.Count > 0)
        {
            HandleTaunt(tauntCharacter); // 도발 처리
        }
        else
        {
            HandleNormalTurn();
        }
    }

    private void HandleTaunt(List<PlayableCharacter> tauntCharacter)
    {
        // 도발 캐릭터 리스트 인덱스 추첨
        int tauntIndex = Random.Range(0, tauntCharacter.Count);

        // 공격 가능한 스킬 추가
        foreach (SkillInfoSO skillInfoSO in hasSkillList)
        {
            if (skillInfoSO.SkillType == SkillType.Attack)
            {
                AttackSkillSO attackSkillSO = (AttackSkillSO)skillInfoSO;
                if (attackSkillSO.CanUsePos[currentPositionIndex] && attackSkillSO.TargetPositions[tauntCharacter[tauntIndex].CurrentPositionIndex])
                {
                    usableSkillList.Add(attackSkillSO); // 도발된 캐릭터를 공격할 수 있는 스킬 추가
                }
            }
        }

        // 도발 공격 가능한 스킬이 있다면
        if (usableSkillList.Count > 0)
        {
            int index = Random.Range(0, usableSkillList.Count); // 스킬 추첨
            UseSkill(usableSkillList[index], tauntCharacter[tauntIndex]);
        }
        else
        {
            HandleNormalTurn(); // 사용 가능한 스킬이 없다면 일반 턴 처리
        }
    }
    private void HandleNormalTurn()
    {
        usableSkillList.Clear();
        foreach (SkillInfoSO skillInfoSO in hasSkillList)
        {
            if (skillInfoSO.CanUsePos[currentPositionIndex])
            {
                usableSkillList.Add(skillInfoSO); // 사용 가능한 스킬 리스트에 추가
            }
        }
        if(disabledSkillList.Count > 0)
        {
            for (int i = 0; i < disabledSkillList.Count; i++)
            {
                usableSkillList.Remove(disabledSkillList[i]);
            }
        }
        else
        {
            //Debug.Log("사용불가 스킬 없음");
        }

        if (usableSkillList.Count > 0)
        {
            SkillInfoSO skill = usableSkillList[Random.Range(0, usableSkillList.Count)]; // 랜덤 스킬 추첨
            switch (skill.SkillType)
            {
                case SkillType.Attack:
                    AttackSkillSO attackSkillSO = (AttackSkillSO)skill;
                    // targetPositions 배열에서 true 값의 인덱스를 찾아 리스트로 만듦
                    List<PlayableCharacter> validTargets = new List<PlayableCharacter>();

                    int count = Mathf.Min(BattleManager.Instance.PlayerCharacterList.Count, attackSkillSO.TargetPositions.Length);
                    for (int i = 0; i < count; i++)
                    {
                        if (attackSkillSO.TargetPositions[i]) // true인 값만
                        {
                            validTargets.Add(BattleManager.Instance.PlayerCharacterList[i]);
                        }
                    }

                    // validTargets 리스트에서 랜덤으로 타겟 추첨
                    if (validTargets.Count > 0)
                    {
                        UseSkill(attackSkillSO, validTargets[Random.Range(0, validTargets.Count)]);
                    }
                    else
                    {
                        //Debug.Log("스킬 타겟없음 사용불가처리 및 재추첨");
                        disabledSkillList.Add(skill);
                        HandleNormalTurn();

                    }
                    break;
                case SkillType.Heal:
                case SkillType.Buff:
                    switch (skill.SkillRangeType)
                    {
                        case SkillRangeType.Single:
                        case SkillRangeType.Area:
                            UseSkill(skill, BattleManager.Instance.EnemyCharacterList[Random.Range(0, BattleManager.Instance.EnemyCharacterList.Count)]);
                            break;
                        case SkillRangeType.Self:
                            UseSkill(skill ,this);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            turnEnd = true;
        }
    }



    private List<PlayableCharacter> GetTauntedCharacters()
    {
        List<PlayableCharacter> tauntCharacter = new List<PlayableCharacter>();
        foreach (PlayableCharacter playableCharacter in BattleManager.Instance.PlayerCharacterList)
        {
            if (playableCharacter.StatusEffectHandler.HasSpecificEffect(DebuffType.Taunt))
            {
                tauntCharacter.Add(playableCharacter);
            }
        }
        return tauntCharacter;
    }
}
