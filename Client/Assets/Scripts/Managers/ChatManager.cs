using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    private static ChatManager _instance;
    public static ChatManager Instance{
        get{return _instance;}
    }
    public ChatMessage chatMessagePrefab;
    public TMPro.TMP_InputField chatInput;
    public Transform chatBox;
    public Button btnSend;

    void Awake(){
        if(null == _instance) _instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(SendChatMessage);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            SendChatMessage();
        }
    }

    public void SendChatMessage(){
        string _message = chatInput.text;
        chatInput.text = "";
        if(string.IsNullOrWhiteSpace(_message)) return;
        NetworkManager.Instance.SendPacket(33, _message);
    }

    public void AddMessage(string msg){
        ChatMessage cm = Instantiate(chatMessagePrefab, chatBox.transform);
        cm.SetText(msg);
    }
}
