using System;
using System.Collections;
using System.Collections.Generic;


public class Graph<T> where T : class // T must be an ADT (nullable)
{
    #region GRAPHNODE
    public class GraphNode
    {
        public T Data { get; protected set; }

        protected List<GraphNode> senders;    // Nodes directly connect to
        protected List<GraphNode> receivers;  // Nodes that I "receive" from

        public GraphNode(ref T _data)
        {
            Data = _data;
            senders = new List<GraphNode>();
            receivers = new List<GraphNode>();
        }


        // Connect to specific node
        public void ConnectTo(ref GraphNode _dt)
        {
            if (CheckExistingSender(ref _dt) == true)
                return;

            senders.Add(_dt);
        }

        // Make sure don't add an existing sender node
        public bool CheckExistingSender(ref GraphNode _gn)
        {
            foreach (GraphNode temp in senders)
            {
                if (temp.Equals(_gn))
                    return true;
            }
            return false;
        }

        public void StopConnectionTo(ref GraphNode _dt)
        {
            if (CheckExistingSender(ref _dt) == false)
                return;

            senders.Remove(_dt);
        }

        // Receive a connection for another node
        public void ReceiveFrom(ref GraphNode _sc)
        {
            if (CheckExistingReceiver(ref _sc) == true)
                return;

            receivers.Add(_sc);
        }

        // Make sure we're not already receiving from another node
        public bool CheckExistingReceiver(ref GraphNode _gn)
        {
            foreach (GraphNode temp in receivers)
            {
                if (temp.Equals(_gn))
                    return true;
            }
            return false;
        }

        public void StopReceivingFrom(ref GraphNode _sc)
        {
            if (CheckExistingReceiver(ref _sc) == false)
                return;

            receivers.Remove(_sc);
        }

        public void RemoveAllConnections()
        {
            senders.Clear();
            receivers.Clear();
        }

    } // end class GraphNode

    #endregion

    protected List<GraphNode> nodes;
    public int Count { get; protected set; }

    // Constructor with optional size
    public Graph(int initialSize = 0)
    {
        Count = initialSize;
        nodes = new List<GraphNode>(Count);
    }

    // Find an existing node in the graph
    public GraphNode FindNode(ref T _data)
    {
        GraphNode node = null;

        if (Count == 0)
            return node;

        foreach (GraphNode n in nodes)
        {
            if (n.Data.Equals(_data))
            {
                node = n;
                break;
            }
        }
        return node;
    }

    #region ADD NODE / DIRECTION funcitons
    // Add node to graph
    public void AddNode(ref T _data)
    {
        GraphNode node = new GraphNode(ref _data);
        nodes.Add(node);
        Count++;
    }

    // Add a one way connection b/n two nodes 
    void AddDirectedEdge(ref GraphNode _srcNode, ref GraphNode _dstNode)
    {
        // Make sure we're not connecting the same node to itself
        if (_srcNode == null || _dstNode == null || _srcNode.Equals(_dstNode))
            return;

        _srcNode.ConnectTo(ref _dstNode);
        _dstNode.ReceiveFrom(ref _srcNode);
    }

    // A public AddDirectedEdge in a easier fashion with data instead of nodes
    public void AddDirectedEdge(ref T _src, ref T _dst)
    {
        GraphNode srcNode = FindNode(ref _src);
        GraphNode dstNode = FindNode(ref _dst);

        AddDirectedEdge(ref srcNode, ref dstNode);
    }

    // Quick way of adding a two way bidirectional connection b/n two nodes
    public void AddBidirectionalEdge(ref T _nodeA, ref T _nodeB)
    {
        GraphNode aNode = FindNode(ref _nodeA);
        GraphNode bNode = FindNode(ref _nodeB);

        AddDirectedEdge(ref aNode, ref bNode);
        AddDirectedEdge(ref bNode, ref aNode);
    }
    #endregion

    #region REMOVE NODE / DIRECTION functions

    public void RemoveNode(ref T _data)
    {
        GraphNode node = FindNode(ref _data);
        node.RemoveAllConnections();
        nodes.Remove(node);

        // Remove the connections of the deleted one to the remaining nodes
        foreach (GraphNode nd in nodes)
        {
            nd.StopConnectionTo(ref node);
            nd.StopReceivingFrom(ref node);
        }
    }

    public void RemoveDirection(ref T sc, ref T dc)
    {
        GraphNode _src = FindNode(ref sc);
        GraphNode _dst = FindNode(ref dc);

        _src.StopConnectionTo(ref _dst);
        _dst.StopReceivingFrom(ref _src);
    }

    public void RemoveBidirection(ref T _nodeA, ref T _nodeB)
    {
        GraphNode aNode = FindNode(ref _nodeA);
        GraphNode bNode = FindNode(ref _nodeB);

        aNode.StopConnectionTo(ref bNode);
        aNode.StopReceivingFrom(ref bNode);

        bNode.StopConnectionTo(ref aNode);
        bNode.StopReceivingFrom(ref aNode);
    }

    #endregion

}  // end class Graph<T>
