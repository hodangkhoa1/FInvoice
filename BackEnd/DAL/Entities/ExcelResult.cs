namespace DAL.Entities
{
    public class ExcelResult
    {
        public string IdExcelResult { get; set; }
        public DateTime ExportedDate { get; set; }
        public byte[] Source { get; set; }
        public string IdAccount { get; set; }
        public virtual Account Account { get; set; }
        public string IdExcelForm { get; set; }
        public virtual ExcelForm ExcelForm { get; set; }
    }
}
