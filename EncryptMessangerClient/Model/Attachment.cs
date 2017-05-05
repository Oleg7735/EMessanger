using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Model
{
    public class Attachment
    {
        private string _fileName;
        private long _size;
        private string _path;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Имя прикрепленного файла</param>
        /// <param name="size">Размер прикрепленного файла в Kb</param>
        public Attachment(string fileName, long size)
        {
            FileName = fileName;
            Size = size;

        }
        public Attachment(string fileName, long size, string path)
        {
            FileName = fileName;
            Size = size;
            Path = path;

        }

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
            }
        }

        public long Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
            }
        }
    }
}
