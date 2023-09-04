using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolService.Models.DBModels
{
    [Table("Cities")]
    public partial class City
    {
        [Key]
        public int CityId { get; set; }

        public string? CityName { get; set; }
    }
}
