using System.Collections.Concurrent;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;

namespace TagsManager;

public class TagManagerCore : BasePlugin
{
    public override string ModuleName => "[Clantag] Core";
    public override string ModuleVersion => "v1.0.0";

    private readonly ConcurrentDictionary<int, SortedSet<ClanTagEntry>> _playerClanTags = new();
    private readonly PluginCapability<ITagsApi> _pluginCapability = new("CLANTAG_API");
    private ITagsApi? _api;

    public override void Load(bool hotReload)
    {
        _api = new TagManagerApi(this);
        Capabilities.RegisterPluginCapability(_pluginCapability, () => _api);
        RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnected);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnected);
    }

    private HookResult OnPlayerConnected(EventPlayerConnectFull @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player != null && player.IsValid)
        {
            _playerClanTags[player.Slot] = new SortedSet<ClanTagEntry>(new ClanTagComparer());
        }
        return HookResult.Continue;
    }

    private HookResult OnPlayerDisconnected(EventPlayerDisconnect @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player != null && player.IsValid)
        {
            _playerClanTags.TryRemove(player.Slot, out _);
        }
        return HookResult.Continue;
    }

    public void SetPlayerClanTag(CCSPlayerController player, string clanTag, int priority)
    {
        if (!_playerClanTags.ContainsKey(player.Slot))
        {
            _playerClanTags[player.Slot] = new SortedSet<ClanTagEntry>(new ClanTagComparer());
        }

        var clanTags = _playerClanTags[player.Slot];
        clanTags.Add(new ClanTagEntry(clanTag, priority));
        
        var highestPriorityTag = clanTags.Max;
        if (highestPriorityTag != null)
        {
            UpdatePlayerClanTag(player, highestPriorityTag.Tag);
        }
    }

    public void RemovePlayerClanTag(CCSPlayerController player, string clanTag, int priority)
    {
        if (!_playerClanTags.ContainsKey(player.Slot)) return;

        var clanTags = _playerClanTags[player.Slot];
        var entryToRemove = clanTags.FirstOrDefault(entry => entry.Tag == clanTag && entry.Priority == priority);
        if (entryToRemove != null)
        {
            clanTags.Remove(entryToRemove);
        }
        if (clanTags.Count == 0)
        {
            UpdatePlayerClanTag(player, string.Empty);
        }
        else
        {
            var highestPriorityTag = clanTags.Max;
            if (highestPriorityTag != null)
            {
                UpdatePlayerClanTag(player, highestPriorityTag.Tag);
            }
        }
    }
    
    public int? GetPlayerClanTagPriority(CCSPlayerController player, string clanTag)
    {
        if (!_playerClanTags.ContainsKey(player.Slot)) return null;

        var clanTags = _playerClanTags[player.Slot];
        var entry = clanTags.FirstOrDefault(tag => tag.Tag == clanTag);
        return entry?.Priority;
    }

    private void UpdatePlayerClanTag(CCSPlayerController player, string? clanTag)
    {
        Server.NextFrame(() =>
        {
            player.Clan = clanTag ?? string.Empty;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
            
            var nextLevelEvent = new EventNextlevelChanged(false)
            {
                Mapgroup = string.Empty,
                Nextlevel = string.Empty,
                Skirmishmode = string.Empty
            };
            nextLevelEvent.FireEvent(false);
        });
    }
}