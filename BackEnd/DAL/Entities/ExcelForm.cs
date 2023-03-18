namespace DAL.Entities
{
    public class ExcelForm
    {
        public string IdExcelForm { get; set; }
        public string Name { get; set; }
        public byte[] Source { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IdAccountingSoftware { get; set; }
        public virtual AccountingSoftware AccountingSoftware { get; set; }

        public virtual IEnumerable<ExcelResult> ExcelResults { get; set; }
    }
}
