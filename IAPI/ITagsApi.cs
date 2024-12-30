using CounterStrikeSharp.API.Core;

public interface ITagsApi
{
    /// <summary>
    /// Sets (or updates) a clan tag for the specified player with the given priority.
    /// Notes:
    /// 1. If the player already has a clan tag with the same priority, it will be replaced by the new one.
    /// 2. If the player has multiple clan tags with different priorities,
    ///    the one with the highest "weight" of priority will be displayed. 
    ///    However, by your logic, the LOWER the number, the HIGHER the priority.
    ///    For example, priority 1 is "more important" than priority 2, and so on.
    /// </summary>
    /// <param name="player">
    /// The player for whom the clan tag will be set (or updated).
    /// </param>
    /// <param name="clanTag">
    /// The string representing the clan tag (e.g., "MyClan -").
    /// If an empty string <c>""</c> is specified, a "blank" clan tag will be set.
    /// </param>
    /// <param name="priority">
    /// A numeric value determining the priority (importance) of the clan tag.
    /// The LOWER this value, the HIGHER the priority.
    /// For example, priority 1 is displayed "earlier" (more important) than priority 2.
    /// </param>
    void SetClanTag(CCSPlayerController player, string clanTag, int priority);

    /// <summary>
    /// Removes a specific clan tag with the given priority from a player.
    /// If no tag with the same name and priority is found among the player's tags,
    /// nothing will be removed.
    /// </summary>
    /// <param name="player">
    /// The player whose clan tag needs to be removed.
    /// </param>
    /// <param name="clanTag">
    /// The specific clan tag that was previously set using
    /// <see cref="SetClanTag(CCSPlayerController, string, int)"/>.
    /// It is important that the tag name (including case, spaces, and symbols) matches exactly.
    /// </param>
    /// <param name="priority">
    /// The priority of the clan tag to be removed.
    /// It must match the priority specified during setup.
    /// (For example, if the tag was set with priority 1, then 1 must be specified here.)
    /// </param>
    void RemoveClanTag(CCSPlayerController player, string clanTag, int priority);

    /// <summary>
    /// Returns the priority of a specific clan tag for a player, if such a tag exists.
    /// </summary>
    /// <param name="player">
    /// The player whose clan tags are being checked.
    /// </param>
    /// <param name="clanTag">
    /// The specific clan tag for which the priority needs to be retrieved.
    /// </param>
    /// <returns>
    /// A numeric priority value if the tag is found for the player, otherwise <c>null</c>.
    /// Note that in this logic, the LOWER the number, the HIGHER the priority.
    /// </returns>
    int? GetClanTagPriority(CCSPlayerController player, string clanTag);
}
