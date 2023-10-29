using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Role : BaseEntity
{
    [MaxLength(256)]
    public string? Code { get; set; }
}