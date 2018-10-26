﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }

        public override string ToString()
        {
            return $"{SiteNumber,-10}{MaxOccupancy,-10}{Accessible.ToString(),-10}{MaxRVLength,-10}{Utilities.ToString(),-10}";
        }
    }
}
