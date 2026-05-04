using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using MESC.Models;

namespace MESC.Managers
{
    public class DriveLinkManager
    {
        private readonly string _path;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        public DriveLinkManager(string path) { _path = path; }
        public List<DriveLink> GetAll() => File.Exists(_path)
            ? _serializer.Deserialize<List<DriveLink>>(File.ReadAllText(_path)) ?? new List<DriveLink>()
            : new List<DriveLink>();
    }
}
