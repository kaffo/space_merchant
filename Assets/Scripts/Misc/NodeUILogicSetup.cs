using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUILogicSetup : MonoBehaviour
{
    [Header("UI Elements")]
    public Button jumpButton;
    public GameObject goodsParent;

    [Header("Scripts")]
    public ActiveNode nodeScript;
    public NodeConnections myConnections;
    public GameManagerFinder managerFinder;

    [Header("Prefabs")]
    public GameObject goodsUIParent; 

    private TimeCounter timeCount;
    private PlayerMoney playerMoney;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (jumpButton == null || nodeScript == null || myConnections == null || managerFinder == null 
            || goodsParent == null || goodsUIParent == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
        }

        jumpButton.onClick.AddListener(JumpButtonClick);

        gameManager = managerFinder.gameManager;
        timeCount = gameManager.GetComponent<TimeCounter>();
        playerMoney = gameManager.GetComponent<PlayerMoney>();

        if (timeCount == null || playerMoney == null)
        {
            Debug.LogError("NodeClick error on " + transform.parent.name);
            this.enabled = false;
        }

        float currentHeight = 0f;
        foreach (Defs.TradeGoods good in Enum.GetValues(typeof(Defs.TradeGoods)))
        {
            // Make a new prefab for each good
            GameObject goodsPrefab = Instantiate<GameObject>(goodsUIParent, goodsParent.transform);
            GameObject buyButtonGO = null;
            GameObject sellButtonGO = null;

            // Move the prefab so it lists 
            goodsPrefab.transform.localPosition = new Vector3(0f, -currentHeight, 0f);
            currentHeight += 50f; //TODO make this not static

            // Set the Good script up
            Good goodScript = goodsPrefab.GetComponent<Good>();
            if (goodScript != null) {
                goodScript.good = good;
                goodScript.enabled = true;
            }

            for (int i = 0; i < goodsPrefab.transform.childCount; i++)
            {
                Transform currentChild = goodsPrefab.transform.GetChild(i);
                if (currentChild.name == "GoodButtonBuy") { buyButtonGO = currentChild.gameObject; }
                if (currentChild.name == "GoodButtonSell") { sellButtonGO = currentChild.gameObject; }
            }

            if (buyButtonGO == null || sellButtonGO == null)
            {
                Debug.LogError("Button setup error on " + gameObject.name);
                this.enabled = false;
                return;
            }

            Button buyButton = buyButtonGO.GetComponent<Button>();
            Button sellButton = sellButtonGO.GetComponent<Button>();

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

    private void GoodsBuyButtonClicked(Good goodScript)
    {
        goodScript.BuyGood(1);
    }

    private void GoodsSellButtonClicked(Good goodScript)
    {
        goodScript.SellGood(1);
    }
}
