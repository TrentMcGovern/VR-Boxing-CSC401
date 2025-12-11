using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score = 0;

    [Header("Difficulty Thresholds")]
    public int unlockSecondSide = 20;
    public int unlockThirdSide = 50;
    public int unlockFourthSide = 100;

    public int sidesUnlocked = 1; // start with 1 side

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        Score += amount;
        EvaluateDifficulty();
    }

    void EvaluateDifficulty()
    {
        if (Score >= unlockFourthSide)
            sidesUnlocked = 4;
        else if (Score >= unlockThirdSide)
            sidesUnlocked = 3;
        else if (Score >= unlockSecondSide)
            sidesUnlocked = 2;
        else
            sidesUnlocked = 1;
    }



}
