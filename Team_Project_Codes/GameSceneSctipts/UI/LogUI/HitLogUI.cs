using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum HitLog_Type
{
    NormalHit,
    CriticalHit,
    Dodge,
    Bleed,
    Posion,
    Burn,
    HealthHeal,
    CriticalHealthHeal,
    ManaHeal,
    CriticalManaHeal,
    StatusEffect
}
public class HitLogUI : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float upTime;
    public RectTransform RectTransform => rectTransform;

    public void SetTextInfo(HitLog_Type hitLog_Type, string text = null)
    {
        // 기본 색상 설정
        textMeshProUGUI.color = colors[(int)hitLog_Type];

        // 기본 텍스트 설정
        string formattedText = text;

        // 각 타입에 따라 텍스트 형식만 다르게 설정
        switch (hitLog_Type)
        {
            case HitLog_Type.NormalHit:
                break;  // 기본 텍스트로 처리 (추가할 내용 없음)
            case HitLog_Type.CriticalHit:
                formattedText = "치명타!\n" + text;
                break;
            case HitLog_Type.Dodge:
                formattedText = Random.value <= 0.9f ? "빗나감!" : "감나빗!";
                break;
            case HitLog_Type.Bleed:
                formattedText = "출혈!\n" + text;
                break;
            case HitLog_Type.Posion:
                formattedText = "중독!\n" + text;
                break;
            case HitLog_Type.Burn:
                formattedText = "화상!\n" + text;
                break;
            case HitLog_Type.HealthHeal:
            case HitLog_Type.ManaHeal:
                formattedText = "+" + text;
                break;
            case HitLog_Type.CriticalHealthHeal:
            case HitLog_Type.CriticalManaHeal:
                formattedText = "치명타!\n+" + text;
                break;
            case HitLog_Type.StatusEffect:
                break;
            default:
                Debug.LogWarning("타입 오류!");
                break;
        }

        // 최종적으로 텍스트 업데이트
        textMeshProUGUI.text = formattedText;
    }

    private void AnimationEvent()
    {
        gameObject.SetActive(false);
    }
}
