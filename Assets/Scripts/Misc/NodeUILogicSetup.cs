using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUILogicSetup : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject mapParentObject;

    [Header("UI Elements")]
    public Button jumpButton;
    public GameObject goodsParent;
    public Text nodeNameText;

    [Header("Scripts")]
    public ActiveNode nodeScript;
    public NodeConnections myConnections;
    public NodeClick nodeClickScript;

    [Header("Prefabs")]
    public GameObject goodsUIParent;
    public GameObject panelPrefab;

    private NodePanelReferences myPanelReferenceScript;

    // Start is called before the first frame update
    void Start()
    {
        if (nodeScript == null || myConnections == null || goodsUIParent == null || panelPrefab == null || mapParentObject == null
            || nodeClickScript == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }

        GameObject myPanelObject = Instantiate(panelPrefab, mapParentObject.transform);
        myPanelObject.transform.position = transform.position;

        myPanelReferenceScript = myPanelObject.GetComponent<NodePanelReferences>();
        if(myPanelReferenceScript == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }

        jumpButton = myPanelReferenceScript.jumpButton;
        goodsParent = myPanelReferenceScript.goodsParent;
        nodeNameText = myPanelReferenceScript.nodeNameText;

        if (jumpButton == null || nodeNameText == null || goodsParent == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }

        nodeClickScript.nodePanel = myPanelObject;
        nodeScript.jumpButton = jumpButton;
        nodeScript.goodsParent = goodsParent;

        nodeNameText.text = gameObject.name;

        jumpButton.onClick.AddListener(JumpButtonClick);

        float currentHeight = 0f;
        foreach (Defs.TradeGoods good in Enum.GetValues(typeof(Defs.TradeGoods)))
        {
            // Make a new prefab for each good
            GameObject goodsPrefab = Instantiate<GameObject>(goodsUIParent, goodsParent.transform);

            // Move the prefab so it lists 
            goodsPrefab.transform.localPosition = new Vector3(0f, -currentHeight, 0f);
            currentHeight += 100f; //TODO make this not static

            // Set the Good script up
            Good goodScript = goodsPrefab.GetComponent<Good>();
            if (goodScript != null) {
                goodScript.good = good;
                goodScript.activeNodeScript = nodeScript;
                goodScript.enabled = true;
            }

            Button buyButton = goodScript.buyButton;
            Button sellButton = goodScript.sellButton;

            if (buyButton == null || sellButton == null)
            {
                Debug.LogError("Button setup error on " + gameObject.name);
                this.enabled = false;
                return;
            }

            // Setup the click logic
            buyButton.onClick.AddListener(() => GoodsBuyButtonClicked(goodScript));
            sellButton.onClick.AddListener(() => GoodsSellButtonClicked(goodScript));
        }
    }


    private void JumpButtonClick()
    {
        if (nodeScript.getRingActive() && !TimeCounter.Instance.gameOver)
        {
            // Get the current active node and disable it
            ActiveNode currentActiveNode = ObjectManager.Instance.currentActiveNode;

            if (currentActiveNode != null && currentActiveNode.getActive())
            {
                currentActiveNode.setActive(false);

                NodeConnections currentActiveNodeConnections = currentActiveNode.GetComponent<NodeConnections>();
                if (currentActiveNodeConnections != null && currentActiveNodeConnections.connectedNodes.ContainsKey(myConnections))
                {
                    TimeCounter.Instance.passTime(currentActiveNodeConnections.connectedNodes[myConnections]);
                } else
                {
                    Debug.LogError("Error getting Current Active Node Connections on " + transform.parent.name);
                }
            }

            //Enable myself
            nodeScript.setActive(true);

            // Update all Goods UI
            foreach (var good in ObjectManager.Instance.globalGoodList)
            {
                good.updateUI();
            }
        }
    }

    private void GoodsBuyButtonClicked(Good goodScript)
    {
        goodScript.BuyGood(1);
    }

    private void GoodsSellButtonClicked(Good goodScript)
    {
        goodScript.SellGood(1);
    }
}
