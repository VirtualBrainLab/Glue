using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

public class PhotodiodeToggle : MonoBehaviour
{
    private Renderer _renderer;
    private bool _black;
    private SerialPort _serialPort;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _serialPort = new SerialPort("COM4", 9600);
        _serialPort.Open();

        QualitySettings.vSyncCount = 1;
    }

    private void OnDestroy()
    {
        _serialPort.Close();
    }

    // Update is called once per frame
    void Update()
    {
        _black = !_black;
        _renderer.material.color = _black ? Color.black : Color.white;
        Debug.Log($"Set to {_black} at {Time.realtimeSinceStartup}");
    }

    private void LateUpdate()
    {
        //ReadFromSerialPort();
    }

    public void ReadFromSerialPort()
    {
        string data = _serialPort.ReadLine();
        bool high = float.Parse(data) > 3;
        //Debug.Log((high, _black));
        Debug.Log($"Received {high} at {Time.realtimeSinceStartup}");
    }
}