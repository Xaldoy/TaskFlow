namespace Model.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; } = new AppUser();
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public Boolean IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
