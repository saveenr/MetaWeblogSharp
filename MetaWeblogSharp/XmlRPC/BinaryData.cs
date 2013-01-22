﻿namespace MetaWeblogSharp.XmlRPC
{
    public class Base64Data
    {
        public byte[] Bytes;

        public Base64Data(byte[] bytes)
        {
            this.Bytes = bytes;
        }

        public static string TypeString
        {
            get { return "base64"; }
        }
    }
}