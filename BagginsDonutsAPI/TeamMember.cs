using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagginsDonutsAPI
{
    internal class TeamMember
    {
        public Guid UserId { get; set; }
        public Guid id { get; set; }
        public string Name { get; set; }
        public List<Award> Donuts { get; set; }
        public List<Award> Croissants { get; set; }
        public int Score { get; set; }
    }

    internal class Award
    {
        public DateTime AwardedDate { get; set; }
        public string AwardedReason { get; set; }
        public Guid AwardId { get; set; }
        public bool IsChristmas { get; set; }
    }
}
