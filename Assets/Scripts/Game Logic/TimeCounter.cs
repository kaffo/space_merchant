using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : Singleton<TimeCounter>
{
    public Text textToUpdate;
    public ClickToContinuePopup popupScript;
    public bool gameOver = false;

    private int timeLeft = 250;
    private int timePassed = 0;

    private void Start()
    {
        if (textToUpdate == null || popupScript == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timeLeft <= 0)
        {
            Debug.Log("Your Father Has Died...");
            textToUpdate.text = "Your Father has died...";
            popupScript.SetupLetter("My Father has died...\n\nFor " + timePassed + " long hours I struggled, however it is apparent I could not do enough...\n\nMaybe I could have done more, or maybe this outcome was always inevitable. But it is too late for these thoughts now.\n\nInstead I must turn my attention to my children and build a future for them.\n\nGod forbid I ever fall ill like my Father and my children must bear the burden as I have....");
            gameOver = true;
        } else
        {
            textToUpdate.text = "Time Left: " + timeLeft;
        }
    }

    public void passTime(int timeToPass)
    {
        if (!gameOver)
        {
            // Can't have less than 0 time
            int newTimeLeft = (int)Mathf.Max(timeLeft - timeToPass, 0f);

            // Track time passed but don't track past 0
            timePassed += timeLeft - newTimeLeft;
            timeLeft = newTimeLeft;

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
