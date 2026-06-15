namespace KajTest.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        // User Relation
        public int UserId { get; set; }
        public User ? user { get; set; }

        // Task Relation
        public int TaskId { get; set; }
        public TaskItem ? TaskItem { get; set; }

    }
}
