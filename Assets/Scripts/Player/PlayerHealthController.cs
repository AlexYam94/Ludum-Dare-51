using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private static PlayerHealthController _instance;

    [SerializeField] int _maxHealth;
    [SerializeField] float _invincibilityLength;
    [SerializeField] float _flashLength;
    [SerializeField] SpriteRenderer[] _playerSprites;

    bool _invincibleFlash;
    float _invincCounter;
    float _flashCounter;
    int _currentHealth;
    bool _isInvincible;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        UpdateHealthUI();
        UIController.GetInstance().SetHealthBarLength(_maxHealth);
        _invincibleFlash = false;
        _isInvincible = false;
    }

    private void Update()
    {
        UpdateHealthUI();
        if (_invincibleFlash)
        {
            if (_invincCounter > 0)
            {
                _invincCounter -= Time.deltaTime;

                _flashCounter -= Time.deltaTime;
                if (_flashCounter <= 0)
                {
                    foreach (SpriteRenderer sr in _playerSprites)
                    {
                        sr.enabled = !sr.enabled;
                    }
                    _flashCounter = _flashLength;
                }
            }
            else
            {
                foreach (SpriteRenderer sr in _playerSprites)
                {
                    sr.enabled = true;
                }
                _invincibleFlash = false;
            }
        }
    }

    public void Damage(int damageAmount)
    {
        if (_invincCounter > 0 || _isInvincible) return;
        _currentHealth -= damageAmount;
        UpdateHealthUI();
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            //gameObject.SetActive(false);
            RespawnController.instance.Respawn();
        }
        else
        {
            _invincibleFlash = true;
            _invincCounter = _invincibilityLength;
        }
    }

    public void GetExtraHealth(int extraHealth)
    {
        _maxHealth += extraHealth;
        FillHealth();
        UIController.GetInstance().SetHealthBarLength(_maxHealth);
    }

    public void FillHealth()
    {
        _currentHealth = _maxHealth;
        UpdateHealthUI();
        UIController.GetInstance().SetHealthBarLength(_maxHealth);
    }

    public void SetHealthScale(float scale)
    {
        _maxHealth = Mathf.RoundToInt(_maxHealth * scale);
        _currentHealth = Mathf.RoundToInt(_currentHealth * scale);
        UpdateHealthUI();
    }

    public static PlayerHealthController GetInstance()
    {
        return _instance;
    }

    private void UpdateHealthUI()
    {
        UIController.GetInstance().UpdateHealthBar(_currentHealth, _maxHealth);
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }
    public void SetCurrentHealth(int currentHealth)
    {
        _currentHealth = currentHealth;
    }

    public void ToggleInvincible()
    {
        _isInvincible = !_isInvincible;
    }
}
