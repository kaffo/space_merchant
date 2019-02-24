using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClick : MonoBehaviour
{
    public ActiveNode nodeScript;
    public NodeConnections myConnections;
    public GameManagerFinder managerFinder;

    private TimeCounter timeCount;
    private GameObject gameManager;

    private void Start()
    {
        if (nodeScript == null || myConnections == null || managerFinder == null)
        {
            Debug.LogError("NodeClick error on " + transform.parent.name);
            this.enabled = false;
        }

        gameManager = managerFinder.gameManager;
        timeCount = gameManager.GetComponent<TimeCounter>();

        if (timeCount == null)
        {
            Debug.LogError("NodeClick error on " + transform.parent.name);
            this.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (nodeScript.getRingActive() && !timeCount.gameOver) {
            // Find the current active node and disable it
            foreach (var node in myConnections.connectedNodes.Keys)
            {
                ActiveNode otherNodeScript = node.GetComponent<ActiveNode>();

                if (otherNodeScript != null && otherNodeScript.getActive()) {
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
