using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CodeLinkBLL
    {
        public CodeLink Save(CodeLink codeLink)
        {
            return new CodeLinkDAL().Save(codeLink);
        }

        public CodeLink Update(CodeLink codeLinkUpdate, CodeLink codeLinkOrigin)
        {
            return new CodeLinkDAL().Update(codeLinkUpdate, codeLinkOrigin);
        }

        public static CodeLink GetByCode(string code)
        {
            return CodeLinkDAL.GetByCode(code);
        }
        public void MarkCodeAsNotUsed(string code)
        {
            new CodeLinkDAL().MarkCodeAsNotUsed(code);
        }

    }
}
