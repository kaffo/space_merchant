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
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (moneyText != null) { moneyText.text = "Money $" + playerCash; };
    }

    public int GetPlayerCash()
    {
        return playerCash;
    }

    public bool SetPlayerCash(int newCash)
    {
        if (CheckCash(newCash))
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

    public bool IncrementPlayerCash(int incrementCash)
    {
        if (CheckCash(playerCash + incrementCash))
        {
            playerCash += incrementCash;
            UpdateUI();
            return true;
        } else
        {
            return false;
        }
    }

    public bool CheckCash(int newCash)
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
