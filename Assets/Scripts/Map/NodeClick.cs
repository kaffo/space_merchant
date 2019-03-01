using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeClick : MonoBehaviour
{
    public GameObject nodePanel;

    public ActiveNode activeNodeScript;
    public NodeConnections myConnections;

    private void Start()
    {

        if (nodePanel == null  || activeNodeScript == null || myConnections == null)
        {
            Debug.LogError("NodeClick error on " + transform.parent.name);
            this.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        nodePanel.SetActive(!nodePanel.activeInHierarchy);
    }
}
