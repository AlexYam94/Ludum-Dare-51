using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] float _distanceToOpen;
    [SerializeField] Animator _anim;
    [SerializeField] Transform _doorWayExitPoint;
    [SerializeField] float _movePlayerSpeed;
    [SerializeField] string _levelToLoad;
    [SerializeField] DoorController _theOtherDoor;
    [SerializeField] float _timeWaitedForEnterDoor = 1.8f;

    PlayerController _player;
    bool _playerExiting;
    bool _canEnter;

    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerHealthController.GetInstance().GetComponent<PlayerController>();
        _canEnter = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < _distanceToOpen)
        {
            _anim.SetBool("doorOpen", true);
        }
        else
        {
            _anim.SetBool("doorOpen", false);
        }
        if (_playerExiting && Vector3.Distance(_player.transform.position, _doorWayExitPoint.transform.position)>.5)
        {
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _doorWayExitPoint.transform.position, _movePlayerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (_player.CanPlayerInput() && !_playerExiting && _canEnter)
            {
                _player.DisableInput();
                StartCoroutine(UseDoorCorotine());
            }
        }
    }

    public void DisableDoor()
    {
        _canEnter = false;
    }

    public void EnableDoor()
    {
        _canEnter = true;
    }

    IEnumerator UseDoorCorotine()
    {
        _playerExiting = true;
        _player.FreezeSprite();

        UIController.GetInstance().StartFadeToBlack();
        _theOtherDoor?.DisableDoor();

        yield return new WaitForSeconds(_timeWaitedForEnterDoor);

        _theOtherDoor?.EnableDoor();
        ScoreController.GetInstance().FinishLevel();
        UIController.GetInstance().StartFadeFromBlack();
        _player.UnfreezeSprite();
        _playerExiting = false;
        _player.EnableInput();
        Vector3 spawnPos = GameObject.Find("SpawnPoint").transform.position;
        _player.transform.position = spawnPos;
        RespawnController.instance.SetSpawn(spawnPos);
        if (_levelToLoad.Length > 0)
        {
            SceneManager.LoadScene(_levelToLoad);
        }
    }
}
