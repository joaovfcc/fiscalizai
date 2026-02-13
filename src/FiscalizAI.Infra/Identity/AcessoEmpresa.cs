using FiscalizAI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalizAI.Infra.Identity
{
    public class AcessoEmpresa
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
