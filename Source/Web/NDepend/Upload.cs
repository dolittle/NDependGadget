using System.Web;
using Bifrost.Configuration;

namespace Web.NDepend
{
    public class Upload : IHttpHandler
    {
        IResults _results;

        public bool IsReusable
        {
            get { return true; }
        }

        public Upload() : this(Configure.Instance.Container.Get<IResults>()) { }

        public Upload(IResults results)
        {
            _results = results;
        }


        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count == 1)
            {
                var file = context.Request.Files[0];

                var buildIdentifier = context.Request.Form["BuildIdentifier"];
                if (string.IsNullOrEmpty(buildIdentifier))
                {
                    buildIdentifier = "Default";
                }

                _results.MergeWith(buildIdentifier, file.InputStream);
            } else {
                context.Response.Write("OK");
            }
        }
    }
}
