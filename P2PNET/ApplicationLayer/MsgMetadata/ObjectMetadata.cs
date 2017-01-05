using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer.MsgMetadata
{
    class ObjectMetadata : IMetadata
    { 
        public string MsgType
        {
            get
            {
                return msgType;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
        }

        private string msgType;
        private string version;

        //private constructor
        //use Create( ) instead
        private ObjectMetadata()
        {

        }

        public void Create<T>( T obj, string mVersion = "1.0" )
        {
            this.msgType = obj.GetType().Name;
            this.version = mVersion;
        }
    }
}
