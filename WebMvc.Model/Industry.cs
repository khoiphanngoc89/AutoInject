using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMvc.Model
{
    public interface IIndustry
    {
        void Error();
    }

    public class Industry : IIndustry
    {
        public void Error()
        {
            throw new NotImplementedException();
        }
    }
}
