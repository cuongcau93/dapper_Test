using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// Represents a non-existing file.
    /// </summary>
    public class NotFoundFileInfo : ISettingFileInfo
    {
        public NotFoundFileInfo(string fileName)
        {
            Name = fileName;
        }

        /// <summary>
        /// Always false
        /// </summary>
        public bool Exists => false;

        /// <summary>
        /// Always equals -1
        /// </summary>
        public long Length => -1;

        /// <summary>
        /// Always null
        /// </summary>
        public string PhysicalPath => null;

        /// <inheritdoc />
        public string Name { get; }

        /// <summary>
        /// Returns <see cref="DateTimeOffset.MinValue"/>
        /// </summary>
        public DateTimeOffset LastModified => DateTime.MinValue;

        /// <summary>
        /// Always throws. A stream cannot be created for non-exists file
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Always thrown</exception>
        /// <returns>Does not return</returns>
        public Stream CreateReadStream()
        {
            throw new FileNotFoundException($"The file {Name} does not exist.");
        }
    }
}
