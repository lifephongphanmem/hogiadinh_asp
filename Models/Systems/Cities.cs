using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class Cities
    {
        [Key]
        public int Id { get; set; }
        public string MaTinh { get; set; }
        public string TenTinh { get; set; }
    }
}
