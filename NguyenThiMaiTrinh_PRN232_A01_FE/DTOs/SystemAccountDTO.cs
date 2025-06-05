namespace NguyenThiMaiTrinh_PRN232_A01_FE.DTOs
{
    public class SystemAccountDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = "";
        public string AccountEmail { get; set; } = "";
        public string AccountPassword { get; set; } = "";

        public int AccountRole { get; set; }
    }
}
