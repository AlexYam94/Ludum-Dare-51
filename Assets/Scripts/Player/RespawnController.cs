using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    [SerializeField] float _waitToRespawn;
    [SerializeField] CinemachineVirtualCamera _followCamera;
    [SerializeField] GameObject _deathEffect;

    private Vector3 _respawnPoint;
    private GameObject _player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //    DontDestroyOnLoad(gameObject);
        }
        else
            {
                Destroy(gameObject);
            }
        }
    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerHealthController.GetInstance().transform.gameObject;
        _respawnPoint = _player.transform.position;
        _player.SetActive(true);
        PlayerHealthController.GetInstance().FillHealth();
        //_followCamera.LookAt = _player.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawn(Vector3 newPosition)
    {
        _respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnPlayer());    
    }

    private IEnumerator RespawnPlayer()
    {
        if(_deathEffect != null)
        {
            Instantiate(_deathEffect, _player.transform.position, _player.transform.rotation);
        }
        _player.SetActive(false);
        UIController.GetInstance().gameObject.SetActive(false);
        yield return new WaitForSeconds(_waitToRespawn);
        //var asyncLoadLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        var asyncLoadLevel = SceneManager.LoadSceneAsync("GameOver");

        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        //_player.transform.position = _respawnPoint;
        //_player.SetActive(true);
        //PlayerHealthController.GetInstance().FillHealth();
        //_followCamera.LookAt = _player.gameObject.transform;
    }
}
