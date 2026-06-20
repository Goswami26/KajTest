namespace KajTest.DTOs.ProjectDtos
{
    public class ProjectResponseDTO
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }  = string.Empty;

        public string ProjectDescription { get; set; } = string.Empty;

        public string CreatorName { get; set; } = string.Empty;
    }
}
