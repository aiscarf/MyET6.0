using System;
using System.Collections.Generic;

// using UnityEngine;

namespace XNode
{
    /// <summary> Base class for all node graphs </summary>
    [Serializable]
    public class NodeGraph
    {
        // public string name;

        [UnityEngine.SerializeField]
        public int Id;

        [UnityEngine.SerializeField]
        public string TypeQualifiedName;

        [UnityEngine.SerializeField]
        public List<Node> nodes = new List<Node>();

        public Node GetNode(int id)
        {
            return nodes.Find(n => n.NodeId == id);
        }

        public void Init()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var data = nodes[i];
                var type = Type.GetType(data.TypeQualifiedName);
                if (type == null)
                    continue;
                Node instance = NodeSerialization.CreateInstance(Type.GetType(data.TypeQualifiedName)) as Node;
                instance.graph = this;
                instance.SyncData(data);
                instance.Init();
                nodes[i] = instance;
            }

            OnInit();
        }

        protected virtual void OnInit()
        {

        }
        // public void SetNode(Node newNode)
        // {
        //     var node = GetNode(newNode.NodeId);
        //     if (node == null)
        //         return;
        //     var index = nodes.IndexOf(node);
        //     if (index <= 0)
        //         return;
        //     nodes[index] = newNode;
        // }

        /// <summary> Add a node to the graph by type (convenience method - will call the System.Type version) </summary>
        public T AddNode<T>() where T : Node
        {
            return AddNode(typeof(T)) as T;
        }

        /// <summary> Add a node to the graph by type </summary>
        public virtual Node AddNode(Type type)
        {
            // Node.graphHotfix = this;
            Node node = NodeSerialization.CreateInstance(type) as Node;
            int count = nodes.Count;
            node.NodeId = nodes.Count > 0 ? nodes[count - 1].NodeId + 1 : 0;
            node.graph = this;
            node.TypeQualifiedName = type.AssemblyQualifiedName;
            nodes.Add(node);
            return node;
        }

        /// <summary> Creates a copy of the original node in the graph </summary>
        public virtual Node CopyNode(Node original)
        {
            // Node.graphHotfix = this;
            Node node = NodeSerialization.Instantiate(original);
            int count = nodes.Count;
            node.NodeId = nodes.Count > 0 ? nodes[count - 1].NodeId + 1 : 0;
            node.graph = this;
            node.TypeQualifiedName = original.TypeQualifiedName;
            node.ClearConnections();
            nodes.Add(node);
            return node;
        }

        /// <summary> Safely remove a node and all its connections </summary>
        /// <param name="node"> The node to remove </param>
        public virtual void RemoveNode(Node node)
        {
            node.ClearConnections();
            nodes.Remove(node);
        }

        /// <summary> Remove all nodes and connections from the graph </summary>
        public virtual void Clear()
        {
            nodes.Clear();
        }

        /// <summary> Create a new deep copy of this graph </summary>
        public virtual NodeGraph Copy()
        {
            // Instantiate a new nodegraph instance
            NodeGraph graph = NodeSerialization.Instantiate(this);
            // Instantiate all nodes inside the graph
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == null)
                    continue;
                // Node.graphHotfix = graph;
                Node node = NodeSerialization.Instantiate(nodes[i]) as Node;
                node.graph = graph;
                graph.nodes[i] = node;
            }

            // Redirect all connections
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                if (graph.nodes[i] == null) continue;
                foreach (NodePort port in graph.nodes[i].Ports)
                {
                    port.Redirect(nodes, graph.nodes);
                }
            }

            return graph;
        }

        public virtual void OnDestroy()
        {
            // Remove all nodes prior to graph destruction
            Clear();
        }

        #region Attributes

        /// <summary> Automatically ensures the existance of a certain node type, and prevents it from being deleted. </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class RequireNodeAttribute : Attribute
        {
            public Type type0;
            public Type type1;
            public Type type2;

            /// <summary> Automatically ensures the existance of a certain node type, and prevents it from being deleted </summary>
            public RequireNodeAttribute(Type type)
            {
                this.type0 = type;
                this.type1 = null;
                this.type2 = null;
            }

            /// <summary> Automatically ensures the existance of a certain node type, and prevents it from being deleted </summary>
            public RequireNodeAttribute(Type type, Type type2)
            {
                this.type0 = type;
                this.type1 = type2;
                this.type2 = null;
            }

            /// <summary> Automatically ensures the existance of a certain node type, and prevents it from being deleted </summary>
            public RequireNodeAttribute(Type type, Type type2, Type type3)
            {
                this.type0 = type;
                this.type1 = type2;
                this.type2 = type3;
            }

            public bool Requires(Type type)
            {
                if (type == null) return false;
                if (type == type0) return true;
                else if (type == type1) return true;
                else if (type == type2) return true;
                return false;
            }
        }

        #endregion
    }
}