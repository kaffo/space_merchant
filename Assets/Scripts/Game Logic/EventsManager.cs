using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : Singleton<EventsManager>
{
    public ClickToContinuePopup continuePopupScript;
    // Start is called before the first frame update
    void Start()
    {
        if (continuePopupScript == null)
        {
            Debug.LogError(gameObject.name + " " + this.name + " is invalid");
            this.enabled = false;
            return;
        }
    }

    public void StartEvent(Defs.Events eventType)
    {
        Debug.Log("Starting Event: " + eventType.ToString());
        switch(eventType)
        {
            case Defs.Events.EVENT_PIRATE_ONE:
                if (PlayerCargo.Instance.TotalCargo() > 0)
                {
                    PlayerCargo.Instance.EmptyCargo();
                    continuePopupScript.SetupLetter("Uh oh, Pirates!\n\nThey've stolen my cargo...");
                } else
                {
                    continuePopupScript.SetupLetter("Uh oh, Pirates!\n\nLuckily I had no cargo for them to take, so they left me alone");
                }
                break;
            case Defs.Events.EVENT_PIRATE_TWO:
                int playerCash = PlayerMoney.Instance.GetPlayerCash();
                // Don't rob the player if they are super poor
                if (playerCash > 100)
                {
                    int moneyLost = (int)((float)playerCash / 100 * 10);
                    PlayerMoney.Instance.IncrementPlayerCash(-moneyLost);
                    continuePopupScript.SetupLetter("Uh oh, Pirates!\n\nThey demanded I transfered them $" + moneyLost.ToString() + " or they turn me into debris.\n\nI've transfered them the money and they left me alone.");
                }
                else
                {
                    continuePopupScript.SetupLetter("Uh oh, Pirates!\n\nThey demanded money from me, but I had so little they just laughed through the comm.\n\nI heard one say \"That pilot is fucked\" before they hung up.");
                }
                break;
        }
    }
}
