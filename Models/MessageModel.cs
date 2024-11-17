using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class MessageModel
    {
        public string Message { get; set; }
        public string Type { get; set; } // e.g., "Success", "Error", "Info"


        public const string SuccessMessage = "Success";
        public const string ErrorMessage = "Error";
        public const string InfoMessage = "Info";
        public const string WarningMessage = "Warning";

        public MessageModel(string message, string type)
        {
            Message = message;
            Type = type;
        }

        public  static MessageModel Success(string message)
        {
            return new MessageModel(message, SuccessMessage);
        }
        public static MessageModel Error(string message)
        {
            return new MessageModel(message, ErrorMessage);
        }
        public static MessageModel Info(string message)
        {
            return new MessageModel(message, InfoMessage);
        }
        public static MessageModel Warning(string message)
        {
            return new MessageModel(message, WarningMessage);
        }
    }
}