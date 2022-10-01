using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bullet;

public class FirePattern : MonoBehaviour
{
    public FirePatternDirection[] directions;
    public bool isIntermittent;

    private int _nextDirection = 0;

    public enum FirePatternDirection
    {
        front,
        back,
        Up,
        Down
    }

    public FirePatternDirection GetNextDirection()
    {
        if (_nextDirection >= directions.Length) _nextDirection = 0;
        return directions[_nextDirection++];
    }
}
