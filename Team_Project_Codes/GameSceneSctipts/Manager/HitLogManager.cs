using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitLogManager : MonoBehaviour
{
    [SerializeField] private Transform hitLogPos;
    [SerializeField] private GameObject hitLogObject;
    [SerializeField] private List<HitLogUI> hitLogUIs;
    [SerializeField] private float upPosValue;

    private WaitForSeconds logDisplayInterval = new WaitForSeconds(1f);
    private Dictionary<Transform, Queue<HitLog>> characterHitLogs = new Dictionary<Transform, Queue<HitLog>>();
    private Dictionary<Transform, bool> isDisplayingLog = new Dictionary<Transform, bool>();

    // 객체 풀을 위한 힛로그 인스턴스 리스트
    private Stack<HitLog> hitLogPool = new Stack<HitLog>();

    // 풀에서 객체를 가져오고, 없다면 새로 생성
    private HitLog GetHitLog(HitLog_Type hitLogType, string text)
    {
        if (hitLogPool.Count > 0)
        {
            // 풀에서 재사용 가능한 HitLog 객체를 가져옴
            HitLog hitLog = hitLogPool.Pop();
            hitLog.hitLogType = hitLogType;
            hitLog.text = text;
            return hitLog;
        }
        else
        {
            // 재사용할 수 없으면 새로 생성
            return new HitLog(hitLogType, text);
        }
    }
    private HitLog GetHitLog(StatusEffectBase statusEffectBase, bool isGranted)
    {
        string text = isGranted ? statusEffectBase.ShowGrantSuccessMessage() : statusEffectBase.ShowGrantFailureMessage();

        if (hitLogPool.Count > 0)
        {
            // 풀에서 재사용 가능한 HitLog 객체를 가져옴
            HitLog hitLog = hitLogPool.Pop();
            hitLog.hitLogType = HitLog_Type.StatusEffect;
            hitLog.text = text;
            return hitLog;
        }
        else
        {
            // 재사용할 수 없으면 새로 생성
            return new HitLog(HitLog_Type.StatusEffect, text);
        }
    }

    // 풀에 객체를 반환
    private void ReturnHitLogToPool(HitLog hitLog)
    {
        hitLogPool.Push(hitLog);
    }

    public void ShowDamageLog(Transform characterPos, HitLog_Type hitLog_Type, string text = null)
    {
        // 캐릭터별로 로그 큐를 생성
        if (!characterHitLogs.ContainsKey(characterPos))
        {
            characterHitLogs[characterPos] = new Queue<HitLog>();
            isDisplayingLog[characterPos] = false;
        }

        // 객체 풀에서 HitLog 객체를 가져와 로그 큐에 추가
        HitLog hitLog = GetHitLog(hitLog_Type, text);
        characterHitLogs[characterPos].Enqueue(hitLog);

        // 로그가 출력되지 않으면 출력 코루틴을 시작
        if (!isDisplayingLog[characterPos])
        {
            StartCoroutine(DisplayLogsForCharacter(characterPos));
        }
    }
    public void ShowDamageLog(Transform characterPos, StatusEffectBase statusEffectBase, bool isGranted)
    {
        // 캐릭터별로 로그 큐를 생성
        if (!characterHitLogs.ContainsKey(characterPos))
        {
            characterHitLogs[characterPos] = new Queue<HitLog>();
            isDisplayingLog[characterPos] = false;
        }

        // 객체 풀에서 HitLog 객체를 가져와 로그 큐에 추가
        HitLog hitLog = GetHitLog(statusEffectBase, isGranted);
        characterHitLogs[characterPos].Enqueue(hitLog);

        // 로그가 출력되지 않으면 출력 코루틴을 시작
        if (!isDisplayingLog[characterPos])
        {
            StartCoroutine(DisplayLogsForCharacter(characterPos));
        }
    }

    private IEnumerator DisplayLogsForCharacter(Transform characterPos)
    {
        isDisplayingLog[characterPos] = true;

        // 캐릭터의 로그 큐가 비어있지 않다면 로그를 순차적으로 표시
        while (characterHitLogs[characterPos].Count > 0)
        {
            HitLog hitLog = characterHitLogs[characterPos].Dequeue();
            DisplayLog(characterPos, hitLog.hitLogType, hitLog.text);

            // 로그 처리가 끝난 후 객체 풀에 반환
            ReturnHitLogToPool(hitLog);

            yield return logDisplayInterval; // 로그 간격 조정
        }

        isDisplayingLog[characterPos] = false;
    }

    private void DisplayLog(Transform characterPos, HitLog_Type hitLog_Type, string text = null)
    {
        HitLogUI hitLogUI = GetReusableUI(hitLogUIs);
        // 캐릭터 위치를 화면 좌표로 변환
        Vector3 characterScreenPosition = Camera.main.WorldToScreenPoint(characterPos.position);

        // y 위치 조정 (upPosValue 값을 좀 더 다이나믹하게 처리)
        characterScreenPosition.y += upPosValue;

        if (hitLogUI == null)
        {
            // 힛로그 UI를 새로 생성하고 텍스트 정보 설정
            hitLogUI = Instantiate(hitLogObject, hitLogPos.transform).GetComponent<HitLogUI>();
            hitLogUI.SetTextInfo(hitLog_Type, text);
            
            /*Vector2 normalizedPos = new Vector2(characterScreenPosition.x / Screen.width, characterScreenPosition.y / Screen.height);
            hitLogUI.RectTransform.anchorMin = normalizedPos;
            hitLogUI.RectTransform.anchorMax = normalizedPos;*/

            // 애니메이션이 실행되면 위치가 덮어쓰지 않도록 미리 설정
            hitLogUIs.Add(hitLogUI);
        }
        else
        {
            // 기존 UI 업데이트
            hitLogUI.SetTextInfo(hitLog_Type, text);

        }
        Vector2 normalizedPos = new Vector2(characterScreenPosition.x / Screen.width, characterScreenPosition.y / Screen.height);
        hitLogUI.RectTransform.anchorMin = normalizedPos;
        hitLogUI.RectTransform.anchorMax = normalizedPos;
        hitLogUI.gameObject.SetActive(true);
    }

    private HitLogUI GetReusableUI(List<HitLogUI> uiList)
    {
        return uiList.FirstOrDefault(ui => !ui.gameObject.activeSelf);
    }

    // 로그 항목을 담는 클래스
    private class HitLog
    {
        public HitLog_Type hitLogType;
        public string text;

        public HitLog(HitLog_Type hitLogType, string text)
        {
            this.hitLogType = hitLogType;
            this.text = text;
        }
    }
}
