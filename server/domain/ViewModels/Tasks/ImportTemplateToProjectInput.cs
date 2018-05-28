namespace ReactAdvantage.Domain.ViewModels.Tasks
{
    public class ImportTemplateToProjectInput
    {
        public string TempFileId { get; set; }
        public string FileContent { get; set; }
        public int ProjectId { get; set; }
    }
}
