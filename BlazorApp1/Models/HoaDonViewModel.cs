using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models
{
    public class HoaDonViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Money { get; set; }
        public DateTime? PaidDate { get; set; }
        public Status Status { get; set; }
        public int MSSV { get; set; }
        public string? PaidById { get; set; }
    }
}
