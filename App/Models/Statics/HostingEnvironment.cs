namespace System.Web
{

    public static class HostingEnvironment
    {
        private static Microsoft.AspNetCore.Hosting.IHostingEnvironment m_HostingEnvironment;


        public static void Configure(Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment)
        {
            m_HostingEnvironment = HostingEnvironment;
        }


        public static Microsoft.AspNetCore.Hosting.IHostingEnvironment Current
        {
            get
            {
                return m_HostingEnvironment;
            }
        }


    }


}