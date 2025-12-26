namespace BeghTools.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageInfoAttribute : Attribute
    {
        public bool IsFooterPage { get; }
        public string PageTitle { get; }
        public string PageIcon { get; }
        public int Order { get; }

        public PageInfoAttribute(bool isFooterPage, string pageTitle, string pageIcon, int order = 1)
        {
            IsFooterPage = isFooterPage;
            PageTitle = pageTitle;
            PageIcon = pageIcon;
            Order = order;
        }
    }
}
