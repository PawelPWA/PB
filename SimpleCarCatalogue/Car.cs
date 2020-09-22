using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCarCatalogue
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(256)")]
        [DisplayName("Car name")]
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Description { get; set; }

        [Column(TypeName = "int")]
        [Required]
        [Range(1900, 2020)]
        public int Year { get; set; }

        [Column(TypeName = "bit")]
        public bool IsDeleted { get; set; }

        [ForeignKey("Producer")]
        [Required]
        public Guid ProducerId { get; set; }

        public virtual Producer Producer { get; set; }
    }
}
