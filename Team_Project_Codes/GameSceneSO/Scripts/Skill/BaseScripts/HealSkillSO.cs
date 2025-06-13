using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HealType
{
    Heath,
    Mana
}
public class HealSkillSO : SkillInfoSO
{
    [SerializeField] private HealType healType;
    public HealType HealType => healType;  
}
