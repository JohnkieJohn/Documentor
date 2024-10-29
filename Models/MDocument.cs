using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentor.Models
{
    public class MDocument
    {
        public int Id { get; set; }
        public int? TemplateId { get; set; }
        public int SizeId { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public DateTime Modified { get; set; }
        public List<MPage> Pages = new List<MPage>();

        public MDocument(int id, int? templateId, int sizeId, string name, DateTime added, DateTime modified) 
        {
            this.Id = id;
            this.TemplateId = templateId;
            this.SizeId = sizeId;
            this.Name = name;
            this.Added = added;
            this.Modified = modified;
        }

        public MDocument(int? newTemplateId, int newSizeId, string newName)
        {
            this.TemplateId = newTemplateId;
            this.SizeId = newSizeId;
            this.Name = newName;
        }
    }
}
