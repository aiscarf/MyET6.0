using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;
using XNodeEditor.Internal;

namespace XNodeEditor
{
    public class NodeGraphProxy : NodeScriptableObject
    {
        [SerializeField] public TextAsset textAsset;

        [SerializeField, HideInInspector] public NodeGraph graph;

        [SerializeField, HideInInspector]
        private List<NodeProxy> nodeProxies = new List<NodeProxy>();

        [ShowInInspector]
        public int Id
        {
            get => graph.Id;
            set => graph.Id = value;
        }

        [ShowInInspector] public NodeGraph Graph => graph;

        [ShowInInspector]
        public List<NodeProxy> NodeProxies => nodeProxies;

        protected virtual void OnEnable()
        {
            if (graph == null)
                return;
            graph.Init();
            graph.TypeQualifiedName = graph.GetType().AssemblyQualifiedName;

            for (int i = 0; i < nodeProxies.Count; i++)
            {
                nodeProxies[i].graphProxy = this;
                nodeProxies[i].node = graph.nodes[i];
                nodeProxies[i].Id = graph.nodes[i].NodeId;
            }

            Debug.Log($"{this.name} 图初始化完成!");
        }

        void OnDestroy()
        {
            this.graph.OnDestroy();
        }

        public NodeProxy GetNodeProxy(Node node) => GetNodeProxy(node.NodeId);

        public NodeProxy GetNodeProxy(int id)
        {
            return nodeProxies.Find(proxy => proxy.Id == id);
        }

        public override Type GetDataType() => graph?.GetType();

        public Type GetGraphType() => graph?.GetType();

        // public List<Node> nodes => this.graph.nodes;

        // public T AddNode<T>() where T : Node => this.graph.AddNode<T>();

        public NodeProxy AddNode(Type type)
        {
            var nodeProxy = ScriptableObject.CreateInstance<NodeProxy>();
            nodeProxy.graphProxy = this;
            var node = this.graph.AddNode(type);
            nodeProxy.node = node;
            nodeProxy.Id = node.NodeId;
            node.Init();
            nodeProxies.Add(nodeProxy);
            return nodeProxy;
        }

        public void RemoveNode(NodeProxy nodeProxy)
        {
            this.graph.RemoveNode(nodeProxy.node);
            this.nodeProxies.Remove(nodeProxy);
        }

        public NodeProxy CopyNode(NodeProxy original)
        {
            var nodeProxy = ScriptableObject.Instantiate(original);
            nodeProxy.graphProxy = this;
            var node = this.graph.CopyNode(original.node);
            nodeProxy.node = node;
            nodeProxy.Id = node.NodeId;
            node.Init();
            return nodeProxy;
        }
    }
}