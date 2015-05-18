using System.Web;

namespace Web
{
    public class UploadNDependTrend : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count == 1)
            {
                var file = context.Request.Files[0];
                context.Response.Write("You uploaded " + file.FileName+" with "+file.ContentLength+" bytes");

            } else {
                context.Response.Write("OK");
            }
        }
    }
}
