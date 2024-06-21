using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagginsDonutsAPI
{
    internal class MapNameToIds
    {
        private Dictionary<string, (string UserId, string Id)> nameToDetails = new Dictionary<string, (string UserId, string Id)>();

        public MapNameToIds()
        {
            nameToDetails["Luke"] = ("40dfa3f5-ba68-4086-a450-f7f5eb723215", "4afd8aa1-cc5f-4df4-b2d5-f73d86847240");
            nameToDetails["Cal"] = ("bbc2ed5a-c123-4366-830a-eddebecfd464", "3d6cb388-a58e-4bd6-9cf0-8c71302883b9");
            nameToDetails["Jordan"] = ("50ff6793-c435-42d0-8d40-6f9e567a63d2", "35a16a94-6713-4877-ad74-35c979b88884");
            nameToDetails["Chris"] = ("b461477e-0dfe-4956-ae44-02ae5f2f9dfa", "46bdb09b-e8af-40ae-8609-e77ca09f5f88");
            nameToDetails["Jamie"] = ("b3a82266-07cf-4963-b975-54e368c01c1f", "99845499-47f4-4cd2-bf2f-32490dadfab1");
            nameToDetails["Feargus"] = ("4116e6a7-3617-4f3c-94aa-c9c56cf4aef3", "0692a734-da7b-4d7e-b8bb-71d00d853e2e");
            nameToDetails["Liam"] = ("f90b4327-ca08-4893-b6e8-c4b3011088f0", "eabb79d6-0173-4c09-be29-da8222ece0f0");
            nameToDetails["Andy"] = ("d2e80bf8-c2e5-4500-b4de-64816dc9ab2c", "5e4d2221-873e-482b-94de-4c34f084ddde");
            nameToDetails["Martino"] = ("40820a84-0531-48b3-a4b1-79ccb4b6c668", "8963ca8f-b9ce-4dca-bc18-b8fa15917f68");
            nameToDetails["Rocket"] = ("f86fb4dd-e65f-4f66-bc43-89c977323707", "f0c52405-926e-4f42-a16c-1e5015af9fca");
        }

        public (string UserId, string Id) GetUserDetails(string name)
        {
            if (nameToDetails.ContainsKey(name))
            {
                return nameToDetails[name];
            }
            else
            {
                throw new KeyNotFoundException("Name not found.");
            }
        }
    }
}
