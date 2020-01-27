using UnityEngine;

public class InputController : MonoBehaviour
{
    private GameManager _gameManager;
    
    private float _paddleSpeed = 1.0f;

    private const float InputTolerance = 0.01f;
    private float _paddleSlide;
    
    private const float PaddleTranslateMin = -4.25f;
    private const float PaddleTranslateMax = 4.25f;

    private void Start()
    {
        _gameManager = (GameManager) GetComponent(typeof(GameManager));
    }

    private void Update()
    {
        if (_gameManager.menuOpen) return;
        _paddleSlide = Input.GetAxis("Mouse Y") * _paddleSpeed;
        if (Input.GetMouseButtonDown(0))
        {
            SwellPaddle(_gameManager.playerRight.cylinder);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ResetPaddleSwell(_gameManager.playerRight.cylinder);
        }

        if (Mathf.Abs(Input.GetAxis("Mouse Y")) < InputTolerance) return;
        
        TranslatePaddle(_gameManager.playerRight.paddle);
    }

    private void TranslatePaddle(GameObject paddle)
    {
        paddle.transform.Translate(0.0f,0.0f,_paddleSlide);
        var positionClamped = paddle.transform.position;
        positionClamped.z = Mathf.Clamp(positionClamped.z, PaddleTranslateMin, PaddleTranslateMax);
        paddle.transform.position = positionClamped;
    }
    //decrease the paddle length to increase the bounce velocity
    private void SwellPaddle(GameObject cylinder)
    {
        //not implemented
    }
    //reset paddle to starting length
    private void ResetPaddleSwell(GameObject cylinder)
    {
        //not implemented
    }
}