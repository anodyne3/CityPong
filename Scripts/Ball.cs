using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager _gameManager;
    private Rigidbody _ballRigidBody;
    [SerializeField] private float _velocityTolerance;
    
    private void Start()
    {
        _ballRigidBody = (Rigidbody) GetComponent(typeof(Rigidbody));
        _gameManager = (GameManager) transform.parent.GetComponent(typeof(GameManager));
    }
    
    //ball passes through trigger in goal area
    private void OnTriggerExit(Collider other)
    {
        switch (other.name)
        {
            case "GoalLeft":
                _gameManager.ShowInfo("GOAL");
                _gameManager.playerRight.playerScore += 1;
                _gameManager.spawnRight = true;
                BallReset();
                break;
            case "GoalRight":
                _gameManager.ShowInfo("GOAL");
                _gameManager.playerLeft.playerScore += 1;
                _gameManager.spawnRight = false;
                BallReset();
                break;
            default:
                break;
        }

        _gameManager.UpdateScores();
    }

    //colliding with bumpers and paddles increases ball velocity
    private void OnCollisionEnter(Collision other)
    {
        _ballRigidBody.AddForce(other.impulse, ForceMode.Force);
    }

    private void Update()
    {
        //ball passes through(over) bumpers, show info, reset ball
        if ((_gameManager.ball.transform.position - _gameManager.ballSpawn).sqrMagnitude > 10000.0f) DeclareFoul();
        //ball slows down below tolerance level, addForce
        if (_ballRigidBody.velocity.sqrMagnitude > _velocityTolerance) return;
        PushBall();
        //ball has stopped moving, reset ball 
        if (_ballRigidBody.velocity.sqrMagnitude == 0 && !_gameManager.menuOpen) BallReset();
    }

    private void PushBall()
    {
        var push = _ballRigidBody.velocity;
        _ballRigidBody.AddForce(push + push, ForceMode.Impulse);
    }
   
    private void DeclareFoul()
    {
        _gameManager.ShowInfo("FOUL");
        BallReset();
    }

    private void BallReset()
    {
        gameObject.SetActive(false);
        _gameManager.ballCount -= 1;
    }

}