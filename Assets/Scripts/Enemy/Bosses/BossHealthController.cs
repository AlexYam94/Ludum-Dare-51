using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{

    public static BossHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Slider _healthSlider;
    [SerializeField] int _currentHealth;
    [SerializeField] GhostBossBattle _boss;

    // Start is called before the first frame update
    void Start()
    {
        _healthSlider.maxValue = _currentHealth;
        _healthSlider.value = _currentHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        
        _healthSlider.value = _currentHealth;

        if(_currentHealth <= 0)
        {
            //_boss.EndBattle();
        }
        
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}
