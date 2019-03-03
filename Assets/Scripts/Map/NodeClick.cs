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
        // Make sure the user is allowed to drag if their mouse was over the panel when they close it
        if (!nodePanel.activeInHierarchy) { Camera.main.GetComponent<CameraControl>().allowStartMouseDrag = true; }
    }
}
