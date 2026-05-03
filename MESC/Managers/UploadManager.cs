using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MESC.Managers
{
    public class UploadManager
    {
        public List<string> UploadedFiles { get; } = new List<string>();
        private static readonly string[] Allowed = { ".pdf", ".docx", ".txt", ".pptx" };
        public bool AddFile(string path)
        {
            if (!File.Exists(path)) return false;
            if (!Allowed.Contains(Path.GetExtension(path).ToLower())) return false;
            UploadedFiles.Add(path);
            return true;
        }
    }
}
