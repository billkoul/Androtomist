namespace Androtomist.Models.Global
{
    public class ProjectProperties
    {
        public string Author { get; set; }
        public string Company { get; set; }
        public string URL { get; set; }

        public string Name { get; set; }
        public string Verions { get; set; }

        public ProjectProperties()
        {
            Author = "EMISIA PEMS";
            Company = "EMISIA";
            URL = "https://www.emisia.com/";

            Name = "Emisia PEMS";
            Verions = "1.0";
        }
    }
}