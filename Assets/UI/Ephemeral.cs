using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ephemeral : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GraphicRaycaster _raycaster;

    [SerializeField]
    private List<AudioSource> _audioSources;

    public bool Enabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Enabled = !Enabled;
            _canvas.enabled = Enabled;
            _raycaster.enabled = Enabled;
        }
        Time.timeScale = Enabled ? 0f : 1f;

        foreach (var audioSource in _audioSources)
        {
            audioSource.mute = Enabled;
        }
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void Resume()
    {
        Enabled = false;
        _canvas.enabled = false;
        _raycaster.enabled = false;
    }
}
