using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResUpUI : StatusUI
{
    [SerializeField,Header("저항 버프 리스트")] private List<ResUp> resUps = new List<ResUp>();

    public List<ResUp> ResUps { get => resUps; }
    private string[] statusName = new string[]{ "<color=#A60505>출혈</color>", "<color=#8D0199>중독</color>", "<color=#BF2D00>화상</color>", "<color=#F7CB00>기절</color>", "<color=#808080>이동</color>", "<color=#42AAF7>약화</color>", "<color=#2149EF>빙결</color>", "<color=#DEC308>감전</color>" };
    private string resUpInfo;
    public override void SetEffectInfo(StatusEffectBase statusEffectBase)
    {
        if (statusEffectBase is ResUp resUp) 
        {
            resUps.Add(resUp);
        }
    }
    private void ShowResistanceStatus()
    {

        Dictionary<StatusEffectType, float> resistanceSum = new Dictionary<StatusEffectType, float>();

        // ResUp 리스트를 순회하며 각 StatusResistance의 resistanceValue를 합산
        foreach (var resUp in resUps)
        {
            foreach (var resistance in resUp.StatusResistances)
            {
                // 각 상태 타입에 대해 resistanceValue 합산
                if (resistanceSum.ContainsKey(resistance.StatusType))
                {
                    resistanceSum[resistance.StatusType] += resistance.ResistanceValue;
                }
                else
                {
                    resistanceSum.Add(resistance.StatusType, resistance.ResistanceValue);
                }
            }
        }

        // 합산된 값들을 UI에 표시
        resUpInfo = "";

        // 각 상태 타입에 대한 저항 정보를 옆으로 이어서 출력
        foreach (var entry in resistanceSum)
        {
            resUpInfo += $"{statusName[(int)entry.Key]} 저항: {entry.Value * 100:0.##}%\n";  // 퍼센트로 변환하여 출력
            //resUpInfo += $"{statusName[(int)entry.Key]}저항: {entry.Value * 100:0.##}%, ";  // 퍼센트로 변환하여 출력
        }

        /*// 마지막의 불필요한 쉼표를 제거
        if (resUpInfo.Length > 0)
        {
            resUpInfo = resUpInfo.Substring(0, resUpInfo.Length - 2); // 마지막 쉼표와 공백 제거
        }*/
    }

    public override void ClearInfo(StatusEffectBase statusEffectBase)
    {
        if (statusEffectBase is ResUp resUp)
        {
            resUps.Remove(resUp);
            if (resUps.Count > 0)
            {
                ShowResistanceStatus();
                statusInfoUI.GetToolTipText(resUpInfo);
            }
            else
            {
                gameObject.SetActive(false);
                statusInfoUI.InfoUI.gameObject.SetActive(false);
            }
        }

    }

    public override void EnableInfo()
    {
        ShowResistanceStatus();
        statusInfoUI.GetToolTipText(resUpInfo, this);
        statusInfoUI.InfoUI.SetActive(true);
    }

    public override void DisableInfo()
    {
        statusInfoUI.InfoUI.SetActive(false);
    }
}
