using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class UpgradeSystemAssistant : EditorWindow
{
    private List<Vector3[]> positions = new List<Vector3[]>();
    private Dictionary<GameObject, bool> dirtyLineDict = new Dictionary<GameObject, bool>();

    [MenuItem("Window/Upgrade System Assistant")]
    public static void ShowWindow()
    {
        GetWindow<UpgradeSystemAssistant>("Upgrade System Assistant");
    }

    private void OnGUI()
    {
        GUILayout.Label("\nTools to assist in developing the Unit Upgrade System.\n", EditorStyles.boldLabel);

        if (GUILayout.Button("Connect Two Nodes"))
        {
            ConnectNodes();
        }

        if (GUILayout.Button("Sever Two Nodes"))
        {
            SeverNodes();
        }

        if (GUILayout.Button("Visualize Children Node Connections"))
        {
            PopulateNodeLines();
        }

        if (GUILayout.Button("Get Node Totals"))
        {
            GetNodeTotals();
        }

    }

    void OnEnable()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    void OnDisable()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if(positions.Count == 0)
        {
            return;
        }

        //Draw lines from our positions list
        foreach (Vector3[] array in positions)
        {
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(10f, array);
        }
    }

    private void PopulateNodeLines()
    {
        positions.Clear();
        dirtyLineDict.Clear();

        if(Selection.count == 0)
        {
            Debug.Log("<color=red>Error:</color> Nothing selected to visualize.");
            return;
        }

        List<Transform> childObjs = new List<Transform>();
        //Populate all children into a list
        foreach (GameObject obj in Selection.gameObjects)
        {
            Transform[] objArray = obj.GetComponentsInChildren<Transform>();
            childObjs.AddRange(objArray);
        }

        foreach (Transform nodeTransform in childObjs)
        {
            dirtyLineDict.Add(nodeTransform.gameObject, false);
        }

        //Go through all nodes in the list, check their connected nodes, and then add that to the final line list assuming all conditions are met
        foreach (Transform obj in childObjs)
        {
            if (obj.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
            {
                foreach (GameObject connectedNode in iupgrade.connectedNodes)
                {
                    //Don't add  null nodes
                    if (connectedNode == null)
                    {
                        continue;
                    }

                    if (dirtyLineDict.ContainsKey(connectedNode) == false)
                    {
                        dirtyLineDict.Add(connectedNode, false);
                    }

                    if (dirtyLineDict[connectedNode] == false)
                    {
                        Vector3[] pos = { obj.gameObject.transform.position, connectedNode.transform.position };
                        positions.Add(pos);
                    }

                }
                dirtyLineDict[obj.gameObject] = true;
            }
        }
    }

    private void ConnectNodes()
    {
        if (Selection.count != 2)
        {
            Debug.Log("<color=red>Error:</color> This tool only works with two selected nodes!");
            return;
        }

        GameObject obj1 = (Selection.gameObjects[0]);
        GameObject obj2 = (Selection.gameObjects[1]);

        List<GameObject> obj1NodeList = null;
        List<GameObject> obj2NodeList = null;
        if(obj1.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade upgradeNode))
        {
            obj1NodeList = upgradeNode.connectedNodes;
        }

        if (obj2.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade upgradeNode2))
        {
            obj2NodeList = upgradeNode2.connectedNodes;
        }

        if(obj1NodeList == null || obj2NodeList == null)
        {
            Debug.Log("<color=red>Error:</color> One or both of your selected game objects is not an upgrade node!");
            return;
        }

        //Go through the connected nodes list of each game object and make sure they aren't already connected to each other.
        foreach(GameObject obj in obj1NodeList)
        {
            if(obj == obj2)
            {
                Debug.Log("<color=red>Error:</color> These two nodes are already connected!");
                return;
            }
        }

        foreach (GameObject obj in obj2NodeList)
        {
            if (obj == obj1)
            {
                Debug.Log("<color=red>Error:</color> These two nodes are already connected!");
                return;
            }
        }

        //If it passes every check, add the connections!
        upgradeNode.connectedNodes.Add(obj2);
        upgradeNode2.connectedNodes.Add(obj1);
        Debug.Log("<color=green>Nodes paired successfully!</color>");
    }

    private void SeverNodes()
    {
        if (Selection.count != 2)
        {
            Debug.Log("<color=red>Error:</color> This tool only works with two selected nodes!");
            return;
        }

        GameObject obj1 = (Selection.gameObjects[0]);
        GameObject obj2 = (Selection.gameObjects[1]);

        List<GameObject> obj1NodeList = null;
        List<GameObject> obj2NodeList = null;
        if (obj1.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade upgradeNode))
        {
            obj1NodeList = upgradeNode.connectedNodes;
        }

        if (obj2.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade upgradeNode2))
        {
            obj2NodeList = upgradeNode2.connectedNodes;
        }

        if (obj1NodeList == null || obj2NodeList == null)
        {
            Debug.Log("<color=red>Error:</color> One or both of your selected game objects is not an upgrade node!");
            return;
        }

        bool check1 = false;
        bool check2 = false;

        for(int i = 0; i < obj1NodeList.Count; i++)
        {
            if (obj1NodeList[i] == obj2)
            {
                upgradeNode.connectedNodes.RemoveAt(i);
                check1 = true;
                break;
            }
        }

        for (int i = 0; i < obj2NodeList.Count; i++)
        {
            if (obj2NodeList[i] == obj1)
            {
                upgradeNode2.connectedNodes.RemoveAt(i);
                check2 = true;
                break;
            }
        }

        if (check1 && check2)
        {
            Debug.Log("<color=green>Both nodes unpaired successfully!</color>");
            return;
        }

        if(check1 || check2)
        {
            Debug.Log("<color=green>Severed a loose node connected of the two selected nodes.</color>");
            return;
        }

        if(!check1 && !check2)
        {
            Debug.Log("<color=red>Error:</color> No nodes to sever!");
            return;
        }
    }

    private void GetNodeTotals()
    {
        if (Selection.count == 0)
        {
            Debug.Log("<color=red>Error:</color> Nothing selected to check.");
            return;
        }

        List<Transform> childObjs = new List<Transform>();
        //Populate all children into a list
        foreach (GameObject obj in Selection.gameObjects)
        {
            Transform[] objArray = obj.GetComponentsInChildren<Transform>();
            childObjs.AddRange(objArray);
        }

        //This dict will store all of our minor node effects and their corresponding values for each node in our childObjs list
        Dictionary<string, double> effectsAndVals = new Dictionary<string, double>();

        foreach (Transform obj in childObjs)
        {
            if (obj.TryGetComponent<MinorUpgradeNode>(out MinorUpgradeNode mNode))
            {
                Dictionary<string, double> dict = mNode.GetEffectAndAmount();
                var first = dict.First(); //Linq is OP

                //If the key already exists, add the value to the existing key
                if(effectsAndVals.ContainsKey(first.Key))
                {
                    effectsAndVals[first.Key] += first.Value;
                    continue;
                }

                effectsAndVals.Add(first.Key, first.Value);
            }
        }

        int nodeCount = 0;

        foreach (Transform obj in childObjs)
        {
            if (obj.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iUp))
            {
                nodeCount++;
            }
        }

        string result = string.Empty;
        result += "Printing results for your selection:\n";
        result += "Total Nodes Selected: " + nodeCount + "\n";

        foreach (KeyValuePair<string, double> pair in effectsAndVals)
        {
            result += (pair.Key + ": " + pair.Value + "\n");
        }

        Debug.Log(result);
    }

}
