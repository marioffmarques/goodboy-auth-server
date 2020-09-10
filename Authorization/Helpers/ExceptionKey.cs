using System;

namespace Authorization
{
    public enum ExceptionKey
    {
        ERROR_GET,
        ERROR_CREATE,
        ERROR_UPDATE,
        ERROR_DELETE,
        ERROR_IN_USE
    }

    public static class ExceptionKeyHelper
    {
        public static string GetString(this ExceptionKey p)
        {
            switch (p)
            {
                case ExceptionKey.ERROR_GET:
                    return "ERROR_GET";
                case ExceptionKey.ERROR_CREATE:
                    return "ERROR_CREATE";
                case ExceptionKey.ERROR_UPDATE:
                    return "ERROR_UPDATE";
                case ExceptionKey.ERROR_DELETE:
                    return "ERROR_DELETE";
                case ExceptionKey.ERROR_IN_USE:
                    return "ERROR_IN_USE";
                default:
                    return "ERROR_GET";
            }
        }

        public static ExceptionKey GetEnum(string enumString)
        {
            switch (enumString.ToLower())
            {
                case "ERROR_GET":
                    return ExceptionKey.ERROR_GET;
                case "ERROR_CREATE":
                    return ExceptionKey.ERROR_CREATE;
                case "ERROR_UPDATE":
                    return ExceptionKey.ERROR_UPDATE;
                case "ERROR_DELETE":
                    return ExceptionKey.ERROR_DELETE;
                case "ERROR_IN_USE":
                    return ExceptionKey.ERROR_IN_USE;
                default:
                    return ExceptionKey.ERROR_GET;
            }
        }
    }
}
