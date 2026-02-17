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
        // CardManager.Instance.SpawnCards(NumberOfPairs);
    }

    public void FinishedGame(int score)
    {
        m_score += score;
        PlayerPrefs.SetInt(m_scoreString, m_score);
        CanvasManager.Instance.ShowResultPanel(true, m_score);
        // CardManager.Instance.DestroyCards();

    }


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
    }

    public void GoToMenu()
    {
        m_score = 0;
        CardManager.Instance.DestroyCards();
    }
}
