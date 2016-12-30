using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using P2P_Advance_Sample.Messages;
using System.IO;

namespace P2P_Advance_Sample
{
    public class Serializer
    {
        public static byte[] SerializeObjectBSON<T>(T keyMsg)
        {
            //seralizes the object to Binary JSON
            //this has better output size when sending binary files (which typically
            //make up the majority of file sizes)
            MemoryStream ms = new MemoryStream();
            using(BsonWriter writer = new BsonWriter(ms))
            { 
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, keyMsg);
            }
            return ms.ToArray();
        }

        public static T DeserializeObjectBSON<T>(byte[] msg)
        {
            MemoryStream ms = new MemoryStream(msg);
            using (BsonReader reader = new BsonReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                T e = serializer.Deserialize<T>(reader);
                return e;
            }
        }

        public static string SerializeObjectJSON<T>(T keyMsg)
        {
            return JsonConvert.SerializeObject(keyMsg);
        }

        public static T DeserializeObjectJSON<T>(string msg)
        {
            return JsonConvert.DeserializeObject<T>(msg);
        }


        public static MessageType GetType(string msg)
        {
            JObject jObj = JObject.Parse(msg);
            return (MessageType)((int)jObj["msgType"]);
        }
    }
}
