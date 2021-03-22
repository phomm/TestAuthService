using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class StorageException: Exception
    {
        public StorageException(string message, Exception innerException)
            : base(message, innerException) { }

    }

}
