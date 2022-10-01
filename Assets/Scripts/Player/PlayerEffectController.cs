using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _afterImage;
    [SerializeField] float _afterImageLifeTime;
    [SerializeField] float _timeBetweenAfterImage;
    [SerializeField] Color _afterImageColor;

    private float _afterImageCounter;
    
    public void ShowAfterImage(SpriteRenderer playerSpriteRender)
    {
        SpriteRenderer image = Instantiate(_afterImage, transform.position, transform.rotation);
        image.sprite = playerSpriteRender.sprite;
        image.transform.localScale = transform.localScale;
        image.color = _afterImageColor;

        Destroy(image.gameObject, _afterImageLifeTime);
        _afterImageCounter = _timeBetweenAfterImage;
    }

    public void CountDown(float time)
    {
        _afterImageCounter -= time;
    }

    public float GetAfterImageCounter()
    {
        return _afterImageCounter;
    }
}
