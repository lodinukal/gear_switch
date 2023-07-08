using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RoadManager Road;
    public VanManager Van;

    public float Speed = 1f;
    public float Health = 3f;
    public float MaxHealth = 3f;

    [SerializeField]
    private GameObject _healthBar;
    [SerializeField]
    private UnityEngine.UI.Image _healthBarFill;
    [SerializeField]
    private TMPro.TextMeshProUGUI _healthText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var goal = Van.Van.transform.position;
        var direction = (goal - transform.position).normalized;
        transform.position += direction * Speed * Time.deltaTime;

        var vecced = Road.Clamp(new Vector2(transform.position.x, transform.position.y), 5);
        transform.position = new Vector3(vecced.x, vecced.y, transform.position.z);

        if (Health == MaxHealth)
        {
            _healthBar.SetActive(false);
        }
        else
        {
            _healthBar.SetActive(true);
            _healthBarFill.fillAmount = Health / MaxHealth;
            _healthText.text = $"{Health}/{MaxHealth}";
        }
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
