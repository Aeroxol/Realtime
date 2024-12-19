using UnityEngine;
using UnityEngine.UI;
using Best.SocketIO;
using Best.SocketIO.Events;
using System;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get { return _instance; }
    }
    public InputField inputField;
    public Button btnConnect, btnDisconnect, btnMessage;
    public AssetManager assetManager;

    private SocketManager _socketManager;
    private PacketHandler _packetHandler = new PacketHandler();
    private string userId;

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
        // btnConnect.onClick.AddListener(BtnConnect);
        // btnDisconnect.onClick.AddListener(BtnDisconnect);
        btnMessage.onClick.AddListener(BtnMessage);

        _socketManager = new SocketManager(new System.Uri("http://localhost:3000"));

        _socketManager.Socket.On(SocketIOEventTypes.Connect, () => { Debug.Log("Connected"!); });
        _socketManager.Socket.On<string>("connection", OnConnected);
        _socketManager.Socket.On<ResponseMessage>("response", OnResponse);
        _socketManager.Socket.On(SocketIOEventTypes.Disconnect, () => { Debug.Log("Disconnected"!); });

        // require game assets to server
        _socketManager.Socket.Emit("event", new { clientVersion = "1.0.0", handlerId = 1, userId = userId, payload = "test" });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BtnMessage()
    {
        _socketManager.Socket.Emit("event", new { clientVersion = "1.0.0", handlerId = 2, userId = userId, payload = "test" });
    }

    private class ResponseMessage
    {
        public string status;
        public int packetId;
        public string payload;
    }

    void OnConnected(string res)
    {
        try
        {
            Debug.Log(res);
            userId = res;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void OnDisconnected(ConnectResponse res)
    {
        Debug.Log("Disconnected!");
    }

    void OnDestroy()
    {
        _socketManager?.Close();
        _socketManager = null;
    }

    void OnResponse(ResponseMessage payload)
    {
        try
        {
            _packetHandler.ExecuteHandler(payload.packetId, payload.payload);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SendPacket(int packetId, params object[] args)
    {
        Debug.Log($"Send Packet! : {args}");
        _packetHandler.SendHandler(packetId, _socketManager.Socket, userId, args);
    }
}
