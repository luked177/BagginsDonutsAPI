using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagginsDonutsAPI
{
    internal class DonutSuggestion
    {
        public Guid SuggestionId { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Nominee{ get; set; }
        public string Reason { get; set; }
        public DateTime SuggestionDate { get; set; }

        public DonutSuggestion(string usersName, string nominee, string reason)
        {
            Id = Guid.NewGuid();
            SuggestionId = Guid.NewGuid();
            SuggestionDate = DateTime.Now;
            UserName = usersName;
            Nominee = nominee;
            Reason = reason;
        }
    }
}
