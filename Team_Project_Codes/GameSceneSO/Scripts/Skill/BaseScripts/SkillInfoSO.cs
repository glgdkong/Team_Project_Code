using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillType
{
    Attack,
    Heal,
    Buff
}
public enum SkillRangeType
{
    Single,
    Area,
    Self
}
[System.Serializable]
public class SkillEffectInfo 
{
    [SerializeField] private Vector3 effectSpawnPos = new Vector3();
    public Vector3 EffectSpawnPos => effectSpawnPos;
}
public abstract class SkillInfoSO : ScriptableObject
{
    [SerializeField, Header("스킬 기본 이미지")] protected Sprite skillEImage;
    [SerializeField, Header("스킬 비활성화 이미지")] protected Sprite skillDImage;
    [SerializeField, Header("스킬 이름")] protected string skillName;
    [SerializeField, Header("스킬 아이디")] protected int skillId;
    [SerializeField, Header("스킬 애니메이션 클립")] protected AnimationClip skillAnimtionClip;
    [SerializeField,Header("스킬 타입")] protected SkillType skillType;
    [SerializeField,Header("스킬 범위 타입")] protected SkillRangeType skillRangeType;
    [SerializeField, Header("스킬 수치")] protected Vector2 impactValue;
    [SerializeField, Header("마나 소모량")] protected int manaCost;
    [SerializeField, Header("스킬 치명타율"), Range(0f, 2f)] protected float critChance;
    [SerializeField, Header("스킬 설명"), TextArea] protected string skillTooltip;
    [SerializeField, Header("스킬 이펙트")] protected GameObject skillEffect;
    [SerializeField, Header("스킬 이펙트 정보")] protected SkillEffectInfo skillEffectInfo;
    [SerializeField, Header("스킬 사용가능 자리")] protected bool[] canUsePos = new bool[4];
    
    public Sprite SkillEImage => skillEImage;
    public Sprite SkillDImage => skillDImage;
    public string SkillName => skillName;
    public int SkillId => skillId;
    public AnimationClip SkillAnimationClip => skillAnimtionClip;
    public SkillType SkillType => skillType;
    public SkillRangeType SkillRangeType => skillRangeType;
    public Vector2 ImpactValue => impactValue; 
    public int ManaCost => manaCost;
    public float CritChance => critChance;
    public string SkillToolTip => skillTooltip; 
    public GameObject SkillEffect => skillEffect;
    public SkillEffectInfo SkillEffectInfo => skillEffectInfo;
    public bool[] CanUsePos => canUsePos;

    public virtual void ApplySkillEffect(Character casterCharacter, Character targetCharacter, bool isCriticalHit = false, bool hitSuccess = true)
    {

    }
}
