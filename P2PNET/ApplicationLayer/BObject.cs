using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    public class BObject
    {
        private byte[] objData;
        private Serializer serializer;

        //constructor
        public BObject(byte[] msg, Serializer mSerializer)
        {
            this.objData = msg;
            this.serializer = mSerializer;
        }

        public T GetObject<T>()
        {
            return serializer.DeserializeObjectBSON<T>(objData);
        }

    }
}
