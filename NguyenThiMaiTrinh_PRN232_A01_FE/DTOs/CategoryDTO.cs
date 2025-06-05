namespace NguyenThiMaiTrinh_PRN232_A01_FE.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
