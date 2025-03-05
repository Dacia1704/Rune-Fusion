using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketTest : MonoBehaviour
{
    private SocketIOUnity socket;
    void Start()
    {
        socket = new SocketIOUnity("http://localhost:3000");
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("Socket connected");
        };
        
        socket.On("hi", data =>
        {
            Debug.Log(data);
        });
        
        
        socket.Connect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            socket.Emit("test",123);
            // socket.Disconnect();
        }
    }
}
