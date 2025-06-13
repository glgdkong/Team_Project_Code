using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillSelectionWarningUI : MonoBehaviour
{
    [SerializeField] private float time = 0.5f;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        if ((gameObject.activeSelf && !isActive) || SceneManager.GetActiveScene().name == "GameScene")
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (!isActive) { return; }

        Invoke("HideSkillSelectionWarning", time);
    }
    public void ShowSkillSelectionWarning()
    {
        isActive = true;
        gameObject.SetActive(true);
    }
    private void HideSkillSelectionWarning()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
