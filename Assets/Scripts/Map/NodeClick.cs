using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClick : MonoBehaviour
{
    public GameObject nodePanel;

    private void Start()
    {
        if (nodePanel == null)
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
