using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using astra.http;

namespace RelayWebServer
{
    public class Program
    {
        HttpImplementation webServer;
        RelayShield relayShield;

        public static void Main()
        {
            new Program();
        }

        public Program()
        {
            Debug.Print(Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);
            relayShield = new RelayShield();
            webServer = new HttpSocketImpl(processResponse);
            webServer.Listen();
        }

        protected void processResponse(HttpContext context)
        {
            String target = context.Request.Path;
            String responseData = "";
            bool redirecting = false; 

            // http://netduino/
            if (target == Names.targetRoot)
            {
                responseData = WebResources.GetString(WebResources.StringResources.index);
                redirecting = false;
            }
            else if (target == Names.targetOpenAll)
            {
                relayShield.OpenAll();
                context.Response.setRedirect(Names.targetRoot);
                redirecting = true; 
            }
            else if (target == Names.targetCloseAll)
            {
                relayShield.CloseAll();
                context.Response.setRedirect(Names.targetRoot);
                redirecting = true; 
            }
            else if (target == Names.targetDo)
            {
                String port = context.Request.getParameter(Names.portParameter);
                String action = context.Request.getParameter(Names.actionParameter);
                
                int p = int.Parse(port);
                bool b = (action == "true" ? true : false);

                if (b == true)
                {
                    relayShield.Open(p);
                }
                else
                {
                    relayShield.Close(p);
                }

                context.Response.setRedirect(Names.targetRoot);
                redirecting = true; 
            }

            context.Response.ContentType = "text/html";
            context.Response.Write(responseData);

            if (!redirecting)
            {
                context.Response.Add("Connection", "close");
                context.Response.Close();
            }

            redirecting = false; 
        }
    }
}
