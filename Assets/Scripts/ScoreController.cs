using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : Singleton<ScoreController>
{
    [SerializeField] TextMeshProUGUI _scoreText;

    private int _totalScore = 0;
    private int _currentLevelScore = 0;

    private void Start()
    {
        _totalScore = 0;
    }

    private void OnEnable()
    {
        _currentLevelScore = 0;
    }

    private void Update()
    {
        _scoreText.text = ""+GetScore();
    }

    public void Add(int score)
    {
        _currentLevelScore += score;
    }

    public int GetScore()
    {
        return _totalScore + _currentLevelScore;
    }

    public void ResetCurrentLevelScore()
    {
        _currentLevelScore = 0;
    }

    public void FinishLevel()
    {
        _totalScore += _currentLevelScore;
        ResetCurrentLevelScore();
    }

    public void ResetScore()
    {
        _totalScore = 0;
        _currentLevelScore = 0;
    }
}
