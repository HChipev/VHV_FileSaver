namespace VHV_FileSaver.ViewModels.ResponseModels
{
    public class BaseResponseViewModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

