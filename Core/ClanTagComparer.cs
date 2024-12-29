namespace TagsManager;

public class ClanTagComparer : IComparer<ClanTagEntry>
{
    public int Compare(ClanTagEntry? x, ClanTagEntry? y)
    {
        if (x == null || y == null) return 0;

        int priorityComparison = y.Priority.CompareTo(x.Priority);
        return priorityComparison != 0 ? priorityComparison : x.Tag.CompareTo(y.Tag);
    }
}
