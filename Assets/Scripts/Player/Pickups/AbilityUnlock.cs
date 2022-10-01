using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField] AbilityEnum _abilityToUnlock;
    [SerializeField] GameObject _pickupEffect;
    [SerializeField] string _unlockMessage;
    [SerializeField] TextMeshProUGUI _unlockText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponentInParent<AbilitiesController>().UnlockAbility(_abilityToUnlock);
            Instantiate(_pickupEffect, transform.position, transform.rotation);
            _unlockText.text = _unlockMessage;
            _unlockText.transform.parent.SetParent(null);
            _unlockText.transform.parent.gameObject.SetActive(true);
            _unlockText.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
