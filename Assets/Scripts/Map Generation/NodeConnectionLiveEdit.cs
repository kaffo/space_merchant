using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class NodeConnectionLiveEdit : MonoBehaviour
{
    public GameObject mapParentObject;

    private void Awake()
    {
        Debug.Log("Awake Live Edit");
        this.enabled = false;
    }

    private void OnEnable()
    {
        Debug.Log("Enable Live Edit");
        if (mapParentObject == null)
        {
            Debug.LogError(this.name + " setup error!");
            this.enabled = false;
            return;
        }
        /* Edit Node Connections
        NodeConnections[] nodeConnectionsList = mapParentObject.GetComponentsInChildren<NodeConnections>();
        if (nodeConnectionsList.Length > 0)
        {
            foreach (NodeConnections currentConnectionScript in nodeConnectionsList)
            {
                for (int i = 0; i < currentConnectionScript.nodesToConnect.Count; i++)
                {
                    currentConnectionScript.timeToTravel[i] = currentConnectionScript.timeToTravel[i] / 2;
                }
            }
        } else
        {
            Debug.Log("No Nodes Found");
        }
        */

        NodeParameters[] nodeParametersList = mapParentObject.GetComponentsInChildren<NodeParameters>();
        if (nodeParametersList.Length > 0)
        {
            foreach (NodeParameters currentParameterScript in nodeParametersList)
            {
                Defs.TradeGoods[] tradeGoods = { Defs.TradeGoods.GOOD_FOOD };
                currentParameterScript.tradeGoodsToSell = tradeGoods;
            }
        }
        else
        {
            Debug.Log("No Nodes Found");
        }

        Debug.Log("Complete Live Edit");
        this.enabled = false;
    }
}
