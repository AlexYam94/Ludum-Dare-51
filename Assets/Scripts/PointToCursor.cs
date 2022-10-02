using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToCursor : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] SpriteRenderer _playerSprite;

    // Update is called once per frame
    void Update()
    {
        ////rotation
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 5.23f;

        //Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - objectPos.x;
        //mousePos.y = mousePos.y - objectPos.y;

        //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        Vector2 difference = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;

        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        var scale = _playerTransform.localScale;
        if (rotationZ < -90 || rotationZ > 90)
        {
            //_playerSprite.flipX = true;
            scale.x = -1;
            transform.localRotation = Quaternion.Euler(180, 0, rotationZ);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
            scale.x = 1;
            //_playerSprite.flipX = false;
        }
        _playerTransform.localScale = scale;
        transform.localScale = scale;
    }
}
