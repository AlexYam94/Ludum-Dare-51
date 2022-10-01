using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Node 
    {
        protected Node parent;
        //private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
        private Dictionary<string, object> _dataContext;
        protected List<Node> _childNodes = new List<Node>();

        public Node() { }

        public Node(List<Node> nodes, Dictionary<string, object> context)
        {
            _dataContext = context;
            foreach (var node in nodes)
            {
                Attach(node, context);
            }
        }

        private void Attach(Node node, Dictionary<string, object> context)
        {
            node.parent = this;
            node.SetContext(context);
            _childNodes.Add(node);
        }

        public void SetContext(Dictionary<string, object> context)
        {
            _dataContext = context;
            foreach (var node in _childNodes)
            {
                node.SetContext(context);
            }
        }

        protected void SetData(string key, object data)
        {
            if (_dataContext == null) return;
            _dataContext[key] = data;
        }

        public T GetData<T>(string key)
        {
            return (T)_dataContext?[key];
        }

        public void SetChildren(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                Attach(node, null);
            }
        }

        public abstract NodeState Evaluate();
    }
}