using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    public AssetManager assetManager = new AssetManager();
    public TextMeshProUGUI txtScore;
    public Ship player;
    public Button btnGameStart, btnGameEnd;

    // public int hiscore;
    private float _score = 0;
    private int _currentStage = 0;
    public int CurrentStage
    {
        get { return _currentStage; }
        set
        {
            Debug.Log(value);
            _currentStage = value;
        }
    }
    private int _currentStageId = 0;
    public int CurrentStageId
    {
        get { return _currentStageId; }
    }

    private enum GameState
    {
        None = 0,
        InGame = 1,
        EndGame = 2,
    }

    private GameState _state;

    void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        btnGameStart.onClick.AddListener(BtnGameStart);
        btnGameEnd.onClick.AddListener(BtnGameEnd);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != GameState.InGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BtnGameStart();
            }
        }
        if (_state == GameState.InGame)
        {
            _score += Time.deltaTime;
            if (_score > assetManager.stages[_currentStage + 1].score)
            {
                Debug.Log($"Current Level : {_currentStageId}");
                BtnMoveStage();
                _currentStage++;
                _currentStageId = assetManager.NextStageId;
            }
            txtScore.text = $"SCORE {_score.ToString("0")}";
            var _x = Input.GetAxis("Horizontal");
            var _y = Input.GetAxis("Vertical");
            player.rb.AddForce(new Vector3(_x, _y, 0));
        }
    }

    public int GetScore()
    {
        return (int)_score;
    }

    public void AddScore(int value)
    {
        _score += value;
    }

    public void BtnGameStart()
    {
        Debug.Log("GameStart!");
        // args[0] : timestamp
        NetworkManager.Instance.SendPacket(2, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    public void BtnGameEnd()
    {
        Debug.Log("GameEnd!");
        NetworkManager.Instance.SendPacket(3, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), (int)_score);
    }

    public void BtnMoveStage()
    {
        Debug.Log("MoveStage!");
        // args[0] : currentStageId, args[1] : targetStageId
        NetworkManager.Instance.SendPacket(11, _currentStageId, assetManager.NextStageId);
    }

    public void GameStart()
    {
        Debug.Log("Game Start!");
        _state = GameState.InGame;
        UIManager.Instance.HideStartPanel();
        UIManager.Instance.HideEndPanel();
        ObjectManager.Instance.Clear();
        _currentStage = 0;
        _currentStageId = assetManager.FirstStageId;
        _score = 0;
        player.transform.position = Vector3.zero;
        ObjectManager.Instance.SpawnAsteroid(6, _currentStage);
        ObjectManager.Instance.SpawnItem();
    }

    public void EndGame()
    {
        if (_state != GameState.InGame) return;
        UIManager.Instance.ShowEndPanel();
        _state = GameState.EndGame;
        // args[0] : timestamp, args[1] : score
    }

    public void MoveStage()
    {
        ObjectManager.Instance.SpawnAsteroid(2, _currentStage);
    }
}
