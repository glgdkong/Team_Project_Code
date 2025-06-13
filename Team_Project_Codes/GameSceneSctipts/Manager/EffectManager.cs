using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectManager : MonoBehaviour
{
    //[SerializeField] List<GameObject> effectObject = new List<GameObject>();
    public void SetSkillEffect(SkillInfoSO skillInfoSO, Character targetCharacter, bool isHit = true)
    {
        if (skillInfoSO.SkillEffect == null || !isHit) return;

        Vector3 spawnPos = skillInfoSO.SkillEffectInfo.EffectSpawnPos;
        spawnPos += targetCharacter.transform.position;

        //effectObject.Add(Instantiate(skillInfoSO.SkillEffect, spawnPos, skillInfoSO.SkillEffect.transform.localRotation));   
        Instantiate(skillInfoSO.SkillEffect, spawnPos, skillInfoSO.SkillEffect.transform.localRotation);   
        
    }

    /*public void DeleteEffect()
    {
        if (effectObject.Count > 0)
        {
            List<GameObject> deleteEffectList = new List<GameObject>(effectObject);
            foreach (GameObject obj in deletEffectList) 
            {
                effectObject.Remove(obj);
                Destroy(obj);
            }
        }
    }*/
}
