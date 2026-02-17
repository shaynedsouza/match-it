using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private string m_scoreString = "SCORE";
    private int m_score = 0;

    public static GameManager Instance;
    public int NumberOfPairs = 8;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }



    void Start()
    {
        m_score = PlayerPrefs.GetInt(m_scoreString, 0);
        CanvasManager.Instance.SetTotalScore(m_score);
    }

    public void FinishedGame(int score)
    {
        m_score += score;
        AudioManager.Instance.PlaySFX("win", 0.5f);
        PlayerPrefs.SetInt(m_scoreString, m_score);
        CanvasManager.Instance.ShowResultPanel(true, m_score);

    }

    /// <summary>
    /// The difficulty is set by the user in the menu, and it determines how many pairs of cards will be spawned in the game.
    /// </summary>
    /// <param name="difficulty"></param>
    public void StartGame(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                NumberOfPairs = 4;
                break;
            case 1:
                NumberOfPairs = 8;
                break;
            case 2:
                NumberOfPairs = 12;
                break;
        }
        CardManager.Instance.SpawnCards(NumberOfPairs);
        CanvasManager.Instance.StartGame();
        AudioManager.Instance.PlaySFX("countdown");
        StartCoroutine(DoWithDelay(() =>
        {
            CardManager.Instance.HideAllCards();
        }, 3f));
    }




    public void GoToMenu()
    {
        m_score = 0;
        CardManager.Instance.ReturnCards();
    }





    /// <summary>
    /// Do some action with delay
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator DoWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
