using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomName { get; set; }
		public string Building { get; set; }
		public string RoomNumber { get; set; }
		public int Capacity { get; set; }
		public bool HasSpeakers{ get; set; }
		public bool HasProjector{ get; set; }
		public bool HasComputers{ get; set; }

    }
}