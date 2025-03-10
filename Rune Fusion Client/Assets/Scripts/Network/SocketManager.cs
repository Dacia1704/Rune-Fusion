using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SocketManager : MonoBehaviour
{
    private SocketIOUnity socket;

    private MapData mapDataPayload;
    
    
    void Start()
    {
        mapDataPayload = new MapData
        {
            cols = GameManager.Instance.GameManagerSO.WidthRuneMap,
            rows = GameManager.Instance.GameManagerSO.HeightRuneMap,
            numTypes = Enum.GetValues(typeof(RuneType)).Length,
        }; 
        
        socket = new SocketIOUnity("http://localhost:3000");
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket connected");
            MapDataRequest();
        };
        
        socket.On(SocketEvents.Rune.GENERATE_START_RESPONSE, data =>
        {
            List<List<List<int>>> mapData = JsonConvert.DeserializeObject<List<List<List<int>>>>(data.ToString());
            UnityMainThreadDispatcher.Instance().Enqueue(GenMapCoroutine(mapData[0]));
        });
        socket.Connect();
        
    }
    
    private IEnumerator GenMapCoroutine(List<List<int>> mapData)
    {
        GameManager.Instance.RuneManager.GenerateGrid(mapData);
        GameManager.Instance.SetUpTilePosition();
        yield return null;
    }

    private void MapDataRequest()
    {
        string jsonData = JsonUtility.ToJson(mapDataPayload);
        socket.Emit(SocketEvents.Rune.GENERATE_START_REQUEST, jsonData);
    }

    public void RequestNewRune(string mapData)
    {
        socket.Emit(SocketEvents.Rune.NEW_REQUEST, mapData);
    }
    
    
}
