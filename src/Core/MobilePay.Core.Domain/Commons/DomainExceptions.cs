namespace MobilePay.Core.Domain.Commons;

public static class DomainExceptions
{
    public class InvalidEntityState : Exception
    {
        public IEnumerable<Error> Errors { get; set; }
        public InvalidEntityState(IEnumerable<Error> errors) =>
            Errors = errors;

        public InvalidEntityState(string error) =>
            Errors = new[] { new Error(error) };
    }

    public class InvalidEntityValue : Exception
    {
        public IEnumerable<Exception> Exceptions { get; set; }
        public InvalidEntityValue(IEnumerable<Exception> errors) =>
            Exceptions = errors;

        public InvalidEntityValue(string error) =>
            Exceptions = new[] { new Exception(error) };
    }

    public class JsonFormatNotValidException : Exception
    {
        public IEnumerable<Error> Errors { get; set; }

        public JsonFormatNotValidException(string error) =>
            Errors = new[] { new Error(error) };
    }

    public class MerchantNameNotExistException : Exception
    {
        public IEnumerable<Error> Errors { get; set; }

        public MerchantNameNotExistException(string error) =>
            Errors = new[] { new Error(error) };
    }
}
