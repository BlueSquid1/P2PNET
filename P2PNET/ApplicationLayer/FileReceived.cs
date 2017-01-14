using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    class FileReceived
    {
        public IFile FileObj { get; set; }
        public Stream FileStream { get; set; }

        //constructor
        public FileReceived(IFile file)
        {
            this.FileObj = file;
        }

        public async Task OpenStream()
        {
            this.FileStream = await FileObj.OpenAsync(FileAccess.ReadAndWrite);
        }

        public async Task CloseStream()
        {
            await this.FileStream.FlushAsync();
        }
    }
}
