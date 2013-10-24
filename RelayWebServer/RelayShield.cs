using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace RelayWebServer
{
    public class RelayShield : IRelayShield
    {
        private OutputPort[] relayPorts = new OutputPort[4];
        private bool[] relayTable = new bool[] { false, false, false, false };

        #region Constructor
        public RelayShield()
        {
            relayPorts[3] = new OutputPort(Pins.GPIO_PIN_D7, false);
            relayPorts[2] = new OutputPort(Pins.GPIO_PIN_D6, false);
            relayPorts[1] = new OutputPort(Pins.GPIO_PIN_D5, false);
            relayPorts[0] = new OutputPort(Pins.GPIO_PIN_D4, false);
        }
        #endregion

        #region Interface Implementation
        public bool IsEnabled(int port)
        {
            if (port < 0 || port > 3) return false;

            return relayTable[port];
        }

        public void CloseAll()
        {
            for (int i = 0; i < 4; i++)
            {
                relayTable[i] = false;
                relayPorts[i].Write(relayTable[i]);
            }
        }

        public void OpenAll()
        {
            for (int i = 0; i < 4; i++)
            {
                relayTable[i] = true;
                relayPorts[i].Write(relayTable[i]);
            }
        }

        public void Close(int port)
        {
            if (port < 0 || port > 3) return;

            relayTable[port] = false;
            relayPorts[port].Write(relayTable[port]);
        }

        public void Open(int port)
        {
            if (port < 0 || port > 3) return;

            relayTable[port] = true;
            relayPorts[port].Write(relayTable[port]);
        }

        #endregion
    }
}
