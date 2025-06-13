#nullable enable

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerCharacterInfo", menuName = "Character/Player")]
public class PlayableCharacterSO : CharacterInfoSO
{
    [SerializeField, Header("패시브 스킬 설명"), TextArea] protected string passiveSkillText = string.Empty;
    [SerializeField, Header("캐릭터 설명"), TextArea] protected string characterInfoText = string.Empty;
    [SerializeField, Header("플레이어 이동 가능 범위")] protected int moveRange;
    [SerializeField, Header("플레이어 마나")] protected int mana;
    [SerializeField, Header("선택된 스킬 리스트")] protected List<SkillInfoSO> selectedSkillInfos = new List<SkillInfoSO>();
    [SerializeField] protected FinalItem? firstItem;
    [SerializeField] protected FinalItem? secondItem;

    
    public ItemStatType TotalItemStat()
    {
        ItemStatType itemStatType = new ItemStatType();
        itemStatType.Hp = 0;
        itemStatType.Mana = 0;
        itemStatType.Speed = 0;
        itemStatType.Attack = 0;
        itemStatType.Evade = 0;
        itemStatType.Accuracy = 0;
        itemStatType.Critical = 0;
        itemStatType.Defense = 0;

        if (firstItem == null && secondItem == null) { return itemStatType; }

        /*if (firstItem != null)
        {
            itemStatType.Hp += firstItem.ItemStats.Hp;
            itemStatType.Mana += firstItem.ItemStats.Mana;
            itemStatType.Speed += firstItem.ItemStats.Speed;
            itemStatType.Attack += firstItem.ItemStats.Attack;
            itemStatType.Evade += firstItem.ItemStats.Evade;
            itemStatType.Accuracy += firstItem.ItemStats.Accuracy;
            itemStatType.Critical += firstItem.ItemStats.Critical;
            itemStatType.Defense += firstItem.ItemStats.Defense;
            
            for (int i = 0; i < firstItem.ItemStats.ResistanceValues.Count; i++)
            {
                float resStat;
                if (firstItem.ItemStats.ResistanceValues.ContainsKey((StatusEffectType)i))
                {
                    firstItem.ItemStats.ResistanceValues.TryGetValue((StatusEffectType)i, out resStat);
                    if (itemStatType.ResistanceValues.ContainsKey((StatusEffectType)i))
                    {
                        itemStatType.ResistanceValues[(StatusEffectType)i] += resStat;
                    }
                    else
                    {
                        itemStatType.ResistanceValues.Add((StatusEffectType)i, resStat);
                    }
                    
                }
                
            }

        }
        if (secondItem != null)
        {
            itemStatType.Hp += secondItem.ItemStats.Hp;
            itemStatType.Mana += secondItem.ItemStats.Mana;
            itemStatType.Speed += secondItem.ItemStats.Speed;
            itemStatType.Attack += secondItem.ItemStats.Attack;
            itemStatType.Evade += secondItem.ItemStats.Evade;
            itemStatType.Accuracy += secondItem.ItemStats.Accuracy;
            itemStatType.Critical += secondItem.ItemStats.Critical;
            itemStatType.Defense += secondItem.ItemStats.Defense;
            for (int i = 0; i < secondItem.ItemStats.ResistanceValues.Count; i++)
            {
                float resStat;
                if (secondItem.ItemStats.ResistanceValues.ContainsKey((StatusEffectType)i))
                {
                    secondItem.ItemStats.ResistanceValues.TryGetValue((StatusEffectType)i, out resStat);
                    if (itemStatType.ResistanceValues.ContainsKey((StatusEffectType)i))
                    {
                        itemStatType.ResistanceValues[(StatusEffectType)i] += resStat;
                    }
                    else
                    {
                        itemStatType.ResistanceValues.Add((StatusEffectType)i, resStat);
                    }

                }

            }

        }*/

        // 공통 처리를 위한 함수 호출
        AddItemStats(firstItem, ref itemStatType);
        AddItemStats(secondItem, ref itemStatType);

        return itemStatType;
    }

    private void AddItemStats(FinalItem? item, ref ItemStatType itemStatType)
    {
        if (item == null) return;

        // 기본 속성 값 추가
        itemStatType.Hp += item.ItemStats.Hp;
        itemStatType.Mana += item.ItemStats.Mana;
        itemStatType.Speed += item.ItemStats.Speed;
        itemStatType.Attack += item.ItemStats.Attack;
        itemStatType.Evade += item.ItemStats.Evade;
        itemStatType.Accuracy += item.ItemStats.Accuracy;
        itemStatType.Critical += item.ItemStats.Critical;
        itemStatType.Defense += item.ItemStats.Defense;

        // 저항 값 처리
        AddResistanceValues(item.ItemStats.ResistanceValues, ref itemStatType);
    }

    private void AddResistanceValues(Dictionary<StatusEffectType, float> resistanceValues, ref ItemStatType itemStatType)
    {
        if (resistanceValues == null) return;

        foreach (var resistance in resistanceValues)
        {
            if (itemStatType.ResistanceValues.ContainsKey(resistance.Key))
            {
                itemStatType.ResistanceValues[resistance.Key] += resistance.Value;
            }
            else
            {
                itemStatType.ResistanceValues.Add(resistance.Key, resistance.Value);
            }
        }
    }


    public string PassiveSkillText => passiveSkillText;
    public string CharacterInfoText => characterInfoText;
    public int Mana { get => mana; set => mana = value; }
    public int MoveRange { get => moveRange; set => moveRange = value; }
    public List<SkillInfoSO> SelectedSkillInfos { get => selectedSkillInfos; set => selectedSkillInfos = value; }
    public FinalItem? FirstItem { get => firstItem; set => firstItem = value; }
    public FinalItem? SecondItem { get => secondItem; set => secondItem = value; }
}
