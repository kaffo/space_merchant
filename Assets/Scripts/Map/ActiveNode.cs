using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveNode : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject ship;
    public GameObject ring;

    [Header("Materials for Ring")]
    public Material ringHilightMaterial;
    public Material ringAdjacentMaterial;

    [Header("UI Elements")]
    public UnityEngine.UI.Button jumpButton;
    public GameObject goodsParent;

    [Header("Game Manager")]
    public GameManagerFinder managerFinder;

    private bool isActiveNode = false;

    private bool isRingActive = false;

    private TimeCounter timeCounter;

    public void setActive(bool active)
    {
        isActiveNode = active;
        ship.SetActive(active);

        // Game over, so don't do any game logic
        if (timeCounter.gameOver) { return; }

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

        // Enable/disable trade buttons
        for (int i = 0; i < goodsParent.transform.childCount; i++)
        {
            Transform goodParent = goodsParent.transform.GetChild(i);
            for (int j = 0; j < goodParent.childCount; j++)
            {
                UnityEngine.UI.Button goodButton = goodParent.GetChild(j).GetComponent<UnityEngine.UI.Button>();
                if (goodButton != null) { goodButton.interactable = active; }
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
        if (ship == null || ring == null || managerFinder == null || ringHilightMaterial == null || ringAdjacentMaterial == null || jumpButton == null
            || goodsParent == null)
        {
            Debug.LogError("Active Node Script error on " + gameObject.name);
            this.enabled = false;
            return;
        }

        ship.SetActive(isActiveNode);

        timeCounter = managerFinder.gameManager.GetComponent<TimeCounter>();
    }
}
