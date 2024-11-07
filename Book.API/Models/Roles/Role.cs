using System.Text.Json.Serialization;

namespace Book.API.Models.Roles;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role : byte
{
    USER = 1,
    ADMIN = 2,
    SELLER = 3,
}
