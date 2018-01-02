using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using System.Linq;

public class ProgressTracker {
    #region singleton
    private static ProgressTracker instance;

	public static ProgressTracker Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new ProgressTracker();
			}
			return instance;
		}
	}
    #endregion

    public int maxProgress = 8;
    public int currentProgress = 1;
    public int score = 0;
    public int maxGrowthHeight = 50;

	private ProgressTracker()
	{        
    }

    public void SetMaxHeight(int _height)
    {
        if (maxGrowthHeight < _height)
            maxGrowthHeight = _height;
    }

    public void SetScore(int _score)
    {
        score = _score;
    }

    public void ModScore(int _score)
    {
        score += _score;
    }

    public void SubmitProgress(int progressScore)
    {
        if (progressScore > currentProgress)
            currentProgress = progressScore;
        SubmitProgress(currentProgress, score);        
    }

    public void SubmitProgress(int progressNumber, int progressScore)
	{
        if (LOLSDK.Instance.IsInitialized)
        {            
            LOLSDK.Instance.SubmitProgress(score, progressNumber, maxProgress);// SCORE, CURRENTPROGRESS, MAXPROGRESS
        }
    }
}
