using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPiece : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color colour)
    {
        _spriteRenderer.color = colour;
    }

    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
