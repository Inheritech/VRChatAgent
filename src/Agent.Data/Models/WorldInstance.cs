using Microsoft.EntityFrameworkCore;

namespace Agent.Data.Models {
    [Index(nameof(WorldId))]
    public record WorldInstance(string WorldId, string WorldInstanceId);
}
