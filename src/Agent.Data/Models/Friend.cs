using Microsoft.EntityFrameworkCore;

namespace Agent.Data.Models {
    [PrimaryKey(nameof(UserId))]
    public record Friend(string UserId);
}
