﻿using System.Collections;
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

	private ProgressTracker()
	{        
    }

    public void SetScore(int _score)
    {
        score = _score;
    }

    public void ModScore(int _score)
    {
        score += _score;
    }

    public void SubmitAndIncrementProgress()
    {
        Debug.Log("progress: " + currentProgress + " score: " + score + " submitted");
        SubmitProgress(currentProgress, score);
        currentProgress++;
        currentProgress = Mathf.Clamp(currentProgress,0,maxProgress);
        
    }

    public void SubmitProgress(int progressNumber, int progressScore)
	{
        if (LOLSDK.Instance.IsInitialized)
        {            
            LOLSDK.Instance.SubmitProgress(score, progressNumber, maxProgress);// SCORE, CURRENTPROGRESS, MAXPROGRESS
        }
    }
}
