using System.Text;

namespace CI.TMS.Claim.API.Helper
{
    public static class FileUploadHelper
    {
        public static StreamContent GetStreamContent(this IFormFile f)
        {
            var fileStream = f.OpenReadStream();
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.Add("Content-Disposition",
                new string(Encoding.UTF8.GetBytes($"form-data; name=\"file\"; filename=\"{f.FileName}\"").
                Select(b => (char)b).ToArray()));

            return streamContent;
        }
    }
}
