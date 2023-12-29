using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.BLL.DTOs
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public int Quantity { get; set; }

        public List<ClaimDTO> Claims { get; set; } = new List<ClaimDTO>();
    }

    public class ClaimDTO
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Label => Value.Replace("Permissions.", "").Replace(".", " ");
    }
}
