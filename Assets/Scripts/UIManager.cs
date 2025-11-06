using JetBrains.Annotations;
using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UI;
    public static event Action OnStop;
    public static event Action OnPlay;
    private void Start()
    {
        UI.SetActive(false);
        Settings.OnVideoApply += HideUI;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UI.activeSelf)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
    }

    private void HideUI()
    {
        UI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        OnPlay.Invoke();
    }
    private void ShowUI()
    {
        UI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        OnStop.Invoke();
    }
}
