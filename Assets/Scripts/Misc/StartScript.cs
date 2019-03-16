using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    [Header("References")]
    public ClickToContinuePopup letterStartScript;
    public GameObject mapToLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (mapToLoad == null || letterStartScript == null)
        {
            Debug.LogError("Setup Script Error!!");
            this.enabled = false;
            return;
        }

        letterStartScript.mapGameObject = mapToLoad;
        letterStartScript.SetupLetter(null);
    }
}
