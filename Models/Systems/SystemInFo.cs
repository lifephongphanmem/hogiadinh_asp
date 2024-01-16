using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHN.Models.Systems
{
    public class SystemInFo
    {
        [Key]
        public int Id { get; set; }
        public string MaTinh { get; set; }    
        public string KyHieuTinhTP { get; set; }
        public string Url { get; set; }
        public bool SSO { get; set; }
        public int YearStart { get; set; }
        public int LoginLock { get; set; }
        public string KeyConnect { get; set; }
    }
}
