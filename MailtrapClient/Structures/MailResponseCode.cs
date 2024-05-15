namespace Mailtrap;

public enum MailResponseCode
{
    // Message has been delivered.
    Success = 200,
    // Fix errors listed in response before retrying.
    BadRequest = 400,
    // Make sure you are sending correct credentials with the request before retrying.
    Unauthorized = 401,
    // Make sure domain verification process is completed.
    Forbidden = 403,
    // Mail was not delivered. Retry later or contact support.
    InternalError = 500
}
