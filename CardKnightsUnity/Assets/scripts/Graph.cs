using System;
using System.Collections;
using System.Collections.Generic;

 
public class Graph<T> where T : class // T must be an ABT (nullable)
{
    public class GraphNode
    {
        public T Data { get; protected set; }

        protected List<GraphNode> senders;    // Nodes directly connect to
        protected List<GraphNode> receivers;  // Nodes that I "receive" from

        public GraphNode(ref T data)
        {
            Data = data;
            senders = new List<GraphNode>();
            receivers = new List<GraphNode>();
        }

        public void ConnectTo(ref GraphNode _dt)
        {
            senders.Add(_dt);
        }

        public void ReceiveFrom(ref GraphNode _sc)
        {
            receivers.Add(_sc);
        }
    }

    public int Count { get; protected set; }
    protected List<GraphNode> nodes;

    public Graph(int initialSize = 0)
    {
        Count = initialSize;
        nodes = new List<GraphNode>(Count);
    }

    public void AddNode(ref T _data)
    {
        GraphNode node = new GraphNode(ref _data);
        nodes.Add(node);
    }

    public void AddEdge(ref GraphNode _srcNode, ref GraphNode _dstNode)
    {
        if (_srcNode == null || _dstNode == null)
            return;

        _srcNode.ConnectTo(ref _dstNode);
        _dstNode.ReceiveFrom(ref _srcNode);
    }

    public void AddEdge(ref T _src, ref T _dst)
    {
        AddEdge(ref _src, ref _dst);
    }

    public T FindNode(ref T _data)
    {
        T val = null; 
        foreach (GraphNode nd in nodes)
        {
            if (nd.Data.Equals(_data))
                val = nd.Data;
        }
        return val;
    }
    
}  // end class Graph<T>