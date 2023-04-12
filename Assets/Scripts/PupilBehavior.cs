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

    private int _port = 5554;

    private const float SPEED = 3f;
    private const float EDGE = 0.3f;
    private bool rightward = true;

    private Thread subscriberThread;
    public string lastMsg;
    public int updateCount;
    public bool subscriberAlive;

    private void Awake()
    {
        updateCount = 0;
        AsyncIO.ForceDotNet.Force();
        //NetMQConfig.Linger = new TimeSpan(0, 0, 1);

        subscriberAlive = true;
        subscriberThread = new Thread(SubscribeThread);
        subscriberThread.Start();

        QualitySettings.maxQueuedFrames = 1;
        QualitySettings.vSyncCount = 1;
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
            subSocket.Connect($"tcp://localhost:{_port}");
            subSocket.Subscribe("");
            Debug.Log($"Connecting to tcp://localhost:{_port}");

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
        posB.x += (rightward ? 1 : -1) * SPEED * Time.deltaTime;
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
            posS.x = float.Parse(x);
            _smallReflectionGO.transform.position = posS;
        }
    }
}
