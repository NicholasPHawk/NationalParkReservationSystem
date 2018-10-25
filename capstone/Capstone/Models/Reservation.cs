﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    class Reservation
    {
        public int ReservationId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
