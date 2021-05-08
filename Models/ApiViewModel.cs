using System;

namespace Models
{
    public class ApiViewModel
    {
        public string msg { get; set; }
        public ApiViewModelStatus status { get; set; }
    }

    public enum ApiViewModelStatus
    {
        Success = 0,
        Error = -1,
        Exception = -2,
        NotExists = 1
    }
}
