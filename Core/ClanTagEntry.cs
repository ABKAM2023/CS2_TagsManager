namespace TagsManager;

public class ClanTagEntry
{
    public string Tag { get; }
    public int Priority { get; }

    public ClanTagEntry(string tag, int priority)
    {
        Tag = tag;
        Priority = priority;
    }
}
