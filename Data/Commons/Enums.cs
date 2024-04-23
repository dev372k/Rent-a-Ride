using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Commons
{
    public enum enRole
    {
        Admin,
        User
    }

    public enum enPaymentStatus
    {
        Pending,
        Paid,
        Unpaid,
    }
    public enum enVehicleType
    {
        Sedan,
        SUV,
        Truck,
        Motorcycle,
        Van,
        Convertible,
        Coupe,
        Hatchback,
        Minivan,
        Pickup,
        Bus,
        Electric,
        Hybrid,
        Luxury
    }
    public enum enStripeSessionStatus
    {
        Expire,
        Open,
        Complete
    }

}
