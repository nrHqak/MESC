using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using MESC.Models;

namespace MESC.Managers
{
    public class MaterialManager
    {
        private readonly string _materialsPath;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        public MaterialManager(string materialsPath) { _materialsPath = materialsPath; }
        public List<MaterialItem> GetAll() => File.Exists(_materialsPath)
            ? _serializer.Deserialize<List<MaterialItem>>(File.ReadAllText(_materialsPath)) ?? new List<MaterialItem>()
            : new List<MaterialItem>();

        public IEnumerable<MaterialItem> GetByGrade(int grade) => GetAll().Where(m => m.Grade == grade);
        public IEnumerable<MaterialItem> Search(string q, int grade) => GetByGrade(grade).Where(m =>
            m.Title.ToLower().Contains(q.ToLower()) ||
            m.Subject.ToLower().Contains(q.ToLower()) ||
            m.Recommendations.ToLower().Contains(q.ToLower()));
        public IEnumerable<MaterialItem> Recommend(int grade) => GetByGrade(grade).OrderBy(_ => System.Guid.NewGuid()).Take(3);

        public void AddMaterial(MaterialItem material)
        {
            var all = GetAll();
            all.Add(material);
            File.WriteAllText(_materialsPath, _serializer.Serialize(all));
        }
    }
}
