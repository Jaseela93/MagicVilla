using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTo> villaList= new List<VillaDTo>
            {
                new VillaDTo {Id=1,Name="Pool View"},
                new VillaDTo {Id=2,Name="Beach View"}
            };
    }
}
