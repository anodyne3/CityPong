using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private GameObject _leaderBoard;
    private GameObject _menuBoard;
    
    private void Start()
    {
        _leaderBoard = transform.GetChild(2).gameObject;
        _leaderBoard.SetActive(false);
        if (gameManager == null) return;
        _menuBoard = transform.GetChild(1).gameObject;
        _menuBoard.SetActive(false);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
 
    public void DisplayLeaderBoard(bool value)
    {
        _leaderBoard.gameObject.SetActive(value);
    }
    
    public void DisplayMenu(bool value)
    {
        if (gameManager.gameOver) ExitGame();
        _menuBoard.gameObject.SetActive(value);
        gameManager.menuOpen = value;
    }
    
    public void UpdateRecords(float highScore)
    {
        _leaderBoard.transform.GetChild(2).GetComponent<Scores>().EnterNewScore(highScore);
    }
}