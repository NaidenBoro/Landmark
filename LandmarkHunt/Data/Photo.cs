namespace LandmarkHunt.Data
{
    public class Photo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public byte[] Bytes { get; set; } = new byte[0];
        public string FileExtension { get; set; } = string.Empty;
    }
}
