﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Core.Dto
{
    public class PostCodeDTO
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }

        public List<AddressDetailsDTO> AddressDetails { get; set; }
    }
}
