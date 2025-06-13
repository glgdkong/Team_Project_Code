using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatusInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private float upPos;
    private StatusUI statusUI;
    public StatusUI StatusUI => statusUI;
    public GameObject InfoUI => infoUI;


    private void OnEnable()
    {
    }

    private void LateUpdate()
    {
        if (infoUI.gameObject.activeSelf)
        {
            // 마우스 위치를 화면 좌표로 가져오기
            Vector3 mousePosition = Input.mousePosition;

            // 마우스 위치를 부모 RectTransform의 로컬 좌표로 변환
            RectTransform parentRectTransform = GetComponent<RectTransform>();
            // 마우스 위치를 캔버스의 로컬 좌표로 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), mousePosition, canvas.worldCamera, out localPoint);
            localPoint.y += upPos;

            // 부모 RectTransform의 크기 구하기
            Vector2 parentSize = parentRectTransform.rect.size;

            // UI의 크기 구하기 (RectTransform)
            Vector2 uiSize = infoUI.GetComponent<RectTransform>().rect.size;

            // UI가 부모의 범위를 벗어나지 않도록 좌우로 제한
            float halfWidth = uiSize.x / 2;
            localPoint.x = Mathf.Clamp(localPoint.x, -parentSize.x / 2 + halfWidth, parentSize.x / 2 - halfWidth);

            // UI의 위치를 갱신
            infoUI.transform.localPosition = localPoint;
        }
    }

    private void OnDisable()
    {
        infoText.text = string.Empty;
        statusUI = null;
    }

    public void GetToolTipText(string text, StatusUI statusUI = null)
    {
        infoText.text = text;
         this.statusUI = statusUI;
    }

}
