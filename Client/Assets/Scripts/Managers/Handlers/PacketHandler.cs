using System;
using System.Collections.Generic;
using UnityEngine;
using Best.SocketIO;

public class PacketHandler
{
    public Dictionary<int, Action<string>> HandlerMappings;
    public Dictionary<int, Delegate> SendHandlerMappings;


    public PacketHandler()
    {
        HandlerMappings = new Dictionary<int, Action<string>>
    {
        {0, TestHandler},
        {1, GameAssetsHandler},
        {2, GameStartHandler},
        {3, GameEndHandler},
        {11, MoveStageHandler},
        {22, GetItemHandler},
        {33, MessageHandler}

    };

        SendHandlerMappings = new Dictionary<int, Delegate>
        {
        {2, new Action<Socket, string, object[]>(SendGameStartHandler) },
        {3, new Action<Socket, string, object[]>(SendGameEndHandler)},
        {11, new Action<Socket, string, object[]>(SendMoveStageHandler)},
        {22, new Action<Socket, string, object[]>(SendGetItemHandler)},
        {33, new Action<Socket, string, object[]>(SendMessageHandler)},
    };
    }

    #region receive
    public void ExecuteHandler(int packetId, string payload)
    {
        try
        {
            Debug.Log(packetId);
            if (HandlerMappings.TryGetValue(packetId, out Action<string> handler))
            {
                handler.Invoke(payload);
            }
            else
            {
                Debug.Log("no packet handler");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private class ErrorMessage
    {
        public string status;
        public string message;
    }

    public void TestHandler(string payload)
    {
        try
        {
            Debug.Log(payload);
            ErrorMessage e = JsonUtility.FromJson<ErrorMessage>(payload);
            Debug.Log($"status: {e.status} : message : {e.message}");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void GameAssetsHandler(string payload)
    {
        try
        {
            GameManager.Instance.assetManager.LoadAsset(payload);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void GameStartHandler(string payload)
    {
        GameManager.Instance.GameStart();
    }

    public void GameEndHandler(string payload)
    {
        GameManager.Instance.EndGame();
    }

    public void MoveStageHandler(string payload)
    {
        Debug.Log("MoveStagePacket");
        GameManager.Instance.MoveStage(int.Parse(payload));
    }
    
    public void GetItemHandler(string payload){
        GameManager.Instance.AddScore(GameManager.Instance.assetManager.CurrentItemScore);
    }

    // payload: int
    public void MessageHandler(string payload){
        Debug.Log("MessagePacket");
        ChatManager.Instance.AddMessage(payload);
    }
    #endregion

    #region send
    public void SendHandler(int packetId, Socket socket, string userId, params object[] args)
    {
        if (SendHandlerMappings.TryGetValue(packetId, out Delegate handler))
        {
            try
            {
                handler.DynamicInvoke(socket, userId, args);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error: {e.Message}");
            }
        }
        else
        {
            Debug.Log("no packet handler");
        }
    }

    public void SendGameStartHandler(Socket socket, string userId, object[] args)
    {
        Debug.Log($"Send Game Start Packet! : {args[0]}");
        socket.Emit("event",
        new
        {
            clientVersion = "1.0.0",
            handlerId = 2,
            userId = userId,
            payload =
            new
            {
                timestamp = args[0]
            }
        });
    }

    public void SendGameEndHandler(Socket socket, string userId, object[] args)
    {
        Debug.Log($"Send Game End Packet! : {args[0]}, {args[1]}");

        socket.Emit("event",
        new
        {
            clientVersion = "1.0.0",
            handlerId = 3,
            userId = userId,
            payload =
            new
            {
                timestamp = args[0],
                score = args[1]
            }
        });
    }

    public void SendMoveStageHandler(Socket socket, string userId, object[] args)
    {
        Debug.Log($"Send Stage Move Packet! : {args[0]}, {args[1]}");

        socket.Emit("event",
                new
                {
                    clientVersion = "1.0.0",
                    handlerId = 11,
                    userId = userId,
                    payload =
                    new
                    {
                        currentStage = args[0],
                        targetStage = args[1]
                    }
                });
    }

    public void SendGetItemHandler(Socket socket, string userId, object[] args){
        Debug.Log($"Send Get Item Packet! : {args[0]}");

        socket.Emit("event",
        new
        {
            clientVersion = "1.0.0",
            handlerId = 22,
            userId = userId,
            payload =
            new
            {
                score = args[0]
            }
        });
    }

    public void SendMessageHandler(Socket socket, string userId, object[] args)
    {
        Debug.Log($"Send Message Packet! : {args[0]}");

        socket.Emit("event",
        new
        {
            clientVersion = "1.0.0",
            handlerId = 33,
            userId = userId,
            payload =
            new
            {
                message = args[0]
            }
        });
    }
    #endregion
}
