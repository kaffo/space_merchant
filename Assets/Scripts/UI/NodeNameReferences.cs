using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeNameReferences : MonoBehaviour
{
    [Header("UI Elements")]
    public Text nodeNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (nodeNameText == null)
        {
            Debug.LogError(gameObject.name + " UI setup script is invalid");
            this.enabled = false;
            return;
        }
    }
}
