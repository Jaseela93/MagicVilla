using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTo> villaList= new List<VillaDTo>
            {
                new VillaDTo {Id=1,Name="Pool View",Sdft=100,Occupancy=4},
                new VillaDTo {Id=2,Name="Beach View",Sdft=300,Occupancy=3}
            };
    }
}
