using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //ui
    private Canvas _mainCanvas;
    private UIButtons _uiButtons;
    private TMP_Text _scoreLeft;
    private TMP_Text _scoreRight;
    private GameObject _info;
    public bool menuOpen;
  
    private float _highScore;
    public bool gameOver;
    //players
    public Player playerLeft = new Player();
    public Player playerRight = new Player();
    //ball
    private GameObject _ballPrefab;
    public GameObject ball;
    public int ballCount;
    public bool spawnRight;
    public Vector3 ballSpawn;
    [SerializeField] private float _spawnVelocity;

    private void Start()
    {
        _ballPrefab = (GameObject) Resources.Load("Prefabs/Ball", typeof(GameObject));

        _mainCanvas = (Canvas) Camera.main.GetComponentInChildren(typeof(Canvas));
        _uiButtons = (UIButtons) _mainCanvas.GetComponent(typeof(UIButtons));
        _scoreLeft = (TMP_Text) _mainCanvas.transform.GetChild(3).GetChild(0).GetComponent(typeof(TMP_Text));
        _scoreRight = (TMP_Text) _mainCanvas.transform.GetChild(3).GetChild(1).GetComponent(typeof(TMP_Text));
        _info = _mainCanvas.transform.GetChild(4).gameObject;

        if (_info == null) return;
        _info.SetActive(false);
    }
    //freeze game time if menuOpen bool is true, and make sure there's always a ball in play
    private void Update()
    {
        Time.timeScale = menuOpen ? 0 : 1;
        if (ballCount > 0) return;
        SpawnBall();
    }
    //add a ball with some starting velocity
    private void SpawnBall()
    {
        ballCount += 1;
        if (gameOver) return;
        var ballSpawnX = spawnRight ? 1.0f : -1.0f;
        ballSpawn = new Vector3(ballSpawnX, 0.0f, Random.Range(-1.5f, 1.5f));
        ball = Instantiate(_ballPrefab, ballSpawn, Quaternion.identity, transform);
        var newBallRigidBody = (Rigidbody) ball.GetComponent(typeof(Rigidbody));
        newBallRigidBody.AddExplosionForce(_spawnVelocity, Vector3.zero, 10.0f, 0.0f, ForceMode.Impulse);
    }
    //after goal, update ui scoreboard
    public void UpdateScores()
    {
        _scoreLeft.text = playerLeft.playerScore.ToString();
        _scoreRight.text = playerRight.playerScore.ToString();
        WinTest();
    }
    //check scores and determine if human won or lost
    private void WinTest()
    {
        if (playerLeft.playerScore >= 11)
        {
            ShowInfo("YOU LOSE!!!");
            gameOver = true;
            _uiButtons.DisplayLeaderBoard(true);
        }

        if (playerRight.playerScore < 11) return;
        ShowInfo("YOU WIN!!!");
        gameOver = true;
        _highScore = (float) playerRight.playerScore / playerLeft.playerScore;
        _uiButtons.DisplayLeaderBoard(true);
        _uiButtons.UpdateRecords(_highScore);
    }
    //show info, goals, fouls, wins, losses
    public void ShowInfo(string message)
    {
        _info.GetComponentInChildren<TMP_Text>().text = message;
        _info.SetActive(true);
        menuOpen = true;
        var timer = StartCoroutine("Timer");
    }
    //info display timer
    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        _info.SetActive(false);
        if (!gameOver) menuOpen = false;
        yield return null;
    }
}

[System.Serializable]
public class Player
{
    public GameObject paddle;
    public GameObject cylinder;
    public int playerScore;
}