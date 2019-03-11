using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : Singleton<TimeCounter>
{
    public Text timeText;
    public Text fatherHealthText;
    public ClickToContinuePopup popupScript;
    public bool gameOver = false;

    private int timeLeft = 250;
    private int timePassed = 0;
    private int fathersHealth = 100;

    private void Start()
    {
        if (timeText == null || fatherHealthText  == null || popupScript == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        
        if (gameOver)
        {
            Debug.Log("Your Father Has Died...");
            timeText.text = "Your Father has died...";
            fatherHealthText.text = "";
            popupScript.SetupLetter("My Father has died...\n\nFor " + timePassed + " long hours I struggled, however it is apparent I could not do enough...\n\nMaybe I could have done more, or maybe this outcome was always inevitable. But it is too late for these thoughts now.\n\nInstead I must turn my attention to my children and build a future for them.\n\nGod forbid I ever fall ill like my Father and my children must bear the burden as I have....");
        } else
        {
            timeText.text = "Time Left: " + timeLeft;
            fatherHealthText.text = "Father Health: " + fathersHealth;
        }
    }

    private void SetGameOver(bool isGameOver)
    {
        if (isGameOver)
        {
            gameOver = isGameOver;

            // Update all Goods UI
            foreach (var good in ObjectManager.Instance.globalGoodList)
            {
                good.updateUI();
            }

            // Update all Upgrades UI
            foreach (var upgrade in ObjectManager.Instance.globalUpgradeList)
            {
                upgrade.updateUI();
            }

            // Update all Jump Button UI
            foreach (var node in ObjectManager.Instance.globalNodeList)
            {
                node.jumpButton.interactable = false;
            }
        }
    }

    public void passTime(int timeToPass)
    {
        if (!gameOver)
        {
            // Pass Time
            int newTimeLeft = timeLeft - timeToPass;

            // Track time passed
            timePassed += timeLeft - newTimeLeft;
            timeLeft = newTimeLeft;

            // Next time check?
            if (timeLeft <= 0)
            {
                PlayerCargo playerCargo = PlayerCargo.Instance;
                // Check player inventory for Medicine
                if (playerCargo.HasGood(Defs.TradeGoods.GOOD_MEDICINE))
                {
                    playerCargo.RemoveSingleCargo(Defs.TradeGoods.GOOD_MEDICINE);
                    Debug.Log("Payed Medicine for Father");
                } else
                {
                    // Min 20 damage, Max 50
                    int fatherDamage = (int)(Random.value * 30) + 20;
                    fathersHealth -= fatherDamage;
                    Debug.Log("No Medicine Avaliable, Father taking " + fatherDamage + " damage down to " + fathersHealth);
                }

                // Reset time left
                timeLeft += 250;
            }

            if (fathersHealth <= 0) { SetGameOver(true); }

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
