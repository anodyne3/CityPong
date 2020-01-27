using UnityEngine;

public class CylinderShape : MonoBehaviour
{
    private Vector3 _localScale;
    private float _scaleY ;
    public float StartingSwell { get; private set; }

    private void Start()
    {
        _localScale = transform.localScale;
        StartingSwell = _localScale.y;
        _scaleY = StartingSwell;
    }

    private void Update()
    {
        var scale = transform.localScale.x;
        _localScale.Set(scale, _scaleY, scale);
        transform.localScale = _localScale;
    }

    public void SwellPaddle(float swellAmount)
    {
        _scaleY = swellAmount;
        //not implemented
    }
}