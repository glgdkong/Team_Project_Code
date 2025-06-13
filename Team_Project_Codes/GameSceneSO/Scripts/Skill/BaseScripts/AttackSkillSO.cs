using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillSO : SkillInfoSO
{
    [SerializeField, Header("타겟 지정 가능자리")] protected bool[] targetPositions = new bool[4];

    public bool[] TargetPositions => targetPositions; 

}
