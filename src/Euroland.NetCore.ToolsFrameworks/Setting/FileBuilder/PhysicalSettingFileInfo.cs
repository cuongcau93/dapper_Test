using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    public class PhysicalSettingFileInfo : ISettingFileInfo
    {
        private readonly System.IO.FileInfo _fileInfo;
        public PhysicalSettingFileInfo(System.IO.FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public bool Exists => _fileInfo.Exists;

        public long Length => _fileInfo.Length;

        public string PhysicalPath => _fileInfo.FullName;

        public string Name => _fileInfo.Name;

        public DateTimeOffset LastModified => _fileInfo.LastWriteTimeUtc;

        public Stream CreateReadStream()
        {
            // Buffer size to 1 to prevent FileStream from allocating it's internal buffer
            // 0 causes constructor to throw
            var buffersize = 1;
            return new System.IO.FileStream(
                PhysicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                buffersize, 
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );
        }
    }
}
