using System;

namespace johns_lyng_app.Server.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }


    public class OperationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AiResult
    {      
       public string Message { get; set; }
    }
}
