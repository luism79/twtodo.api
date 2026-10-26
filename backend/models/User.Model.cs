namespace backend.models
{
    public class UserModel
    {
        public UserModel()
        {
            Active = true;
        }

        public bool Active { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Password { get; set; } = string.Empty;

        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
