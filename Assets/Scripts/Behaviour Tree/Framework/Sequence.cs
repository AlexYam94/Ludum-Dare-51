using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Sequence : Node
    {
        private int _index = 0;

        public override NodeState Evaluate()
        {
            bool anyChildeIsRunning = false;

            while (_index < _childNodes.Count)
            {
                switch (_childNodes[_index].Evaluate())
                {
                    case NodeState.FAILURE:
                        return NodeState.FAILURE;
                    case NodeState.SUCCESS:
                        _index++;
                        continue;
                    case NodeState.RUNNING:
                        anyChildeIsRunning = true;
                        break;
                    default:
                        return NodeState.SUCCESS;
                }
                if (anyChildeIsRunning) break;
            }

            if (anyChildeIsRunning)
                return NodeState.RUNNING;
            else
            {
                _index = 0;
                return NodeState.SUCCESS;
            }
        }
    }
}
