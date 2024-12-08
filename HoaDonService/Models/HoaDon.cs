using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HoaDonService.Models
{
    [Table("hoadon", Schema = "db0")]
    public class HoaDon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("hoadon_id")]
        public int Id { get; set; }
        [Column("tieu_de")]
        public string Title { get; set; }
        [Column("so_tien")]
        public double Money { get; set; }
        [Column("ngay_thanh_toan")]
        public DateTime? PaidDate { get; set; }
        [Column("trang_thai")]
        public Status Status { get; set; }
        [Column("mssv")]
        public int MSSV { get; set; }
        [Column("thanh_toan_boi")]
        public string? PaidById { get; set; }
    }
}
