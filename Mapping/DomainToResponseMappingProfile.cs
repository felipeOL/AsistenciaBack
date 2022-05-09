using AutoMapper;

namespace AsistenciaBack.Mapping;

public class DomainToResponseMappingProfile : Profile
{
	public DomainToResponseMappingProfile()
	{
		this.CreateMap<User, UserResponse>();
		this.CreateMap<Course, CourseResponse>()
			.ForMember(destinationMember => destinationMember.UserResponses, options => options.MapFrom(
				sourceMember => sourceMember.Users
			));
		this.CreateMap<Clazz, ClazzResponse>();
	}
}