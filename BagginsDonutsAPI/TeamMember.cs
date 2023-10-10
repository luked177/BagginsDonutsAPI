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
        public int Donuts { get; set; }
        public int Croissants { get; set; }
    }
}
