using AutoMapper;

namespace AsistenciaBack.Mapping;

public class RequestToDomainMappingProfile : Profile
{
	public RequestToDomainMappingProfile()
	{
		this.CreateMap<CourseRequest, Course>();
		this.CreateMap<ClazzRequest, Clazz>();
	}
}