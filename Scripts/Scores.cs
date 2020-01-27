using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;

public class Scores : MonoBehaviour
{
    public List<GameObject> scoresList = new List<GameObject>();
    private GameObject _scorePrefab;
    private const float ScoreOffset = 36.0f;
    
    private static string _savedScores;
    private static TextAsset _default;
    private string _savedData;
    public string[] scoresData = new string[10];
    [SerializeField] private TMP_InputField insertScore;

    private void Awake()
    {
        _savedScores = Application.persistentDataPath + "/SavedScores.txt";
        _default = (TextAsset) Resources.Load("LeaderBoard", typeof(TextAsset));
        _savedData = File.Exists(_savedScores) ? File.ReadAllText(_savedScores) : _default.text;
        _scorePrefab = (GameObject) Resources.Load("Prefabs/Score", typeof(GameObject));
        var scorePosition = transform.position;
        for (var i = 0; i < 10; i++)
        {
            var createScore = Instantiate(_scorePrefab, scorePosition, Quaternion.identity, transform);
            scoresList.Add(createScore);
        }
        RefreshScores();
        
        scoresData = _savedData.Split('\n');
        for (var i = 0; i < scoresList.Count; i++)
        {
            var scoreParts = scoresData[i].Split(',');
            scoresList[i].transform.GetChild(0).GetComponent<TMP_Text>().text = scoreParts[0];
            scoresList[i].transform.GetChild(1).GetComponent<TMP_Text>().text = scoreParts[1];
        }
    }
    //create a new score object with the high score, and insert it into the score list, and listen for user input
    public void EnterNewScore(float highScore)
    {
        var scoreSet = false;
        var scoresListCount = scoresList.Count;
        for (var i = 0; i < scoresListCount; i++)
        {
            if (highScore < float.Parse(scoresList[i].transform.GetChild(1).GetComponent<TMP_Text>().text)) continue;
            if (scoreSet) break;
            var createScorePosition = transform.position;
            createScorePosition.y -= ScoreOffset * i;
            var createScore = Instantiate(_scorePrefab, createScorePosition, Quaternion.identity, transform);
            scoresList.Insert(i, createScore);
            scoresList[scoresList.Count - 1].gameObject.SetActive(false);
            scoresList.RemoveAt(scoresList.Count - 1);
            createScore.transform.GetChild(1).GetComponent<TMP_Text>().text = highScore.ToString();
            RefreshScores();
            insertScore.gameObject.SetActive(true);
            createScorePosition.y -= ScoreOffset * 0.5f;
            insertScore.transform.position = createScorePosition;
            EventSystem.current.SetSelectedGameObject(insertScore.gameObject);
            insertScore.onEndEdit.AddListener(delegate { SubmitScore(insertScore, createScore); });
            scoreSet = true;
        }
    }
    //take user input (name) into score object, save scores to file
    private void SubmitScore(TMP_InputField input, GameObject scoreObject)
    {
        if (input.text.Length <= 0) return;
        scoreObject.transform.GetChild(0).GetComponent<TMP_Text>().text = input.text;
        input.gameObject.SetActive(false);
        SaveScores();
    }
    //score object alignment
    private void RefreshScores()
    {
        for (var i = 0; i < scoresList.Count; i++ )
        {
            var scorePosition = transform.position;
            scorePosition.y -= ScoreOffset * i;
            scoresList[i].transform.position = scorePosition;
        }
    }
    //create a string array containing names and scores, then write to file
    private void SaveScores()
    {
        for (var i = 0; i < 10; i++)
        {
            var newScoreData = scoresList[i].transform.GetChild(0).GetComponent<TMP_Text>().text + "," +
                               scoresList[i].transform.GetChild(1).GetComponent<TMP_Text>().text;
            scoresData[i] = newScoreData;
        }
        File.WriteAllLines(_savedScores, scoresData);
    }
}