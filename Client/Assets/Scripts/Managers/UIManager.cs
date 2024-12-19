using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }
    public TMPro.TextMeshProUGUI txtScore;
    public CanvasGroup cgStart;
    public CanvasGroup cgEnd;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowStartPanel()
    {
        cgStart.alpha = 1;
    }

    public void HideStartPanel()
    {
        cgStart.alpha = 0;
    }

    public void ShowEndPanel()
    {
        txtScore.text = $"SCORE {GameManager.Instance.GetScore().ToString("0")}";
        cgEnd.alpha = 1;
    }

    public void HideEndPanel()
    {
        cgEnd.alpha = 0;
    }
}
