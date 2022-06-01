using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using Flash.Central.Dtos.Camera;
using Flash.Central.Dtos.DetectionEvent;
using Flash.Central.Dtos.GasStation;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Dtos.Visit;
using Flash.Central.ViewModel.Camera;
using Flash.Central.ViewModel.DetectionEvent;
using Flash.Central.ViewModel.GasStation;
using Flash.Central.ViewModel.RecognitionEvent;
using Flash.Central.ViewModel.Visit;

namespace Flash.Central.Core.Configuration
{
    /// <summary>
    /// Class. Determines AutoMapper profile
    /// </summary>
    public class DtoToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates mapping DTO classes and ViewModels classes
        /// </summary>
        public DtoToViewModelMappingProfile()
        {
            CreateMap<CameraDto, CameraVm>()
                .ForMember(x => x.Regions,
                    _ => _.MapFrom(x => x.Regions.Where(
                        region => !region.IsDeleted)));

            CreateMap<CameraRegionDto, CameraRegionVm>();
            CreateMap<VisitDto, VisitVm>();
            CreateMap<GasStationDto, GasStationVm>();
            CreateMap<TerminalDto, TerminalVm>();
            CreateMap<RecognitionEventDto, RecognitionEventVm>();
            CreateMap<DetectionEventDto, DetectionEventVm>();
        }
    }
}
