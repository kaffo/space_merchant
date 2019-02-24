using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveNode : MonoBehaviour
{
    public GameObject ship;
    public GameObject ring;

    public GameManagerFinder managerFinder;

    private bool isActiveNode = false;

    private bool isRingActive = false;

    private TimeCounter timeCounter;

    public void setActive(bool active)
    {
        isActiveNode = active;
        ship.SetActive(active);

        // Enable/disable the adjacent nodes as clickable
        NodeConnections myNodeConnections = GetComponent<NodeConnections>();
        if (myNodeConnections != null && !timeCounter.gameOver)
        {
            foreach (var node in myNodeConnections.connectedNodes.Keys)
            {
                ActiveNode nodeScript = node.GetComponent<ActiveNode>();
                if (nodeScript != null) { nodeScript.setRingActive(isActiveNode); }
            }
        }
    }

    public bool getActive()
    {
        return isActiveNode;
    }

    public void setRingActive(bool active)
    {
        isRingActive = active;
        ring.SetActive(active);
    }

    public bool getRingActive()
    {
        return isRingActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ship == null || ring == null || managerFinder == null)
        {
            Debug.LogError("Active Node Script error on " + gameObject.name);
            this.enabled = false;
            return;
        }

        ship.SetActive(isActiveNode);
        ring.SetActive(isRingActive);

        timeCounter = managerFinder.gameManager.GetComponent<TimeCounter>();
    }
}
