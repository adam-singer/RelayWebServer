using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RelayWebServer
{
    public interface IRelayShield
    {
        bool IsEnabled(int port);
        void CloseAll();
        void OpenAll();
        void Close(int port);
        void Open(int port);
    }
}
