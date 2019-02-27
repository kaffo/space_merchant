using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : Singleton<TimeCounter>
{
    public Text textToUpdate;
    public bool gameOver = false;

    private int timeLeft = 25;

    private void UpdateUI()
    {
        if (timeLeft <= 0)
        {
            Debug.Log("Your Father Has Died...");
            if (textToUpdate != null) { textToUpdate.text = "Your Father has died..."; }
            gameOver = true;
        } else
        {
            if (textToUpdate != null) { textToUpdate.text = "Time Left: " + timeLeft; }
        }
    }

    public void passTime(int timeToPass)
    {
        if (!gameOver)
        {
            // Can't have less than 0 time
            timeLeft = (int)Mathf.Max(timeLeft - timeToPass, 0f);

            Debug.Log("Time Left: " + timeLeft);
            StepEconomy(timeToPass);
            UpdateUI();
        }
    }

    public void addTime(int timeToAdd)
    {
        if (!gameOver)
        {
            timeLeft += timeToAdd;
            Debug.Log("Time Left: " + timeLeft);
            UpdateUI();
        }
    }
     
    public int currentTimeLeft()
    {
        return timeLeft;
    }

    public void StepEconomy(int timesToStep = 1)
    {
        IEnumerator currentEconMethod;
        foreach (var goodScript in ObjectManager.Instance.globalGoodList)
        {
            currentEconMethod = goodScript.StepEconomy(timesToStep);
            StartCoroutine(currentEconMethod);
        }
    }
}
