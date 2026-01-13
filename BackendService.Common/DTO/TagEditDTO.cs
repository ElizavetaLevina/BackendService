namespace BackendService.Common.DTO
{
    /// <summary>
    /// Редактирование тега
    /// </summary>
    public class TagEditDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
