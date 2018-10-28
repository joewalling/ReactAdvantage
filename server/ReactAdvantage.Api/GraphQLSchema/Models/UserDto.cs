using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Api.GraphQLSchema.Models
{
    public class UserDto
    {
        public UserDto()
        {
        }

        public UserDto(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Email = user.Email;
            IsActive = user.IsActive;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string[] Roles { get; set; }
    }
}
