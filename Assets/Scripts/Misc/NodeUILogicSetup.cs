using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUILogicSetup : MonoBehaviour
{
    [Header("UI Elements")]
    public Button jumpButton;

    [Header("Scripts")]
    public ActiveNode nodeScript;
    public NodeConnections myConnections;
    public GameManagerFinder managerFinder;

    private TimeCounter timeCount;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (jumpButton == null || nodeScript == null || myConnections == null || managerFinder == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
        }

        jumpButton.onClick.AddListener(JumpButtonClick);

        gameManager = managerFinder.gameManager;
        timeCount = gameManager.GetComponent<TimeCounter>();

        if (timeCount == null)
        {
            Debug.LogError("NodeClick error on " + transform.parent.name);
            this.enabled = false;
        }
    }


    private void JumpButtonClick()
    {
        if (nodeScript.getRingActive() && !timeCount.gameOver)
        {
            // Find the current active node and disable it
            foreach (var node in myConnections.connectedNodes.Keys)
            {
                ActiveNode otherNodeScript = node.GetComponent<ActiveNode>();

                if (otherNodeScript != null && otherNodeScript.getActive())
                {
                    otherNodeScript.setActive(false);
                    timeCount.passTime(node.connectedNodes[myConnections]);
                    break;
                }
            }

            //Enable myself
            nodeScript.setActive(true);
        }
    }
}
