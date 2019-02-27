using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeClick : MonoBehaviour
{
    public GameObject nodePanel;

    public ActiveNode activeNodeScript;
    public NodeConnections myConnections;

    private GameObject jumpTimePanel;
    private bool jumpPanelActive = false;

    private void Start()
    {
        jumpTimePanel = ObjectManager.Instance.jumpTimePanel;

        if (nodePanel == null  || activeNodeScript == null || myConnections == null || jumpTimePanel == null)
        {
            Debug.LogError("NodeClick error on " + transform.parent.name);
            this.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (jumpPanelActive)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            jumpTimePanel.transform.position = new Vector3(pos.x, pos.y + 1, 0f);
        }
    }

    private void OnMouseDown()
    {
        nodePanel.SetActive(!nodePanel.activeInHierarchy);
    }

    private void OnMouseEnter()
    {
        // We can jump to this node
        if (activeNodeScript.getRingActive())
        {
            Text jumpPanelText = jumpTimePanel.transform.GetChild(0).GetComponent<Text>();

            int timeToThisNode = 0;
            ActiveNode currentActiveNode = ObjectManager.Instance.currentActiveNode;

            if (currentActiveNode != null && currentActiveNode.getActive())
            {
                NodeConnections currentActiveNodeConnections = currentActiveNode.GetComponent<NodeConnections>();
                if (currentActiveNodeConnections != null && currentActiveNodeConnections.connectedNodes.ContainsKey(myConnections))
                {
                    // Work out the time to pass based off the distance and the engine speed
                    float currentEngineSpeed = Defs.Instance.engineUpgradesSpeeds[PlayerCargo.Instance.GetCurrentEngine()];
                    timeToThisNode = (int)(currentActiveNodeConnections.connectedNodes[myConnections] * currentEngineSpeed);
                }
            }

            if (jumpPanelText != null) { jumpPanelText.text = timeToThisNode.ToString(); }
            jumpTimePanel.SetActive(true);
            jumpPanelActive = true;
        }
    }

    private void OnMouseExit()
    {
        if (activeNodeScript.getRingActive())
        {
            jumpTimePanel.SetActive(false);
            jumpPanelActive = false;
        }
    }
}
