using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseItemInfo", menuName = "Item/BaseItem")]
public class BaseItemSO : ScriptableObject
{
    [SerializeField, Header("아이템 유형")] private BaseItemEnum baseItemType;
    [SerializeField, Header("베이스 아이템 이름")] private string itemName;
    [SerializeField, Header("베이스 아이템 능력치 변화")] private List<StatModifierSO> modifiers;
    [SerializeField, Header("베이스의 기본 스프라이트")] private Sprite basicSprite;

    public BaseItemEnum BaseItemType { get => baseItemType; set => baseItemType = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public List<StatModifierSO> Modifiers { get => modifiers; set => modifiers = value; }
    public Sprite BasicSprite { get => basicSprite; set => basicSprite = value; }
}
