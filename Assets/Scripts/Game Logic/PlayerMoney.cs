using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int startingCash = 1000;
    private int playerCash;
    // Start is called before the first frame update
    void Start()
    {
        playerCash = startingCash;
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
            return true;
        } else
        {
            return false;
        }
    }

    private bool checkCash(int newCash)
    {
        if (newCash < 0)
        {
            Debug.Log("Player run out of cash");
            return false;
        } else
        {
            Debug.Log("New Player cash: " + newCash.ToString());
            return true;
        }
    }
}
