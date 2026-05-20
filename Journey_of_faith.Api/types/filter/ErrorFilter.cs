using HotChocolate.Execution;
using Journey_of_faith.Application.exceptions;

namespace Journey_of_faith.Api.types.filter;


public class GlobalErrorFilter : IErrorFilter
{
    public IError OnError(IError erro)
    {
       return erro.Exception switch
       {
           NotFoundException => erro.WithMessage("Không tìm thấy tài nguyên yêu cầu.").WithCode("NOT_FOUND"),
           ForbiddenException => erro.WithMessage("Bạn không có quyền truy cập tài nguyên này.").WithCode("FORBIDDEN"),
           UnauthorizedAccessException => erro.WithMessage("Bạn cần đăng nhập để truy cập tài nguyên này.").WithCode("UNAUTHORIZED"),
           _ => erro.WithMessage("Đã xảy ra lỗi không xác định. Vui lòng thử lại sau.").WithCode("INTERNAL_ERROR")
       };
    }   
}