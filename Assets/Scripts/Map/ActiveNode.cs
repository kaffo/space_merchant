using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveNode : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject ship;
    public GameObject ring;
    public GameObject connections;

    [Header("Materials for Ring")]
    public Material ringHilightMaterial;
    public Material ringAdjacentMaterial;

    [Header("UI Elements")]
    public UnityEngine.UI.Button jumpButton;
    public GameObject goodsParent;

    [Header("Script References")]
    public NodeConnections nodeConnectionsScript;

    private bool isActiveNode = false;

    private bool isRingActive = false;

    public void setActive(bool active)
    {
        isActiveNode = active;
        ship.SetActive(active);

        // Tell ObjectManager if I'm the new active node
        if (active) { ObjectManager.Instance.currentActiveNode = this; }

        // Game over, so don't do any game logic
        if (TimeCounter.Instance.gameOver) { return; }

        // Enable/disable the adjacent nodes as clickable
        NodeConnections myNodeConnections = GetComponent<NodeConnections>();
        if (myNodeConnections != null)
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
        Defs.ConnectionTypes connectionType = ObjectManager.Instance.currentActiveNode.nodeConnectionsScript.GetConnectionType(this.nodeConnectionsScript);

        // Check a connection exists
        if (connectionType < 0)
        {
            Debug.LogError("Trying to activate node with no connection on " + gameObject.name);
            return;
        } 

        // If it's a green connection, check the player has a green engine
        if (connectionType == Defs.ConnectionTypes.CONNECTIONTYPE_GREEN && PlayerCargo.Instance.GetCurrentEngine() != Defs.EngineUpgrades.ENGINEUPRADE_GREEN_ONE)
        {
            Debug.Log("Can't jump to " + gameObject.name + " without Green engine");
            return;
        }

        isRingActive = active;
        jumpButton.interactable = active;
        if (active)
        {
            ring.GetComponent<MeshRenderer>().material = ringAdjacentMaterial;
        } else
        {
            ring.GetComponent<MeshRenderer>().material = ringHilightMaterial;
        }
    }

    public bool getRingActive()
    {
        return isRingActive;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ship == null || ring == null || connections == null || nodeConnectionsScript == null || ringHilightMaterial == null || ringAdjacentMaterial == null
            || jumpButton == null || goodsParent == null)
        {
            Debug.LogError("Active Node Script error on " + gameObject.name);
            this.enabled = false;
            return;
        }

        ship.SetActive(isActiveNode);
        ObjectManager.Instance.globalNodeList.Add(this);
    }
}
