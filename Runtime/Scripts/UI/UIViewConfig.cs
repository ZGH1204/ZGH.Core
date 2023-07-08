namespace ZGH.Core.UI
{
    public enum UIZOrder
    {
        Bg = -10000,
        Common = 0,
        Tip = 10000,
    }

    public class UIViewConfig
    {
        public string path;
        public UIZOrder order;
        public UIBaseView comp;
        public bool isFullScreen;
    }
}