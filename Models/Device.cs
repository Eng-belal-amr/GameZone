using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Device
    {
        public int Id { get; set; }
        public required string Name { get; set; } // required
        public string? Description { get; set; } // optional
        public string? Icon { get; set; } // optional
        public ICollection<GameDevice> Games { get; set; } = [];
    }



}
