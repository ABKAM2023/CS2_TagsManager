using CounterStrikeSharp.API.Core;
using TagsManager;

public class TagManagerApi : ITagsApi
{
    private readonly TagManagerCore _core;

    public TagManagerApi(TagManagerCore core)
    {
        _core = core;
    }

    public void SetClanTag(CCSPlayerController player, string clanTag, int priority)
    {
        _core.SetPlayerClanTag(player, clanTag, priority);
    }

    public void RemoveClanTag(CCSPlayerController player, string clanTag, int priority)
    {
        _core.RemovePlayerClanTag(player, clanTag, priority);
    }

    public int? GetClanTagPriority(CCSPlayerController player, string clanTag)
    {
        return _core.GetPlayerClanTagPriority(player, clanTag);
    }
}