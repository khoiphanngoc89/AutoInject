using System;

namespace WebCore.Model
{
    public interface IStudent
    {
        void ShowError();
    }

    public class Student : IStudent
    {
        public void ShowError()
        {
            throw new NotImplementedException();
        }
    }
}
