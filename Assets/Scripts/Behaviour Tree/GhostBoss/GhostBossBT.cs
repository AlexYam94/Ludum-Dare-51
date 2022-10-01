using BehaviourTree;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBossBT : MonoBehaviour
{
    public readonly static string ActiveCounterTag = "ActiveCounter";
    public readonly static string InactiveCounterTag = "InactiveCounter";
    public readonly static string FadeCounterTag = "FadeCounter";
    public readonly static string ShotCounterTag = "ShotCounter";
    public readonly static string BattleEndTag = "BattleEnd";

    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] Transform _camPosition;
    [SerializeField] float _camSpeed;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _phase3moveSpeedMultiplier;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] float _activeTime;
    [SerializeField] float _fadeoutTime;
    [SerializeField] float _inactiveTime;
    [SerializeField] int _phase1Threshold;
    [SerializeField] int _phase2Threshold;
    [SerializeField] float _phase2FadeoutTimeMultiplier = 1;
    [SerializeField] Animator _anim;
    [SerializeField] Transform _boss;
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _bullet;
    [SerializeField] Transform _shotPoint;
    [SerializeField] float _fireRate;
    [SerializeField] float _phase2FireRateMultiplier;
    [SerializeField] float _phase3FireRateMultiplier;
    [SerializeField] GameObject _winObj;

    private bool _battelEnded;
    private Transform _targetPoint;
    private GameObject _player;
    private Node _root;
    private Dictionary<string, object> _context;

    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _context = new Dictionary<string, object>();
        _player = PlayerHealthController.GetInstance().gameObject;
        _virtualCamera.GetComponent<LookAt>().enabled = false;
        _virtualCamera.Follow = null;
        _root = SetupTree();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player.GetComponent<PlayerController>().IsVisible()) return;
        _root.Evaluate();

    }
    private Node SetupTree()
    {
        List<Node> children = new List<Node>();
        children.Add(new BattleEndTask(_virtualCamera, _camSpeed, _camPosition, _boss, gameObject,_player.transform));
        children.Add(new Phase3ActivateTask(_boss, _shotPoint, _bullet, _fadeoutTime, _fireRate, _phase3FireRateMultiplier, _collider, _moveSpeed * _phase3moveSpeedMultiplier, _phase2Threshold, _spawnPoints));
        children.Add(new FadeOutTask(_inactiveTime, _boss));
        children.Add(new VanishedTask(_boss, _spawnPoints, _activeTime, _fireRate));
        children.Add(new Phase1ActivateTask(_boss, _shotPoint, _bullet, _fadeoutTime, _inactiveTime, _fireRate, 1, _phase1Threshold));
        children.Add(new Phase2ActivateTask(_boss, _shotPoint, _bullet, _fadeoutTime, _fireRate, _phase2FireRateMultiplier, _phase2Threshold, _phase2FadeoutTimeMultiplier, _collider, _moveSpeed, _spawnPoints));
        Selector root = new Selector();
        root.SetChildren(children);
        root.SetContext(_context);

        _context.Add(ActiveCounterTag, _activeTime);
        _context.Add(ShotCounterTag, _fireRate);
        _context.Add(FadeCounterTag, _fadeoutTime);
        _context.Add(BattleEndTag, false);

        return root;
    }
}
