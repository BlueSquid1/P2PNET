namespace P2PNET.FileLayer.EventArgs
{
    public class DebugInfoEventArgs
    {
        public string Msg { get; }
        public DebugInfoEventArgs(string msg)
        {
            this.Msg = msg;
        }
    }
}