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
        }
    }
}
