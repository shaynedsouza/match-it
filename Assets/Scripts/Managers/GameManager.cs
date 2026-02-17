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
        m_score = PlayerPrefs.GetInt("score", 0);
        CardManager.Instance.SpawnCards(NumberOfPairs);
    }


}
