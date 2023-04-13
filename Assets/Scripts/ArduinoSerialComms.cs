using System.IO.Ports;
using UnityEngine;

public class ArduinoSerialComms : MonoBehaviour
{
    private SerialPort _serialPort;

    private void OnDestroy()
    {
        if (_serialPort != null && _serialPort.IsOpen)
            _serialPort.Close();
    }

    public void SendByte(byte value)
    {
        if (_serialPort != null && _serialPort.IsOpen)
            _serialPort.Write(new byte[] { value }, 0, 1);
    }

    public void SetCommPort(string commPort)
    {
        if (_serialPort != null && _serialPort.IsOpen)
            _serialPort.Close();

        _serialPort = new SerialPort(commPort, 9600);
        _serialPort.Open();
    }
}
