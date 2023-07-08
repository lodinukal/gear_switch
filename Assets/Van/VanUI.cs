using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanUI : MonoBehaviour
{
    [SerializeField]
    private VanManager _vanManager;
    private Van _van;

    [SerializeField]
    private TMPro.TextMeshProUGUI _healthText;
    [SerializeField]
    private UnityEngine.UI.Image _healthBar;

    [SerializeField]
    private TMPro.TextMeshProUGUI _viewText;
    [SerializeField]
    private GameObject _infoPanel;
    [SerializeField]
    private UnityEngine.UI.Image _viewBar;

    private bool _changeInited = false;
    [SerializeField]
    private float _changeTime = 1f;
    private float _changeTimer = 0f;


    [SerializeField]
    private Ephemeral _eph;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_eph.Enabled)
        {
            return;
        }

        if (_vanManager.Van == null)
            return;
        _van = _vanManager.Van;
        _healthText.text = $"Health: {_vanManager.State.Health}/{_vanManager.State.MaxHealth}";
        _healthBar.fillAmount = _vanManager.State.Health / _vanManager.State.MaxHealth;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _changeInited = true;
        }
        if (_changeInited && Input.GetKey(KeyCode.Space)) {
            _changeTimer += Time.deltaTime;
        } else if (_changeInited && Input.GetKeyUp(KeyCode.Space)) {
            _changeInited = false;
            _changeTimer = 0f;
        }

        if (_changeInited && _changeTimer >= _changeTime)
        {
            _changeInited = false;
            _changeTimer = 0f;
            _vanManager.CycleView();
        }

        _infoPanel.SetActive(!_changeInited);
        _viewBar.gameObject.transform.parent.gameObject.SetActive(_changeInited);
        _viewBar.fillAmount = _changeTimer / _changeTime;
        _viewText.text = $"ROLE: {_vanManager.ViewName}";
    }
}
