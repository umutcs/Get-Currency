using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltinKaynakASMXGetCurrency.Entities
{
    public class Currency
    {
        public int ID { get; set; }
        public string Kod { get; set; }
        public string Aciklama { get; set; }
        public string Alis { get; set; }
        public string Satis { get; set; }
        public string GuncellenmeZamani { get; set; }
    }
}
