using christenProject.Data.Services;

namespace christenProject.Models.DTOs
{
    public class ProductDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductNetPrice { get; set; }
    }
    public class ProductDTOResponse:ProductDTO
    {
        public int ID { get; set; }
    }
    public class ApiBasicResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public ApiBasicResponse(T data)
        {
            Success = true;
            Message = "Request completed successfully.";
            Data = data;
            Errors = null;
        }

        public ApiBasicResponse(string message, List<string> errors = null)
        {
            Success = false;
            Message = message;
            Data = default(T);
            Errors = errors ?? new List<string>();
        }
    }
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        //public PaginationMetadata Pagination { get; set; }

        public ApiResponse(T data/* PaginationMetadata pagination = null*/)
        {
            Success = true;
            Message = "Request completed successfully.";
            Data = data;
            Errors = null;
            //Pagination = pagination;
        }

        public ApiResponse(string message, List<string> errors = null)
        {
            Success = false;
            Message = message;
            Data = default(T);
            Errors = errors ?? new List<string>();
            //Pagination = null;
        }
    }

  

}
