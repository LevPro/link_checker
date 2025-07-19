namespace LinkChecker.Core.Helpers
{
    public static class ResponseDescriptionHelper
    {
        public static string GetDescription(int code)
        {
            return code switch
            {
                200 => "OK",
                301 => "Moved Permanently",
                302 => "Found",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                _   => "Unknown"
            };
        }
    }
}