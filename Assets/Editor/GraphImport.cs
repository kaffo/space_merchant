using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class GraphImport : EditorWindow
{
    public GameObject nodePrefab;

    private static string startFilePath;

    private string fileName;
    private Dictionary<string, Dictionary<string, int>> graphDict;
    private Dictionary<string, NodeConnections> nodeConnectionsDict;

    private GameObject mapParent;

    [MenuItem("Graph Tools/Graph Import")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GraphImport));
        string[] filter = { "Graph Files", "ncol" };
        string path = EditorUtility.OpenFilePanelWithFilters("Load Graph File", "", filter);
        startFilePath = path;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("INPUT FILE MUST BE ORDERED");
        fileName = startFilePath;
        fileName = EditorGUILayout.TextField("File Name:", fileName);
        if (GUILayout.Button("Load"))
        {
            graphDict = new Dictionary<string, Dictionary<string, int>>();
            nodeConnectionsDict = new Dictionary<string, NodeConnections>();
            mapParent = GameObject.FindGameObjectWithTag("MapParent");

            string[] lines = System.IO.File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineSplit = lines[i].Split(' ');
                string currentNode = lineSplit[0];
                string otherNode = lineSplit[1];
                int weight = 0;
                if (lineSplit.Length > 2)
                {
                    weight = Int32.Parse(lineSplit[2]);
                }

                if (!graphDict.ContainsKey(currentNode))
                {
                    graphDict.Add(currentNode, new Dictionary<string, int>());
                    GameObject nodeGameObject = Instantiate(nodePrefab, mapParent.transform);
                    nodeGameObject.name = currentNode;
                    NodeConnections nodeConnectionsScript = nodeGameObject.GetComponent<NodeConnections>();
                    nodeConnectionsDict.Add(currentNode, nodeConnectionsScript);
                    // Don't move the first node
                    if (i > 0)
                    {
                        nodeGameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0f);
                    }
                }

                graphDict[currentNode].Add(otherNode, weight);
            }

            List<string> keyList = new List<string>(graphDict.Keys);
            foreach (string nodeValue in keyList)
            {
                Dictionary<string, int> graphConnections = graphDict[nodeValue];
                foreach (string nodeValueToConnect in graphConnections.Keys)
                {
                    // Check if there are any missing nodes from the list due to one way connections
                    if (!nodeConnectionsDict.ContainsKey(nodeValueToConnect))
                    {
                        graphDict.Add(nodeValueToConnect, new Dictionary<string, int>());
                        GameObject nodeGameObject = Instantiate(nodePrefab, mapParent.transform);
                        nodeGameObject.name = nodeValueToConnect;
                        NodeConnections nodeConnectionsScript = nodeGameObject.GetComponent<NodeConnections>();
                        nodeConnectionsDict.Add(nodeValueToConnect, nodeConnectionsScript);
                        nodeGameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0f);
                    }
                }
            }

            foreach (string nodeValue in graphDict.Keys)
            {
                Dictionary<string, int> graphConnections = graphDict[nodeValue];
                NodeConnections nodeConnectionsScript = nodeConnectionsDict[nodeValue];
                foreach (string nodeValueToConnect in graphConnections.Keys)
                {
                    NodeConnections otherConnectionScript = nodeConnectionsDict[nodeValueToConnect];
                    nodeConnectionsScript.nodesToConnect.Add(otherConnectionScript);
                    nodeConnectionsScript.timeToTravel.Add(graphConnections[nodeValueToConnect]);
                    nodeConnectionsScript.nodeConnectionTypes.Add(Defs.ConnectionTypes.CONNECTIONTYPE_DEFAULT);
                    nodeConnectionsScript.costToTravel.Add(0);

                    // NCOL sucks so have to add reference back
                    otherConnectionScript.nodesToConnect.Add(nodeConnectionsScript);
                    otherConnectionScript.timeToTravel.Add(graphConnections[nodeValueToConnect]);
                    otherConnectionScript.nodeConnectionTypes.Add(Defs.ConnectionTypes.CONNECTIONTYPE_DEFAULT);
                    otherConnectionScript.costToTravel.Add(0);
                }
            }
        }
    }
}
