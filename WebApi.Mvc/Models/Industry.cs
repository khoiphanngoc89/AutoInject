using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Mvc.Models
{
    public interface IIndustry
    {
        void Show();
    }
    public class Industry : IIndustry
    {
        public void Show()
        {
            throw new NotImplementedException();
        }
    }
}