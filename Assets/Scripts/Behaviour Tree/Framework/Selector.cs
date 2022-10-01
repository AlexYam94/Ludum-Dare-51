using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        public override NodeState Evaluate()
        {
            foreach(Node node in _childNodes)
            {

                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        return NodeState.SUCCESS;
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                    default:
                        continue;
                }
            }

            return NodeState.FAILURE;
        }
    }
}
