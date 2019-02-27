using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    public ActiveNode startNode;
    public ClickToContinuePopup letterStartScript;
    // Start is called before the first frame update
    void Start()
    {
        if (startNode)
        {
            startNode.setActive(true);
            ObjectManager.Instance.currentActiveNode = startNode;
        }

        if (letterStartScript != null)
        {
            letterStartScript.SetupLetter(null);
        }
    }
}
