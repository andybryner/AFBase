using System;
using System.Collections.Generic;
using System.Text;

namespace MiAccount.Models.Response
{
    class GetAccountResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? TokenTimeout { get; set; }
    }
}
