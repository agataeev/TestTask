using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User: BaseEntity
{
    [MaxLength(256)]
    public string Username { get; set; }
    [MaxLength(256)]
    public string Password { get; set; }
    [ForeignKey("RoleId")]
    public long RoleId { get; set; }
}