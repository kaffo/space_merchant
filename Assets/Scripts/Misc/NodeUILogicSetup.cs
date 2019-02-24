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
    public Button goodsButton; 

    private TimeCounter timeCount;
    private PlayerMoney playerMoney;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (jumpButton == null || nodeScript == null || myConnections == null || managerFinder == null 
            || goodsParent == null || goodsButton == null)
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
            // Make a new button for each good
            Button newButton = Instantiate<Button>(goodsButton, goodsParent.transform);

            // Set the Good script up
            Good goodScript = newButton.GetComponent<Good>();
            if (goodScript != null) {
                goodScript.good = good;
                goodScript.enabled = true;
            }

            // Move the button so it lists 
            newButton.transform.localPosition = new Vector3(0f, -currentHeight, 0f);
            currentHeight += 50f; //TODO make this not static

            // Setup the click logic
            newButton.onClick.AddListener(GoodsBuyButtonClicked);
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

    private void GoodsBuyButtonClicked()
    {
        playerMoney.incrementPlayerCash(-50);
    }
}
