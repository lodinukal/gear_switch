using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class VanState
{
    public VanState(float health, float maxHealth, float fuel)
    {
        this.Health = health;
        this.MaxHealth = maxHealth;
        this.Fuel = fuel;
    }

    public float Health;
    public float MaxHealth;
    public float Fuel;
}

[System.Serializable]
public class ViewChangeEvent : UnityEvent<System.Type> { }

public abstract class View : MonoBehaviour
{
    [System.NonSerialized]
    public System.Type Type;
    [System.NonSerialized]
    public bool InView = false;
    [System.NonSerialized]
    public GameObject Van;
    [System.NonSerialized]
    public VanState State;

    [System.NonSerialized]
    public ViewChangeEvent ChangeView;
    public void SetView<T>()
    {
        ChangeView.Invoke(typeof(T));
        InView = false;
    }

    public abstract string GetName();

    public abstract void Enter();
    public abstract void UpdateView(float delta);
    public abstract void Exit();
}

public class VanManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _vanPrefab;
    private GameObject _createdVan;
    private Van _van;
    private VanState _state;

    [SerializeField]
    private List<View> _views;

    [SerializeField]
    private AudioSource _damagedSound;

    public ViewChangeEvent ChangeView;
    private View _currentView;
    private System.Type _setType;
    private int _viewIndex = 0;

    private Collider2D _cachedCollider;

    public Van Van { get { return _van; } }
    public VanState State { get { return _state; } }
    public string ViewName { get { return _currentView.GetName(); } }
    public Collider2D VanCollider
    {
        get
        {
            if (_cachedCollider)
            {
                return _cachedCollider;
            }
            _cachedCollider = _van.GetComponent<Collider2D>();
            return _cachedCollider;
        }
    }

    [SerializeField]
    private Shake _shaker;

    [SerializeField]
    private Ephemeral _eph;

    public UnityEvent OnGameOver;

    void Awake()
    {
        OnGameOver = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        _createdVan = Instantiate(_vanPrefab, transform);
        _createdVan.GetComponent<Van>().Manager = this;
        _van = _createdVan.GetComponent<Van>();
        _state = new VanState(30f, 30f, 10f);
        ChangeView = new ViewChangeEvent();
        foreach (var view in _views)
        {
            view.InView = false;
            view.Type = view.GetType();
            view.Van = _createdVan;
            view.State = _state;
            view.ChangeView = ChangeView;
        }

        ChangeView.AddListener(OnViewChange);

        _setType = _views[0].Type;
        ProcessViewChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (_eph.Enabled)
        {
            return;
        }

        float delta = Time.deltaTime;
        foreach (var view in _views)
        {
            view.UpdateView(delta);
        }

        ProcessViewChange();
    }

    public void CycleView()
    {
        _viewIndex = (_viewIndex + 1) % _views.Count;
        _setType = _views[_viewIndex].Type;
    }

    void OnViewChange(System.Type type)
    {
        _setType = type;
    }

    void ProcessViewChange()
    {
        if (_setType == null)
        {
            return;
        }

        if (_currentView != null)
        {
            _currentView.InView = false;
            _currentView.Exit();
        }

        _currentView = _views.Find(view => view.Type == _setType);
        if (_currentView)
        {
            _currentView.Enter();
            _currentView.InView = true;
        }

        _setType = null;
    }

    public void Damage(float amount)
    {
        _state.Health -= amount;
        _damagedSound.PlayOneShot(_damagedSound.clip);
        _shaker.TriggerShake();
        if (_state.Health <= 0)
        {
            OnGameOver.Invoke();
        }
    }
}
