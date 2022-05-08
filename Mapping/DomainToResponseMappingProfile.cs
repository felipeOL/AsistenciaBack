using AutoMapper;

namespace AsistenciaBack.Mapping;

public class DomainToResponseMappingProfile : Profile
{
	public DomainToResponseMappingProfile()
	{
		this.CreateMap<Course, CourseResponse>();
		this.CreateMap<Clazz, ClazzResponse>();
	}
}