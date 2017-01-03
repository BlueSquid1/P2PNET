namespace P2PNET.EventArgs
{
    public class ObjReceivedEventArgs : System.EventArgs
    {
        public object Object { get; }
        public System.Type ObjType { get; }
        public TransportType BindingType { get; }
        public string RemoteIp { get; }

        public ObjReceivedEventArgs(string remoteIp, object mObject, System.Type objType, TransportType bindType)
        {
            this.RemoteIp = remoteIp;
            this.Object = mObject;
            this.ObjType = objType;
            this.BindingType = bindType;
        }
    }
}
