using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodUIReferences : MonoBehaviour
{
    [Header("UI Elements")]
    public Text goodText;
    public Text buyText;
    public Button buyButton;
    public Text sellText;
    public Button sellButton;
    public Outline sellButtonOutline;

    [Header("UI Scripts")]
    public BuyButtonUIHandler buyButtonUIHandler;
}
