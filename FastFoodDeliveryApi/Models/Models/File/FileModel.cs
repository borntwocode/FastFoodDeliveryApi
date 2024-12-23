namespace FastFoodDeliveryApi.Models.Models.File;

public class FileModel
{
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string ContentBase64 { get; set; }
}