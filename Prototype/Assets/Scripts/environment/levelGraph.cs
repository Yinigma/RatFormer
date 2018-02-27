using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGraph : MonoBehaviour {

    private struct Node
    {
        private int id;
        Vector2 pos;
        private List<Node> adjacent;
        public Node(uint id, Vector2 pos)
        {
            this.id = (int)id;
            this.pos = pos;
            adjacent = new List<Node>();
        }
        public void addNode(ref Node n)
        {
            adjacent.Add(n);
        }
        public void removeNode(int ID) 
        {
            int dex = 0;
            foreach(Node n in adjacent)
            {
                if (n.ID == ID)
                {
                    adjacent.RemoveAt(dex);
                    break;
                }
                dex++;
            }
        }
        public int ID { get { return id; } }
        public Vector2 Position { get { return pos; } }
        public Node[] Adjacents { get{ return adjacent.ToArray(); } }
    }


    private static List<platformData> rawData;
    private static List<Node> nodes;
    private uint numNodes;


    public static void addEntry(platformData data)
    {
        if (rawData != null) {
            rawData.Add(data);
        }
    }

    void Awake()
    {
        rawData = new List<platformData>();
        nodes = new List<Node>();
    }
    void Start()
    {
        numNodes = 0;
        foreach(platformData pd in rawData)
        {
            int dex = 0;
            Node prev = new Node(0, Vector2.zero);
            Node current;
            foreach (Vector2 v in pd.getPoints())
            {
                current= new Node(numNodes, v);
                nodes.Add(current);
                if (dex != 0)
                {
                    current.addNode(ref prev);
                    prev.addNode(ref current);
                }
                prev = current;
                numNodes++;
                dex++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (nodes != null)
        {
            foreach (Node n in nodes)
            {
                Gizmos.DrawSphere(n.Position, 0.1f);
                foreach(Node a in n.Adjacents)
                {
                    Gizmos.DrawLine(n.Position, a.Position);
                }
            }
        }
    }
}
