using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PupilBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _bigReflectionGO;
    [SerializeField] private GameObject _smallReflectionGO;

    [SerializeField] private PhotodiodeToggle _photodiode;
    [SerializeField] private ArduinoSerialComms _arduino;

    private int _port = 5554;

    private const float SPEED = 2f;
    private const float EDGE = 0.3f;
    private bool rightward = true;

    private Thread subscriberThread;
    public string lastMsg;
    public int updateCount;
    public bool subscriberAlive;

    private string _ip;

    private void Awake()
    {
        updateCount = 0;
        AsyncIO.ForceDotNet.Force();
        //NetMQConfig.Linger = new TimeSpan(0, 0, 1);

        QualitySettings.maxQueuedFrames = 1;
        QualitySettings.vSyncCount = 0;
    }

    public void StartupEyetracker(string ip)
    {
        _ip = ip;
        subscriberAlive = true;
        subscriberThread = new Thread(SubscribeThread);
        subscriberThread.Start();
    }

    private void OnDestroy()
    {
        if (!subscriberAlive)
            subscriberThread.Abort();
        subscriberAlive = false;
    }

    private void SubscribeThread()
    {
        using (var subSocket = new SubscriberSocket())
        {
            //subSocket.Options.Linger = new TimeSpan(0, 0, 1);
            subSocket.Connect($"tcp://{_ip}:{_port}");
            subSocket.Subscribe("");
            Debug.Log($"Connecting to tcp://{_ip}:{_port}");

            while (subscriberAlive)
            {
                lastMsg = subSocket.ReceiveFrameString();
                //updateCount++;
            }

            subSocket.Close();
            NetMQConfig.Cleanup();
            Debug.Log("Thread killed");
        }
    }

    void Update()
    {
        //Debug.Log(updateCount);
        //updateCount = 0;

        Vector3 posB = _bigReflectionGO.transform.position;
        bool left = posB.x < 0;
        posB.x += (rightward ? 1 : -1) * SPEED * Time.deltaTime;

        if (left && posB.x > 0)
        {
            _arduino.SendByte(1);
            _photodiode.FlashPhotodiode();
        }

        if (posB.x > EDGE)
        {
            rightward = false;
            // bounce off the edge
            posB.x = EDGE - Mathf.Abs(posB.x - EDGE);
        }
        if (posB.x < -EDGE)
        {
            rightward = true;
            posB.x = -EDGE + Mathf.Abs(posB.x - -EDGE);
        }

        _bigReflectionGO.transform.position = posB;

        if (lastMsg != null && lastMsg.Length > 0)
        {
            int commaIdx = lastMsg.IndexOf(',');
            string x = lastMsg.Substring(1, commaIdx - 1);
            string y = lastMsg.Substring(commaIdx + 2, lastMsg.Length - commaIdx - 2);

            Vector3 posS = _smallReflectionGO.transform.position;

            bool leftS = posS.x < 0;

            posS.x = float.Parse(x);

            if (leftS && posS.x > 0)
            {
                _arduino.SendByte(1);
                _photodiode.FlashPhotodiode();
            }

            _smallReflectionGO.transform.position = posS;
        }
    }
}
