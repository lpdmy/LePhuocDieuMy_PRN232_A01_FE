namespace NguyenThiMaiTrinh_PRN232_A01_FE.Models
{
    public class ODataResult<T>
    {
        public List<T> Value { get; set; } = new();
    }
}
