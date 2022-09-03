﻿namespace ShopParsers.Http
{
    public class ProxyContainer
    {
        public ProxyContainer(ProxyType proxyType, string host, string port)
        {
            ProxyType = proxyType;
            Host = host;
            Port = port;
        }

        public ProxyType ProxyType { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
