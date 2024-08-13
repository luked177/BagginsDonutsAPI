using System;
using System.Collections.Generic;

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
        public List<ArchivedBoxes> ArchivedBoxes { get; set; }
    }

    internal class Award
    {
        public DateTime AwardedDate { get; set; }
        public string AwardedReason { get; set; }
        public Guid AwardId { get; set; }
        public bool IsChristmas { get; set; }

        public Award(string awardReason)
        {
            AwardedDate = DateTime.Now;
            AwardedReason = awardReason;
            AwardId = Guid.NewGuid();
            IsChristmas = DateTime.Now.Month == 12; //Only  give cookies in December
        }
    }

    internal class ArchivedBoxes
    {
        public int number { get; set; }
        public List<Award> donuts { get; set; }
    }
}
