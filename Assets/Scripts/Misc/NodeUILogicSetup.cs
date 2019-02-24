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
    public NodeGoods nodeGoods;

    [Header("Prefabs")]
    public Button goodsButton; 

    private TimeCounter timeCount;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (jumpButton == null || nodeScript == null || myConnections == null || managerFinder == null 
            || nodeGoods == null || goodsParent == null || goodsButton == null)
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

        float currentHeight = 0f;
        foreach (var good in nodeGoods.goodPrices.Keys)
        {
            Button newButton = Instantiate<Button>(goodsButton, goodsParent.transform);
            Transform buttonTextTransform = newButton.transform.GetChild(0);
            Text buttonText = buttonTextTransform.GetComponent<Text>();
            if (buttonText != null) { buttonText.text = nodeGoods.goodNames[good] + " - $" + nodeGoods.goodPrices[good]; }
            newButton.transform.localPosition = new Vector3(0f, -currentHeight, 0f);
            currentHeight += 50f; //TODO make this not static
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
