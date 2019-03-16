using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIReferences : MonoBehaviour
{
    [Header("Upgrade Setup")]
    public Defs.EngineUpgrades upgrade;

    [Header("UI Elements")]
    public Text upgradeNameText;
    public Text buyText;
    public Button buyButton;
}
