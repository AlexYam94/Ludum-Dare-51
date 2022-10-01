using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] float _timeToExplode = 1f;
    [SerializeField] GameObject _explosion;
    [SerializeField] float _explosionYOffset = .5f;
    [SerializeField] float _blastRange = 1f;
    [SerializeField] LayerMask _destructibleLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeToExplode -= Time.deltaTime;
        if(_timeToExplode <= 0)
        {
            if(_explosion != null)
            {
                Vector3 explosionPos = transform.position;
                explosionPos.y += _explosionYOffset;
                Instantiate(_explosion, explosionPos, transform.rotation);
                Destroy(gameObject);
                Collider2D[] destructibles = Physics2D.OverlapCircleAll(transform.position, _blastRange, _destructibleLayer);
                foreach(Collider2D collider in destructibles)
                {
                    Destroy(collider.gameObject);
                }
            }
        }
    }
}
