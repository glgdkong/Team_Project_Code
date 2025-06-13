using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterType
{
    Player,
    Enemy
}
public enum StatusEffectType
{
    Bleed,      // 출혈
    Poison,     // 중독
    Burn,       // 화상
    Stun,       // 기절
    Weakness,   // 약화
    Move,       // 이동
    Freeze,     // 빙결
    Electrocute // 감전
}
[System.Serializable]
public struct StatusResistance
{
    [SerializeField] private string name;
    [SerializeField] private StatusEffectType statusType;
    [SerializeField, Range(0f, 2f)] private float resistanceValue;

    public StatusEffectType StatusType { get => statusType; set => statusType = value; }
    public float ResistanceValue { get => resistanceValue; set => resistanceValue = value; }
    public string Name { get => name; set => name = value; }
}
// 체력,마나, 공격력, 방어력, 속도, 회피, 명중, 지능, 치명타, 상태이상 저항력
// 상태이상 출혈, 중독, 화상, 기절, 약화, 빙결, 감전
public abstract class CharacterInfoSO : ScriptableObject
{
    [SerializeField, Header("캐릭터 타입")] protected CharacterType characterType;
    [SerializeField, Header("캐릭터 아이디")] protected int characterId;
    [SerializeField, Header("캐릭터 이미지")] protected Sprite characterImage;
    [SerializeField, Header("캐릭터 이름")] protected string characterName;
    [SerializeField, Header("캐릭터 체력")] protected int characterHp;
    [SerializeField, Header("캐릭터 속도")] protected int characterSpd;
    [SerializeField, Header("캐릭터 회피"), Range(0f, 2f)] protected float characterEvd;
    [SerializeField, Header("캐릭터 명중"), Range(0f, 2f)] protected float characterAcc;
    [SerializeField, Header("캐릭터 방어력"), Range(0, 200)] protected int characterDef;
    [SerializeField, Header("캐릭터 저항력")] protected StatusResistance[] resistances;
    [SerializeField,Header("스킬 리스트")] protected List<SkillInfoSO> skillInfos;
    [SerializeField, Header("캐릭터 프리펩")] protected GameObject characterPrefab;

    public CharacterType CharacterType { get => characterType; }
    public int CharacterId { get => characterId; }
    public Sprite CharacterImage => characterImage;
    public string CharacterName { get => characterName; set => characterName = value; }
    public int CharacterHp { get => characterHp; set => characterHp = value; }
    public int CharacterSpd { get => characterSpd; set => characterSpd = value; }
    public float CharacterEvd { get => characterEvd; set => characterEvd = value; }
    public float CharacterAcc { get => characterAcc; set => characterAcc = value; }
    public int CharacterDef { get => characterDef; set => characterDef = value; }
    public List<SkillInfoSO> SkillInfoSOs => skillInfos;
    public StatusResistance[] Resistances { get => resistances; set => resistances = value; }
    public GameObject CharacterPrefab => characterPrefab;


    // 프로젝트 파일에서 생성할때 배열 생성
    private void Awake()
    {
        if (resistances == null || resistances.Length == 0)
        {
            resistances = new StatusResistance[]
            {
                new StatusResistance {Name = StatusEffectType.Bleed.ToString() ,StatusType = StatusEffectType.Bleed},
                new StatusResistance {Name = StatusEffectType.Poison.ToString(),StatusType = StatusEffectType.Poison},
                new StatusResistance {Name = StatusEffectType.Burn.ToString(),StatusType = StatusEffectType.Burn },
                new StatusResistance {Name = StatusEffectType.Stun .ToString(),StatusType = StatusEffectType.Stun },
                new StatusResistance {Name = StatusEffectType.Weakness .ToString(),StatusType = StatusEffectType.Weakness },
                new StatusResistance {Name = StatusEffectType.Move.ToString(),StatusType = StatusEffectType.Move },
                new StatusResistance {Name = StatusEffectType.Freeze .ToString(),StatusType = StatusEffectType.Freeze },
                new StatusResistance {Name = StatusEffectType.Electrocute.ToString(),StatusType = StatusEffectType.Electrocute }
            };
        }
    }
    // 정보 복제
    public CharacterInfoSO Clone()
    {
        // 현재 스크립터블 오브젝트의 인스턴스를 복제하여 새로운 인스턴스를 생성
        CharacterInfoSO newCharacterInfo = Instantiate(this);
        Character character = characterPrefab.GetComponent<Character>();
        character.CharacterInfo = newCharacterInfo;
        return newCharacterInfo;
    }
}
