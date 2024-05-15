using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OOAD.Models
{
    public class CalenderApointment
    {
        [Key]
        public int CalenderID {  get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar")]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start {  get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        public int UserID { get; set; }





    }
}
