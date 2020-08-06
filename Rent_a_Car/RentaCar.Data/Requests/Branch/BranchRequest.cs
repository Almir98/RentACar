﻿using System;

namespace RentaCar.Data.Requests.Branch
{
    public class BranchRequest
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public string Description { get; set; }
    }
}
