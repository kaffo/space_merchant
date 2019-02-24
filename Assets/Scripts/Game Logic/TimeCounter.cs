using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public Text textToUpdate;
    public bool gameOver = false;
    private int timePassed = 0;

    public void passTime(int timeToPass)
    {
        if (!gameOver)
        {
            timePassed += timeToPass;
            Debug.Log("Time Passed: " + timePassed);
            if (textToUpdate != null) { textToUpdate.text = "Time Passed: " + timePassed + " hours"; }

            if (timePassed > 25)
            {
                Debug.Log("Your Father Has Died...");
                if (textToUpdate != null) { textToUpdate.text = "Your Father has died..."; }
                gameOver = true;
            }
        }
    }
     
    public int currentTimePassed()
    {
        return timePassed;
    }
}
