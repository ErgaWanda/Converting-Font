using System;

namespace ConvertingFontApp.events
{
    public class ProgressEventArgs : EventArgs
    {
        public int Percentage { get; set; }
        public string Message { get; set; }
        public string CurrentFile { get; set; }
        public bool IsCompleted { get; set; }
        public bool HasError { get; set; }

        public ProgressEventArgs(int percentage, string message, string currentFile = "", bool isCompleted = false, bool hasError = false)
        {
            Percentage = percentage;
            Message = message;
            CurrentFile = currentFile;
            IsCompleted = isCompleted;
            HasError = hasError;
        }
    }
}
