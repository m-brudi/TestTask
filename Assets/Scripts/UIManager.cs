using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button btn50;
    [SerializeField] Button btn100;
    [SerializeField] Button btn250;
    [SerializeField] Button btn500;
    [SerializeField] Button btnStart;
    [SerializeField] GameObject startBtnIcon;
    [Space]
    [Header("GameOver")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button mainMenuBtn;
    [Space]
    [Header("Buttons Sprites")]
    [SerializeField] Sprite greenSpr;
    [SerializeField] Sprite greySpr;
    [SerializeField] Sprite blueSpr;
    [SerializeField] Sprite orangeSpr;

    Button selectedBtn;
    int objsCount;
    public void SetupMenuPanel() {
        menuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        btnStart.onClick.RemoveAllListeners();
        startBtnIcon.SetActive(false);
        btnStart.image.sprite = greySpr;
        btnStart.interactable = false;

        ResetMenuButtons();

        btn50.transform.localScale = Vector3.one;
        btn100.transform.localScale = Vector3.one;
        btn250.transform.localScale = Vector3.one;
        btn500.transform.localScale = Vector3.one;

        btn50.image.sprite = blueSpr;
        btn100.image.sprite = blueSpr;
        btn250.image.sprite = blueSpr;
        btn500.image.sprite = blueSpr;
    }

    public void SetupGameOverPanel() {
        gameOverPanel.SetActive(true);
        mainMenuBtn.onClick.RemoveAllListeners();
        mainMenuBtn.onClick.AddListener(() => SetupMenuPanel());
    }
    void HideMenu() {
        menuPanel.SetActive(false);
    }

    void HideGameOver() {
        gameOverPanel.SetActive(false);
    }

    void ResetMenuButtons() {
        btn50.onClick.RemoveAllListeners();
        btn100.onClick.RemoveAllListeners();
        btn250.onClick.RemoveAllListeners();
        btn500.onClick.RemoveAllListeners();
        btnStart.onClick.RemoveAllListeners();

        btn50.onClick.AddListener(() => ButtonSelected(btn50, 50));
        btn100.onClick.AddListener(() => ButtonSelected(btn100, 100));
        btn250.onClick.AddListener(() => ButtonSelected(btn250, 250));
        btn500.onClick.AddListener(() => ButtonSelected(btn500, 500));
    }

    void ButtonSelected(Button btn, int num) {
        if (selectedBtn) {
            selectedBtn.transform.DOScale(1, 0.3f);
            selectedBtn.image.sprite = blueSpr;
        }
        selectedBtn = btn;
        ResetMenuButtons();
        objsCount = num;
        selectedBtn.transform.DOScale(1.1f, 0.3f);
        selectedBtn.image.sprite = orangeSpr;
        btnStart.interactable = true;
        startBtnIcon.SetActive(true);
        btnStart.image.sprite = greenSpr;
        btnStart.onClick.AddListener(() => StartGame());
    }

    void StartGame() {
        Controller.Instance.StartGame(objsCount);
        menuPanel.SetActive(false);
    }
}
