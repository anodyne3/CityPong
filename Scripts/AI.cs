using UnityEngine;

public class AI : MonoBehaviour
{
    private GameManager _gameManager;
    private const float PaddleTranslateMin = -4.25f;
    private const float PaddleTranslateMax = 4.25f;
    private float _paddleSlide;
    private float _skill = 0.55f;
    [SerializeField] private GameObject _aiPaddle;

    private void Start()
    {
        _gameManager = (GameManager) GetComponent(typeof(GameManager));
        _aiPaddle = _gameManager.playerLeft.paddle;
    }
    //if there's a ball in play, and the menu is not open, move the AI paddle 
    private void Update()
    {
        if (_gameManager.ballCount <= 0) return;
        if (_gameManager.menuOpen) return;
        MoveToIntercept(_gameManager.ball);
    }
    //if the ball is moving towards the AI paddle, translate the paddle to intercept, with a degree of error 
    private void MoveToIntercept(GameObject ball)
    {
        var ballRigidBody = (Rigidbody) ball.GetComponent(typeof(Rigidbody));
        if (ballRigidBody.velocity.x > 0) return;
        var randomSign = _gameManager.spawnRight ? 1.0f : -1.0f;
        _paddleSlide = ball.transform.position.z - _aiPaddle.transform.position.z / 2 * _skill * randomSign;
        TranslatePaddle(_aiPaddle);
    }
    //translate the paddle within the clamped range
    private void TranslatePaddle(GameObject paddle)
    {
        paddle.transform.Translate(0.0f, 0.0f, _paddleSlide);
        var positionClamped = paddle.transform.position;
        positionClamped.z = Mathf.Clamp(positionClamped.z, PaddleTranslateMin, PaddleTranslateMax);
        paddle.transform.position = positionClamped;
    }
}