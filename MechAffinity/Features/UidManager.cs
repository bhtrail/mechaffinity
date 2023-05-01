namespace MechAffinity;

public static class UidManager
{
    private static int uid = -1;

    public static int Uid
    {
        get
        {
            uid++;
            return uid;
        }
    }

    public static void reset()
    {
        uid = -1;
    }
}
