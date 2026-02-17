using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    public TextMeshProUGUI TotalScoreField;
    public TextMeshProUGUI RoundScoreField;
    public GameObject MenuPanel;
    public GameObject ResultPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }


    public void SetTotalScore(int score)
    {
        TotalScoreField.text = "Total Score: " + score.ToString("D3"); ;
    }
    public void SetRoundScore(int score)
    {
        RoundScoreField.text = "Round Score: " + score.ToString("D3"); ;
    }



    public void StartGame()
    {
        RoundScoreField.gameObject.SetActive(true);
    }



    public void ShowResultPanel(bool value, int score = 0)
    {

        if (value)
        {
            TotalScoreField.text = "Total Score: " + score.ToString("D3"); ;

            ResultPanel.transform.localScale = Vector3.zero;
            ResultPanel.SetActive(true);
            ResultPanel.transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            ResultPanel.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                SetRoundScore(0);
                ResultPanel.SetActive(false);
                RoundScoreField.gameObject.SetActive(false);

                MenuPanel.transform.localScale = Vector3.zero;
                MenuPanel.SetActive(true);
                MenuPanel.transform.DOScale(Vector3.one, 0.5f);
            });
        }
    }

    /// <summary>
    /// Difficulty
    /// 0 - Easy 
    /// 1 - Medium      
    /// 2 - Hard 
    /// </summary>
    /// <param name="difficulty"></param>
    public void StartGameButtonClicked(int difficulty)
    {
        GameManager.Instance.StartGame(difficulty);

        MenuPanel.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => MenuPanel.gameObject.SetActive(false));
        RoundScoreField.gameObject.SetActive(true);
    }


    public void GoToMenuClicked()
    {
        GameManager.Instance.GoToMenu();
        ShowResultPanel(false);

    }

}
