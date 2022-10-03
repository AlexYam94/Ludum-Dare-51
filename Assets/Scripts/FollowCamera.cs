using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerHealthController.GetInstance()?.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerPos = _player.position;

        transform.position = new Vector2((mousePos.y + playerPos.y) / 2, (mousePos.y + playerPos.y) / 2);
    }
}
