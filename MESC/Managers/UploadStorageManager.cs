using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using MESC.Models;

namespace MESC.Managers
{
    public class UploadStorageManager
    {
        private readonly string _path;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        public UploadStorageManager(string path) { _path = path; }
        public List<UploadItem> GetAll() => File.Exists(_path)
            ? _serializer.Deserialize<List<UploadItem>>(File.ReadAllText(_path)) ?? new List<UploadItem>()
            : new List<UploadItem>();
        public void Add(UploadItem item)
        {
            var all = GetAll();
            all.Add(item);
            File.WriteAllText(_path, _serializer.Serialize(all));
        }
    }
}
