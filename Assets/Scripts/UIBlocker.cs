public static class UIBlocker
{
    private static int openPanels = 0;

    public static bool isBlocking => openPanels > 0;

    public static void Open() => openPanels++;
    public static void Close() => openPanels = openPanels > 0 ? openPanels - 1 : 0;
    public static void ForceClose() => openPanels = 0;
}
