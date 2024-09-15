using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace carnival_computer.Areas.Identity.Data;

public class carnival_computerUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public required string Name { get; set; }
    [PersonalData]
    [AllowNull]
    [Column(TypeName = "nvarchar(100)")]
    public string Role { get; set; } = "User";
    [PersonalData]
    [AllowNull]
    [Column(TypeName = "nvarchar(100)")]
    public string ProfilePic { get; set; } = string.Empty;
}

