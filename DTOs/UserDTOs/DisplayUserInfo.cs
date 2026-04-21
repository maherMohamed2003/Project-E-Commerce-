namespace E_Commerce_Proj.DTOs.UserDTOs
{
    public class DisplayUserInfo
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public bool isBlocked { get; set; }
    }
}
