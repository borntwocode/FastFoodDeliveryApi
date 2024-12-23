using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FastFoodDeliveryApi.Models.Entities;

public class File
{
    [Key]
    public Guid FileId { get; set; }

    [JsonIgnore]
    public byte[] Content { get; set; }

    public string Extension { get; set; }

    public string FileName { get; set; }

    public long FileSize { get; set; }

}