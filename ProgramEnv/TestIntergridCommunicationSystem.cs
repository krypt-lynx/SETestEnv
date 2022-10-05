using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    public class TestMessageProvider : IMyMessageProvider
    {
        public bool HasPendingMessage => false;

        public int MaxWaitingMessages => 25;

        public MyIGCMessage AcceptMessage() => default(MyIGCMessage);

        public void DisableMessageCallback() { }

        public void SetMessageCallback(string argument = "") { }
    }

    public class TestUnicastListener : TestMessageProvider, IMyUnicastListener { }

    public class TestBroadcastListener : TestMessageProvider, IMyBroadcastListener
    {
        public TestBroadcastListener(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; private set; }

        public bool IsActive => true;
    }

    public class TestIntergridCommunicationSystem : IMyIntergridCommunicationSystem
    {
        long me = 0;

        public TestIntergridCommunicationSystem(long entityId)
        {
            me = entityId;
        }

        public long Me => me;

        public IMyUnicastListener UnicastListener { get; private set; } = new TestUnicastListener();

        public void DisableBroadcastListener(IMyBroadcastListener broadcastListener) { }

        public void GetBroadcastListeners(List<IMyBroadcastListener> broadcastListeners, Func<IMyBroadcastListener, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public bool IsEndpointReachable(long address, TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay) => false;

        public IMyBroadcastListener RegisterBroadcastListener(string tag) { return new TestBroadcastListener(tag); }

        public void SendBroadcastMessage<TData>(string tag, TData data, TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay) { }

        public bool SendUnicastMessage<TData>(long addressee, string tag, TData data) { return false; }
    }
}
