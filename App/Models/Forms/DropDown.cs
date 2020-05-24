namespace Androtomist.Models.Forms
{
    public class DropDown
    {
        public DropDown()
        {
            items = new DropDownItem[] { };
        }

        public DropDownItem[] items { get; set; }
        public int page_length { get; set; }
        public int total_count { get; set; }
    }

    public class DropDownItem
    {
        public DropDownItem(string id, string text, bool selected = false, string extra_id = "")
        {
            this.id = id;
            this.text = text;
            this.selected = selected ? 1 : 0;
            this.extra_id = extra_id;
        }

        public string id { get; set; }
        public string text { get; set; }
        public int selected { get; set; }
        public string extra_id { get; set; }
    }

}
