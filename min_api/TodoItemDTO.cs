namespace min_api
{
    // To hide the secret from the model
    public class TodoItemDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        public TodoItemDTO() { }
        public TodoItemDTO(Todo todo) => (Id, Name, IsComplete) 
                                            = (todo.Id, todo.Name, todo.IsComplete);;
    }
}
