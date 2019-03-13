using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePanelReferences : MonoBehaviour
{
    [Header("UI Elements")]
    public Button jumpButton;
    public GameObject goodsParent;
    public Text nodeNameText;
    public RectTransform panelTransfrom;

    // Start is called before the first frame update
    void Start()
    {
        if (jumpButton == null || nodeNameText == null || goodsParent == null || panelTransfrom == null)
        {
            Debug.LogError(gameObject.name + " Node Panel Reference script is invalid");
            this.enabled = false;
            return;
        }
    }
}
