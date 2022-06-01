using AutoMapper;
using Flash.Central.Dtos.Camera;
using Flash.Central.Dtos.DetectionEvent;
using Flash.Central.Dtos.GasStation;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Dtos.Visit;
using Flash.Central.Foundation.Base.Models;
using Flash.Domain.Entities;

namespace Flash.Central.Data.Configuration
{
    /// <summary>
    /// Class. Determines AutoMapper profile
    /// </summary>
    public class EntityToDtoMappingProfile : Profile
    {
        /// <summary>
        /// Creates mapping Entities classes and DTO classes
        /// </summary>
        public EntityToDtoMappingProfile()
        {
            CreateMap<Camera, CameraDto>();

            CreateMap<CameraRegion, CameraRegionDto>();

            CreateMap<RecognitionEvent, RecognitionEventDto>()
                .ForMember(x => x.CameraRegionName, _ => _.MapFrom(x => x.CameraRegion.RegionName))
                .ForMember(x => x.GasStationId, _ => _.MapFrom(x => x.CameraRegion.Camera.GasStationId))
                .ForMember(x => x.CameraId, _ => _.MapFrom(x => x.CameraRegion.CameraId));

            CreateMap<DetectionEvent, DetectionEventDto>()
                .ForMember(x => x.GasStationId, _ => _.MapFrom(x => x.CameraRegion.Camera.GasStationId))
                .ForMember(x => x.CameraId, _ => _.MapFrom(x => x.CameraRegion.CameraId));

            CreateMap<Visit, VisitDto>()
                .ForMember(x => x.FullImageLink, _ => _.Ignore());

            CreateMap<Terminal, TerminalDto>();

            CreateMap<GasStation, GasStationDto>();
        }
    }
}
