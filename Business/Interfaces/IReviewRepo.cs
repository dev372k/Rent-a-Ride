﻿using Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReviewRepo
    {
        void Add(int BookingId, AddReviewDTO dto);
        List<GetReviewDTO> Get(int bookingId);
        List<GetReviewDTO> Get();
    }
}
