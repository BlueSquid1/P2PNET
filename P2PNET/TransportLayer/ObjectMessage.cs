using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.TransportLayer
{
    public class ObjectMessage
    {
        public Type ObjType { get; }
        object Obj { get; }

        //constructor
        public ObjectMessage(object mObject, Type mType)
        {
            this.Obj = mObject;
            this.ObjType = mType;
        }
    }
}
