using System.Collections.Generic;

namespace ConvertingFontApp.mdl
{
    public class MdlMergerRow
    {
        public int RowIndex { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public string OutputFileName { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
