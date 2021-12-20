namespace RequestService.Repo.EntityFramework.Entities
{
    public class EnumJobStatusChangeReasonCodes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool TriggersStatusChange { get; set; }
    }
}
