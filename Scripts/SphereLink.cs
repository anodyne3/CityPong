using UnityEngine;

public class SphereLink : MonoBehaviour
{
    [SerializeField] private Transform cylinder;
    [SerializeField] private Vector3 offset;
    private Transform _sphereTransform;
    private float _offsetSign;
    private void Start()
    {
        _sphereTransform = transform;
        _offsetSign = offset.z;
    }
    //move each sphere to the ends of the cylinder, creating the paddle
    private void Update()
    {
        offset.z = cylinder.localScale.y * _offsetSign;
        _sphereTransform.position = cylinder.position + offset;
    }
}