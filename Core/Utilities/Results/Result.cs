using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        //Getter readonlydir constructor içinde set edilebilir.
        //this(success) : Bu kısım aşağıdaki constructorunda çalışmasını sağlar ve dry (dont repeat yourself) kuralları dışına çıkmamış oluruz.
        public Result(bool success, string message):this(success)
        {
            Message = message;
            //Success = success; bu kısmı set etme işini aşşağıdakine veriyoruz
        }

        //Overloading - Aşırı Yükleme
        public Result(bool success)
        {          
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
