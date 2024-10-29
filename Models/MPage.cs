using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentor.Models
{
    public class MPage
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int Number { get; set; }

        public MPage(int id, int documentId, int number)
        {
            this.Id = id;
            this.DocumentId = documentId;
            this.Number = number;
        }

        public MPage(int documentId, int newNumber)
        {
            this.DocumentId = documentId;
            this.Number = newNumber;
        }
    }
}
