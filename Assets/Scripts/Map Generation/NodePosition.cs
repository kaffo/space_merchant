using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Node Positioning Script
 * 
 */

[ExecuteInEditMode]
public class NodePosition : MonoBehaviour
{
    public bool startArrange = false;
    public bool stopArrange = false;
    public GameObject mapParentObject;

    private bool doArrange = false;
    private int arrangeFrameCount = 0;

    private NodeConnections[] nodesToArrange;

    // Start is called before the first frame update
    void Start()
    {
        if (mapParentObject == null)
        {
            Debug.LogError(gameObject.name + " " + this.name + " is invalid");
            this.enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        EditorApplication.update += UpdateNodePosition;
    }

    public void ArrangeNodes()
    {
        startArrange = false;
        if (doArrange == false)
        {
            doArrange = true;
            arrangeFrameCount = 0;
            nodesToArrange = mapParentObject.GetComponentsInChildren<NodeConnections>();
        } else
        {
            Debug.LogError("Still Arranging!!");
        }
        
    }

    // Update is called once per frame
    void UpdateNodePosition()
    {
        if (startArrange)
        {
            ArrangeNodes();
        }
        if (doArrange)
        {
            arrangeFrameCount++;
            foreach (NodeConnections nodeConnectionScript in nodesToArrange)
            {
                // Make the first node static so we have a reference point
                if (nodeConnectionScript == nodesToArrange[0]) { continue; }
                // Final vector we want our current node to travel
                Vector3 compositeVector = new Vector3();
                // Check each connection this node has
                for (int i = 0; i < nodeConnectionScript.nodesToConnect.Count; i++)
                {
                    NodeConnections otherNodeConnectionScript = nodeConnectionScript.nodesToConnect[i];
                    Transform otherNodeTransform = otherNodeConnectionScript.transform;

                    // Work out the position vector of the desired end position
                    Vector3 vectorToDesiredPosition = (nodeConnectionScript.transform.position - otherNodeTransform.position).normalized * (nodeConnectionScript.timeToTravel[i] / 10);
                    Vector3 desiredPosition = vectorToDesiredPosition + otherNodeTransform.position;

                    // Pull our node in the direction of the other node, trying to make sure the distance between them is the jump cost
                    compositeVector = compositeVector + (desiredPosition - nodeConnectionScript.transform.position).normalized * 0.1f;
                }
                nodeConnectionScript.transform.Translate(compositeVector);
            }
            if (stopArrange)
            {
                Debug.Log("Stopping...");
                stopArrange = false;
                doArrange = false;
            }
        }
    }
}
