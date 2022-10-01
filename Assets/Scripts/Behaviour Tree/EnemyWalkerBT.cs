using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkerBT : MonoBehaviour
{
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _waitAtPoint;
    [SerializeField] float _jumpForce;

    private Node _root;
    // Start is called before the first frame update
    void Start()
    {
        _root = SetupTree();
        foreach (Transform pPoint in _patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _root.Evaluate();
    }

    private Node SetupTree()
    {
        List<Node> children = new List<Node>();
        children.Add(new PatrolTask(transform, _patrolPoints, _moveSpeed, _waitAtPoint, _jumpForce));
        Sequence root = new Sequence();
        root.SetChildren(children);
        root.SetContext(null);
        return root;
    }
}
