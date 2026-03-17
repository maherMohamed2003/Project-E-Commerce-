namespace E_Commerce_Proj.Authentication
{
    public class JWTOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int Duration { get; set; }

    }
}
