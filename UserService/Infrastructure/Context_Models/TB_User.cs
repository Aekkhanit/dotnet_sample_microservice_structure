using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Context_Models
{
    [Table("TB_User")]
    public class TB_User
    {
        public TB_User()
        {
        }

        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string First_Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Last_Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string Tel { get; set; }
    }
}
