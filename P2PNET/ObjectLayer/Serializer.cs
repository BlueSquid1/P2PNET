using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Text;

namespace P2PNET.ObjectLayer
{
    public class Serializer
    {
        public byte[] SerializeObject<T>(T keyMsg)
        {
            string msgString = SerializeObjectJSON(keyMsg);
            byte[] msgBin = Encoding.UTF8.GetBytes(msgString);
            
            //byte[] msgBin = SerializeObjectBSON(keyMsg);
            return msgBin;
        }

        public T DeserializeObject<T>(byte[] msg)
        {
            string msgString = Encoding.UTF8.GetString(msg, 0, msg.Length);
            T obj  = DeserializeObjectJSON<T>(msgString);
            //T obj = DeserializeObjectBSON<T>(msg);
            return obj;
        }

        //seralizes the object to Binary JSON
        //this has better output size when sending binary files (which typically
        //make up the majority of file sizes)
        private byte[] SerializeObjectBSON<T>(T keyMsg)
        {
            //generate metadata
            //ObjectMetadata x;

            MemoryStream ms = new MemoryStream();
            BsonWriter writer = new BsonWriter(ms);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, keyMsg);
            return ms.ToArray();
        }

        private T DeserializeObjectBSON<T>(byte[] msg)
        {
            MemoryStream ms = new MemoryStream(msg);
            using (BsonReader reader = new BsonReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                T e = serializer.Deserialize<T>(reader);
                return e;
            }
        }

        private string SerializeObjectJSON<T>(T keyMsg)
        {
            return JsonConvert.SerializeObject(keyMsg);
        }

        private T DeserializeObjectJSON<T>(string msg)
        {
            return JsonConvert.DeserializeObject<T>(msg);
        }

        public int ReadInt32(byte[] bytes)
        {
            byte[] bin = new byte[sizeof(int)];
            Array.Copy(bytes, bin, sizeof(int));
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(bin);
            }
            int value = BitConverter.ToInt32(bin, 0);
            return value;
        }

        public byte[] WriteInt32(int value)
        {
            byte[] binary = BitConverter.GetBytes(value);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(binary);
            }
            return binary;
        }
    }
}
