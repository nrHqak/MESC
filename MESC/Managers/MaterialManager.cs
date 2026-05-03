using System.Collections.Generic;
using System.IO;
using System.Linq;
using MESC.Models;
using Newtonsoft.Json;

namespace MESC.Managers
{
    public class MaterialManager
    {
        private readonly string _materialsPath;
        public MaterialManager(string materialsPath) { _materialsPath = materialsPath; }
        public List<MaterialItem> GetAll() => File.Exists(_materialsPath)
            ? JsonConvert.DeserializeObject<List<MaterialItem>>(File.ReadAllText(_materialsPath)) ?? new List<MaterialItem>()
            : new List<MaterialItem>();

        public IEnumerable<MaterialItem> GetByGrade(int grade) => GetAll().Where(m => m.Grade == grade);
        public IEnumerable<MaterialItem> Search(string q, int grade) => GetByGrade(grade).Where(m =>
            m.Title.ToLower().Contains(q.ToLower()) ||
            m.Subject.ToLower().Contains(q.ToLower()) ||
            m.Recommendations.ToLower().Contains(q.ToLower()));
        public IEnumerable<MaterialItem> Recommend(int grade) => GetByGrade(grade).OrderBy(_ => System.Guid.NewGuid()).Take(3);
    }
}
