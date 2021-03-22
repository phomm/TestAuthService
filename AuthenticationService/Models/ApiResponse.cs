using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    [Serializable]
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } 
        
        public ApiResponse(object data)
        {
            Code = 0;
            Message = null;
            Data = data;
        }
        
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var sample = obj as ApiResponse;
            if (Code == sample.Code && Message == sample.Message && Equals(Data, sample.Data))
                return true;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Message, Data);
        }
    }
}
