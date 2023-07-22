namespace VHV_FileSaver.Data.Models
{
    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
        User? ModifiedBy { get; set; }
        int? ModifiedById { get; set; }
    }
}