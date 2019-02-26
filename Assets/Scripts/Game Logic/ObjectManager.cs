using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public ActiveNode currentActiveNode;
    public GameObject jumpTimePanel;
    public List<Good> globalGoodList = new List<Good>();
}
