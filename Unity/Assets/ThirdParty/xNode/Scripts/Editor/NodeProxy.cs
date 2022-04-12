using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;
using XNodeEditor.Internal;

namespace XNodeEditor
{
    public class NodeProxy : NodeScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public NodeGraphProxy graphProxy;

        [NonSerialized, ShowInInspector]
        private Node _node;
        public Node node
        {
            get => _node ?? (_node = graphProxy.graph.GetNode(this.Id));
            set => _node = value;
        }

        public override Type GetDataType() => node.GetType();

        public Type GetNodeType() => node.GetType();

        [SerializeField, HideInInspector]
        private int _Id;
        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
            }
        }

        public NodeGraph graph => this.node.graph;
        public IEnumerable<NodePort> Ports => this.node.Ports;
        public IEnumerable<NodePort> Outputs => this.node.Outputs;
        public IEnumerable<NodePort> Inputs => this.node.Inputs;
        public IEnumerable<NodePort> DynamicPorts => this.node.DynamicPorts;

        public IEnumerable<NodePort> DynamicOutputs => this.node.DynamicOutputs;

        public IEnumerable<NodePort> DynamicInputs => this.node.DynamicInputs;

        public void UpdatePorts() => this.node.UpdatePorts();

        #region Lifecycle



        #endregion

        #region Ports

        public NodePort GetOutputPort(string fieldName) => this.node.GetOutputPort(fieldName);
        public NodePort GetInputPort(string fieldName) => this.node.GetInputPort(fieldName);
        public NodePort GetPort(string fieldName) => this.node.GetPort(fieldName);
        public bool HasPort(string fieldName) => this.node.HasPort(fieldName);

        #endregion

        #region Dynamic Ports

        public NodePort AddDynamicInput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple,
            Node.TypeConstraint typeConstraint = XNode.Node.TypeConstraint.None, string fieldName = null) =>
            this.node.AddDynamicInput(type, connectionType, typeConstraint, fieldName);

        public NodePort AddDynamicOutput(Type type, Node.ConnectionType connectionType = Node.ConnectionType.Multiple,
            Node.TypeConstraint typeConstraint = XNode.Node.TypeConstraint.None, string fieldName = null) =>
            this.node.AddDynamicOutput(type, connectionType, typeConstraint, fieldName);

        public void RemoveDynamicPort(string fieldName) => this.node.RemoveDynamicPort(fieldName);

        public void RemoveDynamicPort(NodePort port) => this.node.RemoveDynamicPort(port);

        public void ClearDynamicPorts() => this.node.ClearDynamicPorts();

        #endregion

        #region Inputs/Outputs

        public T GetInputValue<T>(string fieldName, T fallback = default(T)) =>
            this.node.GetInputValue<T>(fieldName, fallback);

        public T[] GetInputValues<T>(string fieldName, params T[] fallback) =>
            this.node.GetInputValues<T>(fieldName, fallback);

        public virtual object GetValue(NodePort port) => this.node.GetValue(port);

        #endregion
    }
}