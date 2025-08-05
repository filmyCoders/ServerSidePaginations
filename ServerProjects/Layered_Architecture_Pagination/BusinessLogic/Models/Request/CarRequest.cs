using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Request
{
    public class AddCarsRequest
    {

        public string Brand { get; set; }

        public string Classes { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Model_No { get; set; }

        public string Features { get; set; } = string.Empty;

        public int Price { get; set; }

        public DateTime Date { get; set; }

        public bool Activity { get; set; } = false;

    }

    public class UpdateCarsRequest
    {

        public Guid CarId { get; set; }

        public string Brand { get; set; }

        public string Classes { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Model_No { get; set; }

        public string Features { get; set; } = string.Empty;

        public int Price { get; set; }

        public DateTime Date { get; set; }
        public bool Activity { get; set; } = false;

    }
}
