namespace AdaStore.UI.UI
{
    public class TableColumn
    {
        public string PropName { get; set; }
        public string DisplayName { get; set; }
        public string OptionalStyles { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDesc { get; set; }
        public bool NotOrder { get; set; }
        public string Style
        {
            get
            {
                if (NotOrder)
                {
                    return OptionalStyles;
                }
                else
                {
                    var defaultClass = "th-order";

                    if (!IsSelected)
                    {
                        return $"{defaultClass} order-disabled";
                    }
                    else if (IsDesc)
                    {
                        return $"{defaultClass} order-desc";
                    }
                    else
                    {
                        return $"{defaultClass} order-asc";
                    }
                }
            }
        }
    }
}
