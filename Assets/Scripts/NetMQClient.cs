using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine.Assertions;

namespace MH
{
    ///<summary>
    /// test Req-Rep with NetMQ
    ///</summary>
    public class ReqRepNMQ : MonoBehaviour
    {
        public int _port = 5555;

        public RequestSocket _client;
        public ResponseSocket _server;

        public float _interval = 0.5f; // wait interval

        private float _timer = 0;
        private int _cnter = 1;

        void Start()
        {
            AsyncIO.ForceDotNet.Force();
            NetMQConfig.Linger = new TimeSpan(0, 0, 1);

            //_server = new ResponseSocket();
            //_server.Options.Linger = new TimeSpan(0, 0, 1);
            //_server.Bind($"tcp://*:{_port}");
            //print($"server on {_port}");

            _client = new RequestSocket();
            _client.Options.Linger = new TimeSpan(0, 0, 1);
            _client.Connect($"tcp://localhost:{_port}");
            print($"client connects {_port}");

            //Assert.IsNotNull(_server);
            Assert.IsNotNull(_client);
        }

        void OnDisable()
        {
            _client?.Dispose();
            //_server?.Dispose();
            NetMQConfig.Cleanup(false);
        }

        void Update()
        {
            //_timer += Time.deltaTime;
            //if (_timer >= _interval)
            //{
            //    _timer = 0;
            //    var c_sent = $"Request {_cnter}";
            //    _client.SendFrame(c_sent);

            //    print($"client sents: {c_sent} at {Time.realtimeSinceStartup}");

            //    //var s_recv = _server.ReceiveFrameString();
            //    //print($"server receives {s_recv}");

            //    //var s_sent = $"Response {_cnter}";
            //    //_server.SendFrame(s_sent);
            //    //print($"Server sents: {s_sent}");

            //    var c_recv = _client.ReceiveFrameString();
            //    print($"client receives {c_recv} at {Time.realtimeSinceStartup}");

            //    _cnter++;
            //}
        }

        
    }
}