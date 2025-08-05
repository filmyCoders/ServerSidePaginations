using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Response
{
    public class CarResponse
    {
        public Guid CarId { get; set; }
        public string Brand { get; set; }
        public string Classes { get; set; }
        public string Model { get; set; }
        public int? Model_No { get; set; }
        public string Features { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public bool Activity { get; set; }
    }
}
