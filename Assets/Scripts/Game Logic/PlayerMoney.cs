using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : Singleton<PlayerMoney>
{
    public int startingCash = 1000;
    public Text moneyText;
    private int playerCash;
    // Start is called before the first frame update
    void Start()
    {
        playerCash = startingCash;
    }

    private void UpdateUI()
    {
        if (moneyText != null) { moneyText.text = "Money $" + playerCash; };
    }

    public int getPlayerCash()
    {
        return playerCash;
    }

    public bool setPlayerCash(int newCash)
    {
        if (checkCash(newCash))
        {
            playerCash = newCash;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool incrementPlayerCash(int incrementCash)
    {
        if (checkCash(playerCash + incrementCash))
        {
            playerCash += incrementCash;
            UpdateUI();
            return true;
        } else
        {
            return false;
        }
    }

    public bool checkCash(int newCash)
    {
        if (newCash < 0)
        {
            Debug.Log("Player run out of cash");
            return false;
        } else
        {
            return true;
        }
    }
}
