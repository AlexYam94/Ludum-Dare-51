using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject _player;
    public float PixelsPerUnit;
    public float yOffset = 10f;
    public float maxYDifference = 10f;

    void Start()
    {
    }

    void Update()
    {
        Vector2 newPos = new Vector2(_player.transform.position.x, _player.transform.position.y + yOffset);
        var pos = Vector2.MoveTowards(transform.position, newPos, PixelsPerUnit);
        if (Mathf.Abs(_player.transform.position.y - transform.position.y) > maxYDifference)
        {
            pos.y = _player.transform.position.y + yOffset;
        }
        transform.position = pos;
    }

}
