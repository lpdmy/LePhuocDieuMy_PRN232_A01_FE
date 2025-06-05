﻿namespace LePhuocDieuMy_PRN232_A01_FE.DTOs
{
    public class NewsArticleDTO
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string NewsContent { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool NewsStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<int> TagIds { get; set; } = new(); // <-- Thêm dòng này
        public int AccountId { get; set; }


    }

}
